using Godot;
using System;
using System.Collections.Generic;

public class ObjectPool<T>
{
    public List<(T, int)> pool = [];         //Pool of random things to grab from.
                                             //Includes element of type T and the parent WeightedEntry's index in entries.

    public List<WeightedEntry> entries = []; //Where our entries belong even if they are gone from the pool

    /// <summary>
    /// Provide a list of values and a same-length list of weights
    /// </summary>
    /// <param name="values"></param>
    /// <param name="weights"></param>
    /// <exception cref="ArgumentException"></exception>
    public ObjectPool(T[] values, int[] weights)
    {
        if (values.Length != weights.Length)
        {
            throw new ArgumentException($"objPool constructor expects 2 arrays of equal length. Array 1 length: {values.Length}, Array 2 length: {weights.Length}");
        }

        for (int i = 0; i < values.Length; i++)
        {
            AddNewEntry(values[i], weights[i], i);
        }
    }

    /// <summary>
    /// Provide a list of values and they will all share the same specified weight
    /// </summary>
    /// <param name="values"></param>
    /// <param name="weight"></param>
    public ObjectPool(T[] values, int weight)
    {
        for (int i = 0; i < values.Length; i++)
        {
            AddNewEntry(values[i], weight, i);
        }
    }

    /// <summary>
    /// Use for single element pools
    /// </summary>
    /// <param name="value"></param>
    public ObjectPool(T value)
    {
        AddNewEntry(value, 1, 0);
    }

    /// <summary>
    /// Use to create an empty pool and add elements later
    /// </summary>
    public ObjectPool() { }

    /// <summary>
    /// Get a random object as well as its entry as out parameter
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    public T GetRandom(out WeightedEntry entry)
    {
        int index = GD.RandRange(0, pool.Count);

        var randomTuple = pool[index];

        T obj = randomTuple.Item1;
        entry = entries[randomTuple.Item2];
        return obj;
    }

    /// <summary>
    /// Get random object from pool without entry
    /// </summary>
    /// <returns></returns>
    public T GetRandom()
    {
        int index = GD.RandRange(0, pool.Count);

        var randomTuple = pool[index];

        T obj = randomTuple.Item1;
        return obj;
    }

    /// <summary>
    /// Adds new entry and creates objects in pool to match weight
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="weight"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public WeightedEntry AddNewEntry(T entry, int weight, int index)
    {
        WeightedEntry newEntry = new(this, entry, weight, index);
        entries.Add(newEntry);
        return newEntry;
    }

    /// <summary>
    /// Delete an entry and repopulate the pool to fix stale indexes. Intensive, try to avoid!
    /// </summary>
    public void DeleteEntry(WeightedEntry entry)
    {
        entries.Remove(entry);

        Repopulate();
    }

    /// <summary>
    /// Delete multiple entries, then repopulate pool
    /// </summary>
    /// <param name="entriesToRemove"></param>
    public void DeleteEntry(WeightedEntry[] entriesToRemove)
    {
        foreach(var entry in entriesToRemove)
        {
            entries.Remove(entry);
        }

        Repopulate();
    }

    /// <summary>
    /// Remove and readd all objects in pool, used to update values
    /// </summary>
    void Repopulate()
    {
        pool.Clear();

        for (int i = 0; i < entries.Count; i++)
        {
            entries[i].index = i;
            entries[i].ReaddPoolElements();
        }
    }

    /// <summary>
    /// Remove every entry
    /// </summary>
    public void ClearPool()
    {
        pool.Clear();
        entries.Clear();
    }

    public class WeightedEntry
    {
        readonly ObjectPool<T> parent;

        readonly T entry;
        public int weight;

        //This index links back to the entry in entries
        //Used for objects in the pool so you can pull the entry from them
        internal int index;

        internal WeightedEntry(ObjectPool<T> parent, T entry, int weight, int index)
        {
            this.parent = parent;
            this.entry = entry;
            this.index = index;
            ReaddPoolElements();
        }

        public void AddWeight(int value)
        {
            for (int i = 0; i < value; i++)
            {
                weight += 1;
                if (weight > 0)
                {
                    parent.pool.Add((entry, index));
                }
            }
        }

        /// <summary>
        /// Remove value from weight. Can go below zero!
        /// </summary>
        /// <param name="value"></param>
        public void SubtractWeight(int value)
        {
            for (int i = 0; i < value; i++)
            {
                if (weight > 0) { parent.pool.Remove((entry, index)); }
                weight -= 1;
            }
        }

        /// <summary>
        /// Set weight equal to value
        /// </summary>
        /// <param name="value"></param>
        public void SetWeight(int value)
        {
            //First remove every entry to make sure no desyncs occur.
            for (int i = 0; i < weight; i++)
            {
                parent.pool.Remove((entry, index));
            }

            //Now add every entry back, to reach the specified value
            for (int i = 0; i < value; i++)
            {
                parent.pool.Add((entry, index));
            }
            weight = value;
        }

        /// <summary>
        /// Readds all values from pool. Assumes we have already deleted the stale values
        /// </summary>
        public void ReaddPoolElements()
        {
            for(int i = 0; i < weight; i++)
            {
                parent.pool.Add((entry, index));
            }
        }

        /// <summary>
        /// Set entry weight to zero. To fully delete entry, use ObjectPool method DeleteEntry instead.
        /// </summary>
        /// <param name="deleteEntry"></param>
        public void RemoveEntry()
        {
            for (int i = 0; i < weight; i++)
            {
                parent.pool.Remove((entry, index));
            }

            weight = 0;
        }
    }
}
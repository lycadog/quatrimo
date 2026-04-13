using Godot;
using System;
using System.Collections.Generic;

public class ObjectPool<T>
{
    public List<WeightedEntry> pool = [];    //Pool of random things to grab from.
                                             //Includes element of type T and the parent WeightedEntry's index in entries.

    public List<WeightedEntry> entries = []; //Where our entries belong even if they are gone from the pool

    /// <summary>
    /// Provide an array of values and a same-length list of weights
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
            AddNewEntry(values[i], weights[i]);
        }
    }

    /// <summary>
    /// Provide a list of values and a same-length list of weights
    /// </summary>
    /// <param name="values"></param>
    /// <param name="weights"></param>
    /// <exception cref="ArgumentException"></exception>
    public ObjectPool(List<T> values, int[] weights)
    {
        if (values.Count != weights.Length)
        {
            throw new ArgumentException($"objPool constructor expects a list and an array of equal length. List 1 length: {values.Count}, Array 2 length: {weights.Length}");
        }

        for (int i = 0; i < values.Count; i++)
        {
            AddNewEntry(values[i], weights[i]);
        }
    }

    /// <summary>
    /// Provide a list of values and they will all share the same specified weight
    /// </summary>
    /// <param name="values"></param>
    /// <param name="weight"></param>
    public ObjectPool(T[] values, int weight)
    {
        foreach(T t in values)
        {
            AddNewEntry(t, weight);
        }
    }

    /// <summary>
    /// Use for single element pools
    /// </summary>
    /// <param name="value"></param>
    public ObjectPool(T value)
    {
        AddNewEntry(value, 1);
    }

    /// <summary>
    /// Use to create an empty pool and add elements later
    /// </summary>
    public ObjectPool() { }

    /// <summary>
    /// Get random object from pool
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    public T GetRandom()
    {
        if (pool.Count == 0)
        {
            GD.PushWarning("ObjectPool.GetRandom called with empty pool! Random entry from entries returned instead");
            return entries[GD.RandRange(0, entries.Count - 1)].heldObject;
        }

        int index = GD.RandRange(0, pool.Count - 1);

        return pool[index].heldObject;
    }

    /// <summary>
    /// Get random entry from pool
    /// </summary>
    /// <returns></returns>
    public WeightedEntry GetRandomEntry()
    {
        if (pool.Count == 0)
        {
            GD.PushWarning("ObjectPool.GetRandom called with empty pool! Random entry from entries returned instead");
            return entries[GD.RandRange(0, entries.Count - 1)];
        }

        int index = GD.RandRange(0, pool.Count - 1);

        return pool[index];
    }

    /// <summary>
    /// Adds new entry and creates objects in pool to match weight
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="weight"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public WeightedEntry AddNewEntry(T entry, int weight)
    {
        WeightedEntry newEntry = new(this, entry, weight);
        entries.Add(newEntry);
        return newEntry;
    }

    /// <summary>
    /// Clears the pool and readds every entry to it
    /// </summary>
    public void RepopulatePool()
    {
        pool.Clear();
        
        foreach(var entry in entries)
        {
            entry.AddToPool();
        }
    }

    /// <summary>
    /// Remove every entry
    /// </summary>
    public void Clear()
    {
        pool.Clear();
        entries.Clear();
    }

    /// <summary>
    /// Holds a weight and an entry, controls its own destiny in its pool through its weight methods
    /// </summary>
    public class WeightedEntry
    {
        readonly ObjectPool<T> parent;

        public readonly T heldObject;
        public int weight;

        internal WeightedEntry(ObjectPool<T> parent, T entry, int weight)
        {
            this.parent = parent;
            heldObject = entry;
            this.weight = weight;
            AddToPool();
        }

        /// <summary>
        /// Increase weight by value
        /// </summary>
        /// <param name="value"></param>
        public void AddWeight(int value)
        {
            for (int i = 0; i < value; i++)
            {
                weight += 1;
                if (weight > 0)
                {
                    parent.pool.Add(this);
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
                if (weight > 0) { parent.pool.Remove(this); }
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
            //Could probably improve this TODO
            for (int i = 0; i < weight; i++)
            {
                parent.pool.Remove(this);
            }

            //Now add every entry back, to reach the specified value
            for (int i = 0; i < value; i++)
            {
                parent.pool.Add(this);
            }
            weight = value;
        }

        /// <summary>
        /// Add to pool for every weight. ONLY use if not currently in pool
        /// </summary>
        internal void AddToPool()
        {
            for(int i = 0; i < weight; i++)
            {
                parent.pool.Add(this);
            }
        }

        /// <summary>
        /// Removes every entry from pool and deletes the entry
        /// </summary>
        /// <param name="deleteEntry"></param>
        public void RemoveEntry()
        {
            for (int i = 0; i < weight; i++)
            {
                parent.pool.Remove(this);
            }

            parent.entries.Remove(this);
        }
    }
}
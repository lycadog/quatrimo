using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quatrimo
{
    public class objPool<T>
    {
        public List<T> pool = [];
        public List<weightedEntry> entries = [];

        public objPool(T[] values, int[] weights)
        {
            if (values.Length != weights.Length)
            {
                throw new ArgumentException($"objPool constructor expects 2 arrays of equal length. Array 1 length: {values.Length}, Array 2 length: {weights.Length}");
            }

            for (int i = 0; i < values.Length; i++)
            {
                addNewEntry(values[i], weights[i]);
            }
        }

        //for single element pools
        public objPool(T value)
        {
            addNewEntry(value);
        }

        //add elements later
        public objPool() { }

        public T getRandom()
        {
            Debug.WriteLine("POOL SIZE: " + pool.Count);
            int index = util.rand.Next(0, pool.Count);
            Debug.WriteLine("INDEX: " + index);

            T obj = pool[index];
            return obj;
        }

        public weightedEntry addNewEntry(T entry, int weight = 1)
        {
            Debug.WriteLine("NEW ENTRY ADDED");
            weightedEntry newEntry = new weightedEntry(this, entry, weight);
            entries.Add(newEntry);
            return newEntry;
        }

        public void clearPool()
        {
            foreach(var entry in entries)
            {
                entry.removeEntry();
            }
        }

        public class weightedEntry
        {
            objPool<T> parent;

            T entry;
            public int weight;

            internal weightedEntry(objPool<T> parent, T entry, int weight)
            {
                this.parent = parent;
                this.entry = entry;
                setWeight(weight);
            }

            public void addWeight(int value)
            {
                for(int i = 0; i < value; i++)
                {
                    weight += 1;
                    if (weight > 0)
                    {
                        parent.pool.Add(entry);
                    }
                    
                }
            }

            public void subtractWeight(int value)
            {
                for (int i = 0; i < value; i++)
                {
                    if(weight > 0) { parent.pool.Remove(entry); }
                    weight -= 1;
                }
            }
            public void setWeight(int value)
            {
                for(int i = 0; i < weight; i++)
                {
                    parent.pool.Remove(entry);
                }

                for(int i = 0; i < value; i++)
                {
                    parent.pool.Add(entry);
                }
                weight = value;
            }

            public void removeEntry()
            {
                for(int i = 0; i < weight; i++)
                {
                    parent.pool.Remove(entry);
                }
                parent.entries.Remove(this);
                weight = 0;
            }
        }
    }
}

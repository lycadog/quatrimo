namespace Quatrimo
{
    /// <summary>
    /// Create a weighted set for any type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class wSet<T>
    {
        T[] items;
        float[] weight;
        public wSet(T[] items, float[] weight)
        {
            this.items = items;
            this.weight = weight;
        }

        public T getRandom()
        {

            float rand = util.rand.NextSingle(); //rework this entire function later
            int index = 0;

            for (int i = 0; i < items.Length; i++)
            { //if random number is lower than the weighted chance, it will succeed
                if (rand < weight[i])
                {
                    index = i;
                }
            }
            return items[index];
        }
    }
}

public class tileSet
{
    public tileSet(tileTypeold[] types, float[] chance)
    {
        this.types = types;
        this.chance = chance;
    }

    public tileTypeold[] types { get; set; }
    public float[] chance {  get; set; } //chance of getting the tiletype with the same index in types array, WRITE LARGEST TO SMALLEST
    public tileTypeold getRandomType() //rework this later to use real weighted chance!!!!!
    {
        float rand = util.rand.NextSingle(); //rework this entire function later
        int index = 0;

        for (int i = 0; i < types.Length; i++) { //if random number is lower than the tile chance, it will succeed
            if (rand < chance[i])
            {
                index = i;
            }
        }
        return types[index];
    }
    public tileTypeold getStarterType(pieceType piece, bag bag)
    {
        return null;
    }
}
using Godot;
using System.Collections.Generic;
using System.Xml.Linq;

public class PooledPieceDefinition : PieceDefinition 
{
    static ObjectPool<PieceType> BasicPool = new ObjectPool<PieceType>(PieceType.Basic);

    ObjectPool<PieceType> PiecePool;
    ObjectPool<BlockType> BlockPool;

    static readonly int[] randomGroupPool = { 1, 2, 2, 3, 4 }; //use this in random type distribution

    public PooledPieceDefinition(ObjectPool<PieceType> piecePool, ObjectPool<BlockType> blockPool, PieceShape shape, Rect2 textureRegion, string name = null) : base(shape, textureRegion, name)
    {
        PiecePool = piecePool;
        BlockPool = blockPool;
    }

    public PooledPieceDefinition(ObjectPool<PieceType> piecePool, ObjectPool<BlockType> blockPool, PieceShape shape, string name = null) : base(shape, name)
    {
        PiecePool = piecePool;
        BlockPool = blockPool;
    }

    public PooledPieceDefinition(ObjectPool<BlockType> blockPool, PieceShape shape, string name = null) : base(shape, name)
    {
        PiecePool = BasicPool;
        BlockPool = blockPool;
    }

    //how does random type distribution work?
    //we take two random block types, then get a random number from 1 to 4 using the array above
    //this random number will be the type1Count variable
    //that many groups will be assigned to block type 1, randomly selecting any of the 4 groups
    //then, the remaining groups will be assigned to block type 2

    public override BagPiece GetPiece()
    {
        SetColor();

        BlockType[] blockTypes = DistributeBlockTypes();
        BagBlock[] blocks = CreateBlocks(blockTypes);

        return new BagPiece(PiecePool.GetRandom(), blocks, Shape.dimensions, TextureRegion, hsv.Item1, hsv.Item2, hsv.Item3, Name);

    }


    /// <summary>
    /// Distributes block types randomly based on their numerical group. Returns a 5-element array, one element for each of the 5 groups
    /// </summary>
    /// <returns></returns>
    public BlockType[] DistributeBlockTypes()
    {
        //Each block shape contains numbers 0 to 5, as distinct "Groups" of blocks
        //0 is an empty space
        //1-4 are normal blocks, which are filled with either StarterType or Remaindertype
        //5th is guaranteed to be distinct and uses ExtraType

        BlockType StarterType = BlockPool.GetRandom();      //We start with this one, filling in random groups within 1-4
        BlockType RemainderType = BlockPool.GetRandom();    //The remaining groups are set to this type
        BlockType ExtraType = BlockPool.GetRandom();         //The extra 5th group is filled with this always. 5th group is not guaranteed

        BlockType[] types = [StarterType, StarterType, StarterType, StarterType, ExtraType];
        //Contains the type for groups 1 to 5, populated entirely with the above types                                      
        //Starts at 0!! so group 1-5 are held in index 0-4!

        //First, we need to split groups 1-4 into two random sets.
        //1 to 4 groups will belong to StarterType.
        //The remaining groups will belong to RemainderType if any exist.
        //The split is fully random, so it is not 123 for StarterType. It can be 14, or 23, or any other combination


        if(StarterType == RemainderType) { return types; } //Abort early if our types are identical

        List<int> indexes = [0, 1, 2, 3];
        //indexes to access type array

        int SplitSize = randomGroupPool[GD.RandRange(0, 4)];
        //Size of split 1, pertaining to StarterType

        //Draw a random value from the list, use it as the index for types[], then discard the value until the list is empty
        while (indexes.Count > 0) 
        {
            int randomIndex = indexes[GD.RandRange(0, indexes.Count - 1)];

            indexes.Remove(randomIndex); //Remove our value so it cannot be drawn again
            
            if (SplitSize > 0) //If we are still in split 1: Use split 1 type StarterType
            {
                types[randomIndex] = StarterType;
                SplitSize--;
                continue;
            }

            //If split1 is fulfilled use split 2 type
            types[randomIndex] = RemainderType;

        }

        return types;
    }
}
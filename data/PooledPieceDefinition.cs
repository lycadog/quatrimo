using Godot;
using System.Collections.Generic;
using System.Xml.Linq;

public class PooledPieceDefinition : PieceDefinition 
{
    static ObjectPool<PieceType> BasicPool = new(PieceType.Basic);

    readonly ObjectPool<IHasShape> ShapePool;
    readonly ObjectPool<PieceType> PieceTypePool;
    readonly ObjectPool<BlockType> BlockPool;

    BlockType[] BlockTypes;

    static ObjectPool<Vector2I> randomTextures = new([new(0, 30), new(80, 30), new(0, 40)], [new (0, 50), new(40, 50), new(10, 30), new(90, 30), new(10, 40)], [new(20, 30), new(20, 40), new(10, 50), new(100, 30)], 10, 2, 1);

    bool UseRandomTexture;

    static readonly int[] randomGroupPool = { 1, 1, 1, 2, 2, 2, 2, 3, 3, 4 }; //use this in random type distribution

    public PooledPieceDefinition(ObjectPool<PieceType> piecePool, ObjectPool<BlockType> blockPool, ObjectPool<IHasShape> shapePool, Rect2 textureRegion) : base(textureRegion)
    {
        PieceTypePool = piecePool;
        BlockPool = blockPool;
        ShapePool = shapePool;
        TextureRegion = textureRegion;
    }

    public PooledPieceDefinition(ObjectPool<PieceType> piecePool, ObjectPool<BlockType> blockPool, ObjectPool<IHasShape> shapePool, bool useRandomTexture = true) : base()
    {
        PieceTypePool = piecePool;
        BlockPool = blockPool;
        ShapePool = shapePool;
        UseRandomTexture = useRandomTexture;
    }

    public PooledPieceDefinition(ObjectPool<BlockType> blockPool, ObjectPool<IHasShape> shapePool, bool useRandomTexture = true) : base()
    {
        PieceTypePool = BasicPool;
        BlockPool = blockPool;
        ShapePool = shapePool;
        UseRandomTexture = useRandomTexture;
    }

    public override BagPiece GetPiece()
    {
        SetColor();
        CurrentShape = ShapePool.GetRandom().GetShape();

        BlockTypes = DistributeBlockTypes();

        BagBlock[] blocks = CreateBlocks();

        Rect2 currentTexture = TextureRegion;
        if (UseRandomTexture)
        {
            currentTexture = new(randomTextures.GetRandom(), new(10, 10));
        }

        return new BagPiece(PieceTypePool.GetRandom(), blocks, CurrentShape.dimensions, CurrentShape.BoundingBox, currentTexture, hsv.Item1, hsv.Item2, hsv.Item3, CurrentShape.name);
    }

    public override BagBlock[] CreateBlocks()
    {
        BagBlock[] blocks = new BagBlock[CurrentShape.BlockCount];

        for (int i = 0; i < CurrentShape.BlockCount; i++)
        {
            blocks[i] = new(BlockTypes[GD.RandRange(0, BlockTypes.Length - 1)], CurrentShape[i]);
            //get fully random type - maybe change later?
        }
        return blocks;
    }

    public BlockType[] DistributeBlockTypes()
    {
        int typeCount = randomGroupPool[GD.RandRange(0, randomGroupPool.Length - 1)];
        BlockType[] types = new BlockType[typeCount];

        for(int i = 0; i < typeCount; i++)
        {
            types[i] = BlockPool.GetRandom();
        }

        return types;
    }

}
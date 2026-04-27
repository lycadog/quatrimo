using Godot;

public class SimplePieceDefinition : PieceDefinition
{
    PieceType pieceType;
    BlockType blockType;

    public SimplePieceDefinition(IHasShape shape, Rect2 textureRegion, BlockType blockType = BlockType.Basic, PieceType pieceType = PieceType.Basic) : base(shape, textureRegion)
    {
        this.pieceType = pieceType;
        this.blockType = blockType;
    }

    public SimplePieceDefinition(IHasShape shape, BlockType blockType = BlockType.Basic, PieceType pieceType = PieceType.Basic) : base(shape)
    {
        this.pieceType = pieceType;
        this.blockType = blockType;
    }

    public override BagPiece GetPiece()
    {
        SetColor();
        CurrentShape = Shape.GetShape();

        BagBlock[] blocks = CreateBlocks();

        return new BagPiece(pieceType, blocks, CurrentShape.dimensions, CurrentShape.BoundingBox, TextureRegion, hsv.Item1, hsv.Item2, hsv.Item3, CurrentShape.name);
    }

    public override BagBlock[] CreateBlocks()
    {
        BagBlock[] blocks = new BagBlock[CurrentShape.BlockCount];

        for(int i = 0; i < CurrentShape.BlockCount; i++)
        {
            blocks[i] = new(blockType, CurrentShape[i]);
        }

        return blocks;
    }
}
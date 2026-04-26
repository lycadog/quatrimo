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

        BagBlock[] blocks = CreateBlocks(blockType);

        return new BagPiece(pieceType, blocks, CurrentShape.dimensions, TextureRegion, hsv.Item1, hsv.Item2, hsv.Item3, CurrentShape.name);
    }
}
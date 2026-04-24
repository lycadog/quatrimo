using Godot;

public class SimplePieceDefinition : PieceDefinition
{
    PieceType pieceType;
    BlockType blockType;

    public SimplePieceDefinition(PieceShape shape, Rect2 textureRegion, BlockType blockType = BlockType.Basic, PieceType pieceType = PieceType.Basic) : base(shape, textureRegion)
    {
        this.pieceType = pieceType;
        this.blockType = blockType;
    }

    public SimplePieceDefinition(PieceShape shape, BlockType blockType = BlockType.Basic, PieceType pieceType = PieceType.Basic) : base(shape)
    {
        this.pieceType = pieceType;
        this.blockType = blockType;
    }

    public override BagPiece GetPiece()
    {
        SetColor();
        BagBlock[] blocks = CreateBlocks(blockType);

        return new BagPiece(pieceType, blocks, Shape.dimensions, TextureRegion, hsv.Item1, hsv.Item2, hsv.Item3, Shape.name);
    }

}
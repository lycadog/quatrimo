using Godot;

public class SimplePieceDefinition(PieceShape shape, PieceType pieceType, BlockType blockType, Rect2 textureRegion, string name = null) : PieceDefinition(shape, textureRegion, name)
{

    public override BagPiece GetPiece()
    {
        SetColor();
        BagBlock[] blocks = CreateBlocks(blockType);

        return new BagPiece(pieceType, blocks, Shape.dimensions, TextureRegion, hsv.Item1, hsv.Item2, hsv.Item3, Name);
    }

}
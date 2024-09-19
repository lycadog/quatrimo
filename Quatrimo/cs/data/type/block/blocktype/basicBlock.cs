
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class basicBlock : blockType
{
    public basicBlock(board board) : base(board)
    {
    }

    public override long scoreValue { get => 1; }
    public override long multiplier { get => 0; }

    public override bool collidesPlaced(block fallingBlock)
    {
        return true;
    }

    public override Color getColor(boardPiece piece)
    {
        return piece.color;
    }

    public override blockType getNewObject(board board)
    {
        return new basicBlock(board);
    }

    public override Texture2D getTex(boardPiece piece)
    {
        return piece.tex;
    }
}
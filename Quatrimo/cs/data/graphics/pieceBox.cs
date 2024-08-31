
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class pieceBox
{
    public Texture2D tex;
    public Vector2 pos;
    public Vector2 size;
    public bagPiece piece;
    
    public void updatePiece(bagPiece piece)
    {
        this.piece = piece;
    }
}
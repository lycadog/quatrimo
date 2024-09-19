
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class baseBlock<T> where T : block, new()
{
    
    public Vector2I localpos { get; set; }
    public Texture2D tex { get; set; }
    public Color color { get; set; }

    public T createBlock(board board, boardPiece piece)
    {
        return (T)Activator.CreateInstance(typeof(T), new object[] { board, piece, localpos });
    }

}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

public class boardPiece
{
    public board board;

    public boardPiece(board board, block[] blocks, pieceMod mod, Vector2I dimensions, Vector2I origin, string name, Texture2D tex, Color color)
    {
        this.board = board;
        this.blocks = blocks;
        this.mod = mod;
        this.dimensions = dimensions;
        this.origin = origin;
        this.name = name;
        this.tex = tex;
        this.color = color;
    }

    public block[] blocks { get; set; }
    public pieceMod mod {  get; set; }
    public Vector2I dimensions { get; set; }
    public Vector2I pos { get; set; }
    public Vector2I origin { get; set; }
    public int rotation { get; set; } = 0; //0 - 3
    public int dropOffset { get; set; }
    public string name { get; set; }
    public Texture2D tex { get; set; }
    public Color color { get; set; }

    public void play()
    {
        pos = new Vector2I(5, 9 - (dimensions.y / 2));
        foreach(block block in blocks)
        {
            mod.bPlay(block);
        }
        updatePos();
    }

    public void move(int xOffset, int yOffset)
    {
        pos  = new Vector2I(pos.x + xOffset, pos.y + yOffset); 
        updatePos();
    }

    /// <summary>
    /// Returns whether or not the piece will collide if it is moved in the specified direction
    /// </summary>
    /// <param name="xOffset"></param>
    /// <param name="yOffset"></param>
    /// <returns></returns>
    public bool collidesFalling(int xOffset, int yOffset)
    {
        return mod.pCollidesFalling(xOffset, yOffset);
    }

    /// <summary>
    /// Rotates the piece in the specified direction
    /// </summary>
    /// <param name="direction"></param>
    public void rotate(int direction)
    {
        rotation += direction;
        if (rotation > 3) //if rotation is out of the range 0 to 3, snap it back in range
        {
            rotation -= 4;
        }
        else if (rotation < 0)
        {
            rotation += 4;
        }
        foreach (block block in blocks)
        {
            block.rotate(direction);
        }
        updatePos();
    }

    /// <summary>
    /// Returns if the piece is able to rotate in the specified direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public bool canRotate(int direction)
    {
        foreach(block block in blocks)
        {
            Vector2I rotPos = new Vector2I(block.getRotatePos(direction).x + pos.x, block.getRotatePos(direction).y + pos.y);
            if(mod.bCollidesFalling(block,rotPos))
            {
                return false;
            }
        }
        return true;
    }

    public void place()
    {
        mod.pPlace();
    }

    public void removeFalling()
    {
        foreach(block block in blocks) { block.removeFalling(); }
    }

    public void updatePos()
    {
        foreach (block block in blocks)
        {
            block.updatePos();
        }
        updateDropPos();
        foreach (block block in blocks)
        {
            block.updateSpritePos();
        }
    }

    public void updateDropPos()
    {
        int y = 0;
        while (true)
        {
            if (collidesFalling(0, y))
            {
                dropOffset = y - 1; break;
            }
            else { y++; }
        }
    }
}
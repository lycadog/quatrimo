﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class boardPiece
{
    public board board;

    public boardPiece(board board, block[] blocks, Vector2I dimensions, Vector2I origin, string name, Texture2D tex, Color color)
    {
        this.board = board;
        this.blocks = blocks;
        this.dimensions = dimensions;
        this.origin = origin;
        this.name = name;
        this.tex = tex;
        this.color = color;
    }

    public block[] blocks { get; set; }
    public Vector2I dimensions { get; set; }
    public Vector2I pos { get; set; }
    public Vector2I origin { get; set; }
    public int rotation { get; set; } = 0; //0 - 3
    public int dropOffset { get; set; }
    public string name { get; set; }
    public Texture2D tex { get; set; }
    public Color color { get; set; }

    /// <summary>
    /// Creates the block and overridesnecessary delegates, MAKE SURE TO OVERRIDE for new piece types
    /// </summary>
    /// <param name="block"></param>
    public virtual void createBlock(block block)
    {
        
    }

    public virtual void play()
    {
        pos = new Vector2I(5, 9 - (dimensions.y / 2));
        foreach(block block in blocks)
        {
            block.play(block);
        }
        updatePos();
    }

    public virtual void move(int xOffset, int yOffset)
    {
        pos = new Vector2I(pos.x + xOffset, pos.y + yOffset);
        updatePos();
    }

    /// <summary>
    /// Returns whether or not the piece will collide if it is moved in the specified direction
    /// </summary>
    /// <param name="xOffset"></param>
    /// <param name="yOffset"></param>
    /// <returns></returns>
    public virtual bool collidesFalling(int xOffset, int yOffset)
    {
        Vector2I offset = new Vector2I(xOffset, yOffset);
        foreach (block block in blocks)
        {
            if (block.collidesFalling(offset, block)) { return true; }
        }
        return false;
    }

    /// <summary>
    /// Rotates the piece in the specified direction
    /// </summary>
    /// <param name="direction"></param>
    public virtual void rotate(int direction)
    {
        if(direction != 1 && direction != -1)
        {
            throw new ArgumentOutOfRangeException(nameof(direction), $"Rotation direction MUST be 1 or -1, received {direction} instead");
        }

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
            block.rotate(direction, block);
        }
        updatePos();
    }

    /// <summary>
    /// Returns if the piece is able to rotate in the specified direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public virtual bool canRotate(int direction)
    {
        foreach(block block in blocks)
        {
            Vector2I rotPos = new Vector2I(block.getRotatePos(direction, block).x + pos.x, block.getRotatePos(direction, block).y + pos.y);
            if (block.collidesFalling(rotPos, block))
            {
                return false;
            }
        }
        return true;
    }

    public virtual void place()
    {
        foreach(block block in blocks)
        {
            block.place(block);
        }
    }

    public virtual void removeFalling()
    {
        foreach(block block in blocks) { block.removeFalling(block); }
    }

    public virtual void updatePos()
    {
        foreach (block block in blocks)
        {
            block.updatePos(block);
        }
        updateDropPos();
        foreach (block block in blocks)
        {
            block.updateSpritePos(block);
        }
    }

    public virtual void updateDropPos()
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
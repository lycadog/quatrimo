using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PieceShape
{
    int[,] shape;

    public int this[int x, int y]
    {
        get => shape[x,y];
        set => shape[x, y] = value;
    }


    public Vector2I origin;
    public Vector2I dimensions;

    public string name;

    //Basic PieceType with no special types or anything
    //public SimplePieceType B;

    public PieceShape(int[,] shape, int originX, int originY, string name)
    {
        this.shape = shape;
        origin = new Vector2I(originX, originY);
        this.name = name;

        dimensions = new Vector2I(shape.GetLength(0), shape.GetLength(1));

        //B = new(this, 0, 0);
    }

}

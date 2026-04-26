using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PieceShape : IHasShape
{
    readonly int[,] shape;

    public int this[int x, int y]
    {
        get => shape[x,y];
        set => shape[x, y] = value;
    }

    public Vector2I origin;
    public Vector2I dimensions;

    public string name;

    /// <summary>
    /// Basic PieceType with no special types or anything
    /// </summary>
    public SimplePieceDefinition B;

    public PieceShape GetShape()
    {
        return this;
    }

    public PieceShape(int[,] shape, int originX, int originY, string name)
    {
        this.shape = shape;
        origin = new Vector2I(originX, originY);
        this.name = name;

        dimensions = new Vector2I(shape.GetLength(0), shape.GetLength(1));

        B = new(this);
    }

    /// <summary>
    /// Returns a pieceshape flipped across the X axis
    /// </summary>
    /// <param name="leftShape"></param>
    public PieceShape(PieceShape leftShape)
    {
        dimensions = leftShape.dimensions;


        int originX = dimensions.X - leftShape.origin.X - 1;

        origin = new(originX, leftShape.origin.Y);

        shape = new int[dimensions.X, dimensions.Y];

        for(int x = 0; x < dimensions.X; x++)
        {
            for(int y = 0; y < dimensions.Y; y++)
            {
                int flippedX = dimensions.X - x - 1;

                shape[flippedX, y] = leftShape[x, y];
            }
        }

        B = new(this);
    }
}

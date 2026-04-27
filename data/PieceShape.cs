using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PieceShape : IHasShape
{
    readonly Vector2I[] Shape;
    

    public Vector2I this[int i]
    {
        get => Shape[i];
        set => Shape[i] = value;
    }

    public int BlockCount
    {
        get => Shape.Length;
    }

    public BoundingBox BoundingBox;
    public Vector2I dimensions;

    public string name;

    /// <summary>
    /// Basic PieceType with no special types or anything
    /// </summary>
    public SimplePieceDefinition B;

    public static ObjectPool<IHasShape> AllShapes = new();

    public PieceShape GetShape()
    {
        return this;
    }

    public PieceShape((int, int)[] shape, string name, int weight = 6)
    {
        Shape = new Vector2I[shape.Length];
        for(int i = 0; i  < shape.Length; i++)
        {
            Shape[i] = new(shape[i].Item1, shape[i].Item2);
        }
        
        this.name = name;

        int minX = 100, minY = 100, maxX = -100, maxY = -100;
        foreach(var vector in Shape)
        {
            minX = Math.Min(minX, vector.X);
            minY = Math.Min(minY, vector.Y);

            maxX = Math.Max(maxX, vector.X);
            maxY = Math.Max(maxY, vector.Y);
        }

        BoundingBox = new(minX, maxX, minY, maxY);

        dimensions = new(maxX + Math.Abs(minX) + 1, maxY + Math.Abs(minY) + 1);
        //we add 1 because 0,0 is a valid spot, so -2 + 1 = 3, but actual dimensions is 4 (-2, -1, 0, 1)

        B = new(this);

        AllShapes.AddNewEntry(this, weight);
    }

    public PieceShape GetFlippedShape()
    {
        (int, int)[] flippedShape = new (int, int)[Shape.Length];

        for (int i = 0; i < flippedShape.Length; i++)
        {
            flippedShape[i] = (-Shape[i].X, Shape[i].Y);
        }

        return new(flippedShape, "Right " + name);
    }
}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PieceShape : IHasShape
{
    readonly Vector2[] Shape;

    public Vector2 this[int i]
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

    public PieceShape((float, float)[] shape, string name, int weight = 6, bool addEntry = true)
    {
        Shape = new Vector2[shape.Length];
        for(int i = 0; i  < shape.Length; i++)
        {
            Vector2 newVector = new(shape[i].Item1, shape[i].Item2);

            if (newVector != new Vector2() && Shape.Contains(newVector))
            {
                GD.PushError("Duplicate blocks in pieceshape " + name);
            }

            Shape[i] = newVector;
            
        }
        
        this.name = name;

        int minX = 100, minY = 100, maxX = -100, maxY = -100;
        foreach(var Vector2 in Shape)
        {
            Vector2I flooredVector = (Vector2I)Vector2.Floor();

            minX = Math.Min(minX, flooredVector.X);
            minY = Math.Min(minY, flooredVector.Y);

            maxX = Math.Max(maxX, flooredVector.X);
            maxY = Math.Max(maxY, flooredVector.Y);
        }

        BoundingBox = new(minX, maxX, minY, maxY);

        dimensions = new(maxX + Math.Abs(minX) + 1, maxY + Math.Abs(minY) + 1);
        //we add 1 because 0,0 is a valid spot, so -2 + 1 = 3, but actual dimensions is 4 (-2, -1, 0, 1)

        B = new(this);

        if (addEntry)
        {
            AllShapes.AddNewEntry(this, weight);
        }
    }

    public PieceShape GetFlippedShape()
    {
        (float, float)[] flippedShape = new (float, float)[Shape.Length];

        for (int i = 0; i < flippedShape.Length; i++)
        {
            flippedShape[i] = (-Shape[i].X, Shape[i].Y);
        }

        return new(flippedShape, "Right " + name, 0, false);
    }
}

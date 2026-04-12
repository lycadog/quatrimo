
using Godot;
using System.Collections.Generic;

public abstract class PieceDefinition
{
    public PieceShape Shape;

    public Rect2 TextureRegion;
    public string Name;

    protected (float, float, float) hsv; //Randomize the HSV every time we get a new piece

    public PieceDefinition(PieceShape shape, Rect2 textureRegion, string name = null)
    {
        Shape = shape;
        TextureRegion = textureRegion;
        Name = name;

        if(name == null) { Name = shape.name; }
    }

    public abstract BagPiece GetPiece();


    /// <summary>
    /// Create blocks with type array specifying the type for type values of 1 to 5
    /// </summary>
    /// <param name="blockTypes"></param>
    /// <returns></returns>
    public BagBlock[] CreateBlocks(BlockType[] blockTypes)
    {
        List<BagBlock> blocks = [];

        for (int x = 0; x < Shape.dimensions.X; x++)
        {
            for (int y = 0; y < Shape.dimensions.Y; y++)
            {
                //Create a new block for each non-empty space
                if (Shape[x, y] != 0)
                {

                    //blocks[index] = new BagBlock(blockTypes[shape.shape[x, y]], localX, localY, chosenColor);

                    BlockType type = blockTypes[Shape[x, y] - 1]; //subtracting 1 since we are starting at 1, not 0
                    Vector2I pos = new(x - Shape.origin.X, y - Shape.origin.Y);

                    BagBlock block = new(type, pos, TextureRegion, hsv.Item1, hsv.Item2, hsv.Item3);

                    blocks.Add(block);
                }

            }
        }

        return [.. blocks];
    }

    /// <summary>
    /// Create blocks of all one type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public BagBlock[] CreateBlocks(BlockType type)
    {
        return CreateBlocks([type, type, type, type, type]);
    }

    /// <summary>
    /// Set new random color
    /// </summary>
    public void SetColor()
    {
        hsv = Utils.GetRandomPieceHSV();
    }

}
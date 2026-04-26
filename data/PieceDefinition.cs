
using Godot;
using System.Collections.Generic;

public abstract class PieceDefinition
{
    public IHasShape Shape;
    protected PieceShape CurrentShape;

    public Rect2 TextureRegion;

    protected (float, float, float) hsv; //Randomize the HSV every time we get a new piece

    public abstract BagPiece GetPiece();
    public PieceDefinition(IHasShape shape, Rect2 textureRegion)
    {
        Shape = shape;
        TextureRegion = textureRegion;
    }

    public PieceDefinition(IHasShape shape)
    {
        Shape = shape;
        TextureRegion = new Rect2(0, 30, 10, 10);
    }

    public PieceDefinition(Rect2 textureRegion)
    {
        TextureRegion = textureRegion;
    }

    public PieceDefinition()
    {
        TextureRegion = new Rect2(0, 30, 10, 10);
    }

    /// <summary>
    /// Create blocks with type array specifying the type for type values of 1 to 5
    /// </summary>
    /// <param name="blockTypes"></param>
    /// <returns></returns>
    public BagBlock[] CreateBlocks(BlockType[] blockTypes)
    {
        List<BagBlock> blocks = [];

        for (int x = 0; x < CurrentShape.dimensions.X; x++)
        {
            for (int y = 0; y < CurrentShape.dimensions.Y; y++)
            {
                //Create a new block for each non-empty space
                if (CurrentShape[x, y] != 0)
                {

                    //blocks[index] = new BagBlock(blockTypes[shape.shape[x, y]], localX, localY, chosenColor);

                    BlockType type = blockTypes[CurrentShape[x, y] - 1]; //subtracting 1 since we are starting at 1, not 0
                    Vector2I pos = new(x - CurrentShape.origin.X, y - CurrentShape.origin.Y);

                    BagBlock block = new(type, pos, new(CurrentShape.origin.X, CurrentShape.origin.Y));

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
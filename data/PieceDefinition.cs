
using Godot;
using System.Collections.Generic;
using System.Linq;

public abstract class PieceDefinition
{
    public IHasShape Shape;
    protected PieceShape CurrentShape;

    public Rect2 TextureRegion;

    protected (float, float, float) hsv; //Randomize the HSV every time we get a new piece

    public abstract BagBlock[] CreateBlocks();

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
    /// Set new random color
    /// </summary>
    public void SetColor()
    {
        hsv = Utils.GetRandomPieceHSV();
    }

}
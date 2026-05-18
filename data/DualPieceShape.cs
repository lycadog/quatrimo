

using Godot;

public class DualPieceShape : IHasShape
{
    public readonly PieceShape L;
    public readonly PieceShape R;

    public SimplePieceDefinition LB
    {
        get => L.B;
    }

    public SimplePieceDefinition RB
    {
        get => R.B;
    }

    public DualPieceShape((float, float)[] shape, string name, int weight = 6)
    {
        L = new(shape, name, 0, false);
        R = L.GetFlippedShape();
        {
            R.name = "Right " + name;
        };

        L.name = "Left " + L.name;

        PieceShape.AllShapes.AddNewEntry(this, weight);
    }

    public PieceShape GetShape()
    {
        float rand = GD.Randf();

        if(rand > .5)
        {
            return L;
        }

        return R;
    }
}


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

    public DualPieceShape((int, int)[] shape, string name)
    {
        L = new(shape, name);
        R = L.GetFlippedShape();
        {
            R.name = "Right " + name;
        };

        L.name = "Left " + L.name; 
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
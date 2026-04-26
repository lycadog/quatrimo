

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

    public DualPieceShape(int[,] shape, int originX, int originY, string name)
    {
        L = new(shape, originX, originY, "Left " + name);
        R = new(L)
        {
            name = "Right " + name
        };
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
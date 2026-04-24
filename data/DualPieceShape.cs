

public class DualPieceShape
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

}
using Godot;

public class BoundingBox
{
    public int Left;
    public int Right;
    public int Top;
    public int Bottom;
    

    public BoundingBox(int left, int right, int top, int bottom)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }
}
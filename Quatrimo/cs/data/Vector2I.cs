
public struct Vector2I
{
    public Vector2I(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int x { get; set; }
    public int y { get; set; }
    
    public static Vector2I zero = new Vector2I(0, 0);

    public Vector2I add(Vector2I other)
    {
        return new Vector2I(x + other.x, y + other.y);
    }
}

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

    public static Vector2I operator+ (Vector2I v1, Vector2I v2)
    {
        return new Vector2I(v1.x + v2.x, v1.y + v2.y);
    }

    public static Vector2I operator- (Vector2I v1, Vector2I v2)
    {
        return new Vector2I(v1.x - v2.x, v1.y - v2.y);
    }

    public static Vector2I operator* (Vector2I v1, Vector2I v2)
    {
        return new Vector2I(v1.x * v2.x, v1.y * v2.y);
    }
}
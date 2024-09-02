
public class boardPiece
{
    public board board;
    public block[] blocks { get; set; }
    public Vector2I dimensions { get; set; }
    public Vector2I pos { get; set; }
    public Vector2I origin { get; set; }
    public int rotation { get; set; }
    public int dropOffset { get; set; }

}
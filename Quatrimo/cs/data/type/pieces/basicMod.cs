
public class basicMod : pieceMod
{
    public boardPiece modpiece;
    public override boardPiece piece { get => modpiece; set => modpiece = value; }
}

public class basicMod : pieceMod
{
    public boardPiece modpiece;
    public override boardPiece piece { get => modpiece; set => modpiece = value; }

    public override pieceMod getNew(boardPiece piece)
    {
        basicMod mod = new basicMod();
        mod.piece = piece;
        return mod;
    }
}
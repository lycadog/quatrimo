
using Microsoft.Xna.Framework;
using Quatrimo;
using static System.Reflection.Metadata.BlobBuilder;

public class spectralMod : pieceMod
{
    public boardPiece modpiece;
    public override boardPiece piece { get => modpiece; set => modpiece = value; }

    public override pieceMod getNew(boardPiece piece)
    {
        spectralMod mod = new spectralMod();
        mod.piece = piece;
        return mod;
    }

    public override void pUpdatePos()
    {
        piece.dropOffset = 0;
        foreach (block block in piece.blocks)
        {
            block.updatePos();
        }
        
        foreach (block block in piece.blocks)
        {
            block.updateSpritePos();
        }
    }

    public override void bGraphicsInit(block block)
    {
        Color color = block.type.getColor(piece);
        Vector3 v = color.ToVector3();
        color = new Color(new Vector4(v.X, v.Y, v.Z, .4f));
        block.element = new element(block.type.getTex(piece), color, new Vector2I(0, -5), 0.8f); //create new sprite element
        block.dropElement = new element(Game1.full25, new Color(.4f, .4f, .4f, .4f), new Vector2I(0, -10), 0.79f);
    }

    public override bool bCollidesFalling(block block, Vector2I checkPos)
    {
        if (checkPos.x < 0) { return true; }
        if (checkPos.x >= piece.board.dimensions.x) { return true; } //if the tile is outside the board dimensions return true (invalid move)
        if (checkPos.y < 0) { return true; }
        if (checkPos.y >= piece.board.dimensions.y) { return true; }

        return false;
    }

    public override void bPlace(block block)
    {
        block clippedblock = block.board.blocks[block.boardpos.x, block.boardpos.y];
        if (clippedblock != null)
        {
            bFallingBlockClipped(block, clippedblock);
        }
        else
        {
            block.place();
        }
    }

    public override void bFallingBlockClipped(block block, block placedBlock)
    {
        block.removeFalling();
    }

}
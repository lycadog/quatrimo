namespace Quatrimo
{
    public class singleModPiece : pieceType
    {
        int blockmod;

        public singleModPiece(pieceShape shape, int pieceMod, int blockmod) : base(shape, pieceMod)
        {
            this.blockmod = blockmod;
        }

        public override bagPiece getBagPiece()
        {
            throw new System.NotImplementedException();
        }
    }
}
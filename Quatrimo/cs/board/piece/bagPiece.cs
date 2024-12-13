namespace Quatrimo
{
    public class bagPiece
    {   //add draw weight later

        public bagBlock[] blocks;
        public int pieceMod;
        public Vector2I dimensions;
        public Vector2I origin;
        public string name;

        public bagPiece(bagBlock[] blocks, int pieceMod, Vector2I dimensions, Vector2I origin, string name)
        {
            this.blocks = blocks;
            this.pieceMod = pieceMod;
            this.dimensions = dimensions;
            this.origin = origin;
            this.name = name;
        }
    }
}
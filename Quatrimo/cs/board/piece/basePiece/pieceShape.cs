namespace Quatrimo
{
    public class pieceShape
    {
        public bool[,] shape;
        public Vector2I dimensions;
        public Vector2I origin;
        public short count;
        public string name;

        public pieceShape(bool[,] shape, Vector2I dimensions, Vector2I origin, short count, string name)
        {
            this.shape = shape;
            this.dimensions = dimensions;
            this.origin = origin;
            this.count = count;
            this.name = name;
        }
    }
}
namespace Quatrimo
{
    public class pieceShape
    {
        public bool[,] shape;
        public Vector2I dimensions;
        public Vector2I origin;
        public short blockCount;
        public string name;

        public pieceShape(bool[,] shape, int originX, int originY, short count, string name)
        {
            this.shape = shape;
            dimensions = new Vector2I(shape.GetLength(0), shape.GetLength(1));
            origin = new Vector2I(originX, originY);
            blockCount = count;
            this.name = name;
        }
    }
}
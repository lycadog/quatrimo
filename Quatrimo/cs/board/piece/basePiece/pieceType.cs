using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public abstract class pieceType
    {
        public pieceShape shape;
        public objPool<int> pieceMod;
        public short baseWeight;

        public string name;
        public Texture2DRegion tex;
        public Color[] color;

        public pieceType(pieceShape shape, objPool<int> pieceMod, short baseWeight)
        {
            this.shape = shape;
            this.pieceMod = pieceMod;
            this.baseWeight = baseWeight;
        }

        public abstract bagPiece getBagPiece(Color _color);
    }
}
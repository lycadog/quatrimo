using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public abstract class pieceType
    {   //need support for random color
        public pieceShape shape;
        public int pieceMod; //piece mod id
        public short baseWeight = 6;

        public string name;
        public Texture2DRegion tex;
        public Color color;

        public pieceType(pieceShape shape, int pieceMod)
        {
            this.shape = shape;
            this.pieceMod = pieceMod;
        }

        public abstract bagPiece getBagPiece();
    }
}
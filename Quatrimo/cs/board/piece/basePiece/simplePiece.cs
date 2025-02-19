using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class simplePiece : pieceType
    {
        int blockmod;

        public simplePiece(pieceShape shape, objPool<int> pieceMod, int blockmod, string name, Texture2DRegion tex, Color[] color, short baseWeight =  6) : base(shape, pieceMod, baseWeight)
        {
            this.blockmod = blockmod;
            this.name = name;
            this.tex = tex;
            this.color = color;
        }

        public simplePiece(pieceShape shape, objPool<int> pieceMod, int blockmod, Texture2DRegion tex, Color[] color, short baseWeight = 6) : base(shape, pieceMod, baseWeight)
        {
            this.blockmod = blockmod;
            name = shape.name;
            this.tex = tex;
            this.color = color;
        }

        public override bagPiece getBagPiece(Color _color)
        {
            bagBlock[] bagblocks = new bagBlock[shape.blockCount];
            int index = 0;
            for (int x = 0; x < shape.dimensions.x; x++)
            {
                for (int y = 0; y < shape.dimensions.y; y++)
                {
                    if (shape.shape[x, y]) //if this spot is marked as occupied, create a bagblock for it
                    {
                        Vector2I localpos = new Vector2I(x - shape.origin.x, y - shape.origin.y);

                        var block = new bagBlock(localpos, blockmod, _color, tex);

                        bagblocks[index] = block;
                        index++;
                    }
                }
            }

            return new bagPiece(bagblocks, pieceMod.getRandom(), shape.dimensions, shape.origin, baseWeight, _color, name);
        }
    }
}
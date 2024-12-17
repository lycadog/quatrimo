
using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class detailedPieceType : pieceType
    {
        //UP NEXT: integrate random pools for certain values, like block mod and piece color!
        //then, more kinds of pieceTypes

        baseBlockSpecification[] blockSpecs; //multiple blocks can use the same specification
        int[] blocksIndex; //blockspecification to use, grabbed from blocks array with the value as index
        public detailedPieceType(pieceShape shape, int pieceMod, baseBlockSpecification[] blockSpecs, int[] blocksIndex, string name, Texture2DRegion tex, Color[] color, short baseWeight = 6) : base(shape, pieceMod, baseWeight)
        {
            this.blockSpecs = blockSpecs;
            this.blocksIndex = blocksIndex;
            this.name = name;
            this.tex = tex;
            this.color = color;
        }

        public override bagPiece getBagPiece()
        {
            foreach(var blockspec in blockSpecs)
            {
                blockspec.onGetBagPiece();
            }

            bagBlock[] bagblocks = new bagBlock[shape.blockCount];
            int index = 0;
            Color _color = color[util.rand.Next(0, color.Length)];

            for(int x = 0; x < shape.dimensions.x; x++)
            {
                for(int y = 0; y < shape.dimensions.y; y++)
                {
                    if (shape.shape[x, y]) //if this spot is marked as occupied, create a bagblock for it
                    {
                        Vector2I localpos = new Vector2I(x - shape.origin.x, y - shape.origin.y);

                        var block = new bagBlock(localpos, _color, tex);
                        blockSpecs[blocksIndex[index]]._overwrite(block); //overwrite block's values with desired blockspecification

                        bagblocks[index] = block;
                        index++;
                    }
                }
            }

            return new bagPiece(bagblocks, pieceMod.getRandom(), shape.dimensions, shape.origin, name);
        }
    }
}
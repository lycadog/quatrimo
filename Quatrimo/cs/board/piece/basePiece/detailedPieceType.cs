
using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using System.Diagnostics;

namespace Quatrimo
{
    public class detailedPieceType : pieceType
    {

        baseBlockSpecification[] blockSpecs; //multiple blocks can use the same specification
        int[] blocksIndex; //blockspecification to use, grabbed from blocks array with the value as index
        public detailedPieceType(pieceShape shape, objPool<int> pieceMod, baseBlockSpecification[] blockSpecs, int[] blocksIndex, string name, Texture2DRegion tex, Color[] color, short baseWeight = 6) : base(shape, pieceMod, baseWeight)
        {
            this.blockSpecs = blockSpecs;
            this.blocksIndex = blocksIndex;
            this.name = name;
            this.tex = tex;
            this.color = color;
        }

        public detailedPieceType(pieceShape shape, objPool<int> pieceMod, baseBlockSpecification[] blockSpecs, int[] blocksIndex, Texture2DRegion tex, Color[] color, short baseWeight = 6) : base(shape, pieceMod, baseWeight)
        {
            this.blockSpecs = blockSpecs;
            this.blocksIndex = blocksIndex;
            name = shape.name;
            this.tex = tex;
            this.color = color;
        }

        public override bagPiece getBagPiece(Color _color)
        {
            foreach(var blockspec in blockSpecs)
            {
                blockspec.onGetBagPiece();
            }

            bagBlock[] bagblocks = new bagBlock[shape.blockCount];
            int index = 0;

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

            return new bagPiece(bagblocks, pieceMod.getRandom(), shape.dimensions, shape.origin, baseWeight, name);
        }
    }
}
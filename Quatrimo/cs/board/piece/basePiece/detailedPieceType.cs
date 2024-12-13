
namespace Quatrimo
{
    public class detailedPieceType : pieceType
    {
        //UP NEXT: integrate random pools for certain values, like block mod and piece color!
        //then, more kinds of pieceTypes

        blockSpecification[] blocks; //blockspecifications to be used, multiple blocks can use the same specification
        int[] blocksIndex; //blockspecification to use, grabbed from blocks with the value as index
        public detailedPieceType(pieceShape shape, int pieceMod, blockSpecification[] blocks, int[] blocksIndex) : base(shape, pieceMod)
        {
            this.blocks = blocks;
            this.blocksIndex = blocksIndex;
        }

        public override bagPiece getBagPiece()
        {
            bagBlock[] bagblocks = new bagBlock[shape.count];
            int index = 0;
            for(int x = 0; x < shape.dimensions.x; x++)
            {
                for(int y = 0; y < shape.dimensions.y; y++)
                {
                    if (shape.shape[x, y]) //if this spot is marked as occupied, create a bagblock for it
                    {
                        Vector2I localpos = new Vector2I(x - shape.origin.x, y - shape.origin.y);

                        var block = new bagBlock(localpos, color, tex);
                        blocks[blocksIndex[index]].overwrite(block); //overwrite block's values with desired blockspecification

                        bagblocks[index] = block;
                        index++;
                    }
                }
            }

            return new bagPiece(bagblocks, pieceMod, shape.dimensions, shape.origin, name);
        }
    }
}
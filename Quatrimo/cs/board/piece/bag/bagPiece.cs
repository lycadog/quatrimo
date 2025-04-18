using Microsoft.Xna.Framework;

namespace Quatrimo
{
    public class bagPiece
    {
        //TODO: add piecetags and duration mechanism
        public bagBlock[] blocks;
        public int pieceMod;
        public int baseWeight;
        public Vector2I dimensions;
        public Vector2I origin;
        public Color color;
        public string name;

        public objPool<bagPiece>.weightedEntry bagEntry;

        public bagPiece(bagBlock[] blocks, int pieceMod, Vector2I dimensions, Vector2I origin, int weight, Color color, string name)
        {
            this.blocks = blocks;
            this.pieceMod = pieceMod;
            this.dimensions = dimensions;
            this.origin = origin;
            baseWeight = weight;
            this.color = color;
            this.name = name;
        }

        public void reset()
        {
            bagEntry.setWeight(baseWeight);
        }

        /// <summary>
        /// Ran on all pieces at the start of a turn, ticks relevant piecetags and increases draw weight
        /// </summary>
        public void tick()
        {
            if(bagEntry.weight < baseWeight) { bagEntry.addWeight(1); }
            //tick pieceTags when we add em
        }

        //Tick relevant piece tags and lower weight
        public boardPiece drawPiece(encounter encounter)
        {
            //TODO: ADD pieceTag ticking
            boardPiece piece = getBoardpiece(encounter);
            bagEntry.subtractWeight(3);
            return piece;
        }

        /// <summary>
        /// Grab board piece from bagpiece, IF you want to draw the piece, run drawPiece instead
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public boardPiece getBoardpiece(encounter encounter)
        {
            boardPiece piece = data.pieces[pieceMod](encounter, dimensions, origin, color, name);
            block[] newBlocks = new block[blocks.Length];
            
            for(int i = 0; i <  blocks.Length; i++)
            {
                newBlocks[i] = data.blocks[blocks[i].mod].Invoke();
                newBlocks[i].createBlock(encounter, piece, blocks[i].localpos, blocks[i].tex, blocks[i].color);
                piece.initializeBlock(newBlocks[i]);
            }
            piece.blocks = newBlocks;

            return piece;
        }
    }
}
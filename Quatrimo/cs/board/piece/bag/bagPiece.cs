namespace Quatrimo
{
    public class bagPiece
    {
        public bagBlock[] blocks;
        public int pieceMod;
        public int baseWeight;
        public Vector2I dimensions;
        public Vector2I origin;
        public string name;

        public objPool<bagPiece>.weightedEntry bagEntry;

        public bagPiece(bagBlock[] blocks, int pieceMod, Vector2I dimensions, Vector2I origin, int weight, string name)
        {
            this.blocks = blocks;
            this.pieceMod = pieceMod;
            this.dimensions = dimensions;
            this.origin = origin;
            baseWeight = weight;
            this.name = name;
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
            boardPiece piece = new boardPiece(encounter, dimensions, origin, name);
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
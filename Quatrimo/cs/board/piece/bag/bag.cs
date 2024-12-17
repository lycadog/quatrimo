using MonoGame.Extended.Graphics;
using System.Collections.Generic;

namespace Quatrimo
{
    public class bag
    {
        public encounter encounter;

        List<bagPiece> pieces;
        objPool<bagPiece> piecePool;

        public Texture2DRegion tex;
        public string name;

        public bag(pieceType[] pieceTypes, string name)
        {
            this.name = name;

            foreach(var piece in pieceTypes)
            {
                bagPiece newPiece = piece.getBagPiece();
                pieces.Add(newPiece);
                newPiece.bagEntry = piecePool.addNewEntry(newPiece, newPiece.baseWeight);
            }
        }

        /// <summary>
        /// Ran at the start of the turn
        /// </summary>
        public void tickBag()
        {
            foreach(var piece in pieces)
            {
                piece.tick();
            }
        }

        public boardPiece drawRandomPiece()
        {
            return piecePool.getRandom().drawPiece(encounter);
        }
        
        public boardPiece getRandomPiece()
        {
            return piecePool.getRandom().getBoardpiece(encounter);
        }

        public boardPiece drawPiece(bagPiece piece)
        {
            return piece.drawPiece(encounter);
        }
    }
}
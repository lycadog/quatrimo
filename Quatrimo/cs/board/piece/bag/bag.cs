using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using System.Collections.Generic;

namespace Quatrimo
{
    public class bag
    {
        public encounter encounter;

        List<bagPiece> pieces = [];
        objPool<bagPiece> piecePool;

        List<pieceCard> hand = [];

        public Texture2DRegion tex;
        public string name;

        public bag(pieceType[] pieceTypes, string name, Color[] colors)
        {
            this.name = name;
            List<int> usedColors = [];
            piecePool = new objPool<bagPiece>();
            foreach(var piece in pieceTypes)
            {
                int index = util.rand.Next(0, colors.Length);
                short counter = 0;
                while (usedColors.Contains(index) && counter < 15)
                {
                    index = util.rand.Next(0, colors.Length);
                    counter++;
                }

                usedColors.Add(index);
                bagPiece newPiece = piece.getBagPiece(colors[index]);
                pieces.Add(newPiece);
                newPiece.bagEntry = piecePool.addNewEntry(newPiece, newPiece.baseWeight);
            }
        }

        public bag(pieceType[] pieceTypes, string name)
        {
            this.name = name;
            piecePool = new objPool<bagPiece>();

            foreach (var piece in pieceTypes)
            {
                bagPiece newPiece = piece.getBagPiece(piece.color[util.rand.Next(0, piece.color.Length)]);
                pieces.Add(newPiece);
                newPiece.bagEntry = piecePool.addNewEntry(newPiece, newPiece.baseWeight);
            }
        }

        /// <summary>
        /// Resets the weight of every piece back to its base weight
        /// </summary>
        public void reset()
        {
            foreach(var piece in pieces)
            {
                piece.reset();
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

        public void turnStartUpdate()
        {
            if(hand.Count < encounter.runData.handDrawMaximum)
            {
                drawHand();
                updateHand();
            }

        }

        public void addToHand(boardPiece piece)
        {
            hand.Add(new pieceCard(piece));
            updateHand();
        }

        void updateHand()
        {
            for (int i = 0; i < hand.Count; i++) //update position
            {
                hand[i].update(i, hand.Count);
            }
        }

        public void drawHand()
        {
            for(short i = 0; i < encounter.runData.handDrawSize; i++)
            {
                addToHand(drawRandomPiece());
            }
        }

        public boardPiece drawRandomPiece()
        {
            if(piecePool.pool.Count == 0)
            {//if no pieces have weight: force draw random piece
                return pieces[util.rand.Next(0, pieces.Count)].drawPiece(encounter);
            }
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

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quatrimo
{
    public class bag
    {
        public bag(starterBag bag)
        {
            pieces = new List<bagPiece>();


            foreach (pieceType piece in bag.pieces)
            {
                piece.addToBag(this);
            }


            name = bag.name;
            Debug.WriteLine($"Created bag {name}");
        }

        public List<bagPiece> pieces;
        public string name { get; set; }

        public boardPiece getPiece(encounter encounter)
        {
            foreach (bagPiece pieceW in pieces)
            {
                if (pieceW.weight != 1)
                {
                    pieceW.weight = Math.Min(pieceW.weight + .1f, 1);
                }
            }

            int index = 0;
            bagPiece piece;
            for (int i = 0; i < 20; i++)
            {
                float rand = util.rand.NextSingle();
                index = util.rand.Next(0, pieces.Count);

                piece = pieces[index];
                if (rand < piece.weight)
                {
                    piece.weight -= .5f;
                    break;
                }
            }

            return pieces[index].getBoardPiece(encounter);
        }

    }
}
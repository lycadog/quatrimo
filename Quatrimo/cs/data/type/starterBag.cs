using System;

namespace Quatrimo
{
    public class starterBag
    {
        public starterBag(pieceTypeOld[] pieces, string name)
        {
            this.pieces = pieces;
            this.name = name;
        }

        public pieceTypeOld[] pieces;
        public string name;


    }
}
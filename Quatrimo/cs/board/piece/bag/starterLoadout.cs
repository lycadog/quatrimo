
using Microsoft.Xna.Framework;

namespace Quatrimo
{
    public class starterLoadout
    {
        public pieceType[] starterBag;
        public Color[] colors;
        public string name;

        public starterLoadout(pieceType[] starterBag, Color[] colors, string name)
        {
            this.starterBag = starterBag;
            this.colors = colors;
            this.name = name;
        }

        public bag createBag()
        {
            return new bag(starterBag, name, colors);
        }
    }
}
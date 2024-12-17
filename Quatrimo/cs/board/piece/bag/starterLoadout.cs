
namespace Quatrimo
{
    public class starterLoadout
    {
        public pieceType[] starterBag;

        public string name;

        public starterLoadout(pieceType[] starterBag, string name)
        {
            this.starterBag = starterBag;
            this.name = name;
        }

        public bag createBag()
        {
            return new bag(starterBag, name);
        }
    }
}
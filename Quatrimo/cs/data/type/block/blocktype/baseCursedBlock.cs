using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class baseCursedBlock : baseblockType
    {
        public override block getNew()
        {
            return new cursedBlock();
        }
    }
}
using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using System;

namespace Quatrimo
{
    public abstract class baseBlockSpecification
    {
        public int mod = 0;
        public Color color;
        public Texture2DRegion tex;

        public Action<bagBlock> _overwrite;

        public abstract void overwrite(bagBlock block);
        public virtual void onGetBagPiece() { }
    }
}

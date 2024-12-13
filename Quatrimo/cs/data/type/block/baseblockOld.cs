
using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class baseblockOld
    {
        public baseblockOld(wSet<baseblockType> blocks, Texture2DRegion tex = null, Color color = new Color(), bool isColored = false)
        {
            this.blocks = blocks;
            this.tex = tex;
            this.color = color;
            this.isColored = isColored;
        }

        public block getBlock()
        {
            return chosenBlock.getNew();
        }

        public Vector2I localpos { get; set; }
        public Texture2DRegion tex { get; set; }
        public Color color { get; set; }
        public bool isColored { get; set; }


        public wSet<baseblockType> blocks { get; set; }
        public baseblockType chosenBlock { get; set; }
    }
}


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quatrimo
{
    public class baseblock
    {
        public baseblock(wSet<baseblockType> blocks, Texture2D tex = null, Color color = new Color(), bool isColored = false)
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
        public Texture2D tex { get; set; }
        public Color color { get; set; }
        public bool isColored { get; set; }


        public wSet<baseblockType> blocks { get; set; }
        public baseblockType chosenBlock { get; set; }
    }
}

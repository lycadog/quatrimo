using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class bagBlock
    {
        public Vector2I localpos;
        public int mod; //block mod id
        public Color color;
        public Texture2DRegion tex;

        public bagBlock(Vector2I localpos, Color color, Texture2DRegion tex)
        {
            this.localpos = localpos;
            this.color = color;
            this.tex = tex;
            mod = 0; //set to basic by default
        }

        public bagBlock(Vector2I localpos, int mod, Color color, Texture2DRegion tex)
        {
            this.localpos = localpos;
            this.color = color;
            this.tex = tex;
            this.mod = mod;
        }
    }
}
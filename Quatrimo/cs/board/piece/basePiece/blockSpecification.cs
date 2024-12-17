using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class blockSpecification : baseBlockSpecification
    {
        
        public override void overwrite(bagBlock block)
        {
            _overwrite(block);
        }

        public blockSpecification(int mod)
        {
            this.mod = mod;
            _overwrite += (bagBlock block) => block.mod = this.mod;
        }


        public blockSpecification(int mod, Texture2DRegion tex)
        {
            this.mod = mod;
            this.tex = tex;
            _overwrite += (bagBlock block) => block.mod = this.mod;
            _overwrite += (bagBlock block) => block.tex = this.tex;
        }
        public blockSpecification(Texture2DRegion tex)
        {
            this.tex = tex;
            _overwrite += (bagBlock block) => block.tex = this.tex;
        }

        
    }
}
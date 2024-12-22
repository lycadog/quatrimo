using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class randomBlockSpecif : baseBlockSpecification
    {
        objPool<int> modPool;
        bool rerollOnOverwrite;

        public override void overwrite(bagBlock block)
        {
            if (rerollOnOverwrite) { mod = modPool.getRandom(); }
            _overwrite(block);
        }

        public override void onGetBagPiece()
        {    
            mod = modPool.getRandom();
        }

        public randomBlockSpecif(objPool<int> modPool, bool rerollOnOverwrite = false)
        {
            this.modPool = modPool;

            _overwrite += (bagBlock block) => block.mod = mod;

            this.rerollOnOverwrite = rerollOnOverwrite;
        }

        public randomBlockSpecif(objPool<int> modPool, Texture2DRegion tex, bool rerollOnGetPiece = false)
        {
            this.modPool = modPool;
            mod = modPool.getRandom();

            this.tex = tex;

            _overwrite += (bagBlock block) => block.mod = mod;
            _overwrite += (bagBlock block) => block.tex = this.tex;

            rerollOnOverwrite = rerollOnGetPiece;
        }

    }
}
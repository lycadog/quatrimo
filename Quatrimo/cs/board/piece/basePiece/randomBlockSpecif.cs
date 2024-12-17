using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class randomBlockSpecif : baseBlockSpecification
    {
        objPool<int> modPool;
        bool shouldReroll;

        public override void overwrite(bagBlock block)
        {
            _overwrite(block);
        }

        public override void onGetBagPiece()
        {
            if (shouldReroll)
            {
                mod = modPool.getRandom();
            }
        }

        public randomBlockSpecif(objPool<int> modPool, bool rerollOnGetPiece = false)
        {
            this.modPool = modPool;
            mod = modPool.getRandom();

            _overwrite += (bagBlock block) => block.mod = mod;

            shouldReroll = rerollOnGetPiece;
        }

        public randomBlockSpecif(objPool<int> modPool, Texture2DRegion tex, bool rerollOnGetPiece = false)
        {
            this.modPool = modPool;
            mod = modPool.getRandom();

            this.tex = tex;

            _overwrite += (bagBlock block) => block.mod = mod;
            _overwrite += (bagBlock block) => block.tex = this.tex;

            shouldReroll = rerollOnGetPiece;
        }

    }
}
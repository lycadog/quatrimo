using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class animatedBlockSprite : blockSprite
    {
        public animatedBlockSprite(block block, Texture2DRegion tex, Color color, float depth = 0.8F, SpriteEffects effect = SpriteEffects.None) : base(block, tex, color, depth, effect)
        {
        }

        public animatedBlockSprite(block block, Vector2I offset, Texture2DRegion tex, Color color, float depth = 0.8F, SpriteEffects effect = SpriteEffects.None) : base(block, offset, tex, color, depth, effect)
        {
        }
    }
}
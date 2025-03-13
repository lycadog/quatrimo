using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class regSprite : sprite
    {
        public new Texture2DRegion tex;
        public Vector2 scale = new Vector2(2, 2);

        public regSprite(Texture2DRegion tex, Color color, float depth, SpriteEffects effects = SpriteEffects.None)
        {
            setParent(stateManager.baseParent);
            localPos = Vector2.Zero;
            this.tex = tex;
            this.color = color;
            this.depth = depth;
            this.effects = effects;
        }

        public regSprite(Vector2 localPos, Texture2DRegion tex, Color color, float depth, SpriteEffects effects = SpriteEffects.None)
        {
            setParent(stateManager.baseParent);
            this.localPos = localPos;
            this.tex = tex;
            this.color = color;
            this.depth = depth;
            this.effects = effects;
        }

        public regSprite(Vector2I elementPos, Texture2DRegion tex, Color color, float depth, SpriteEffects effects = SpriteEffects.None)
        {
            setParent(stateManager.baseParent);
            this.elementPos = elementPos;
            this.tex = tex;
            this.color = color;
            this.depth = depth;
            this.effects = effects;
        }

        public regSprite(drawObject parent, Texture2DRegion tex, Color color, float depth, SpriteEffects effects = SpriteEffects.None)
        {
            setParent(parent);
            this.tex = tex;
            this.color = color;
            this.depth = depth;
            this.effects = effects;
        }

        public regSprite(drawObject parent, Vector2 localPos, Texture2DRegion tex, Color color, float depth, SpriteEffects effects = SpriteEffects.None)
        {
            setParent(parent);
            this.tex = tex;
            this.color = color;
            this.depth = depth;
            this.effects = effects;
        }

        public regSprite(drawObject parent, Vector2I elementPos, Texture2DRegion tex, Color color, float depth, SpriteEffects effects = SpriteEffects.None)
        {
            setParent(parent);
            this.tex = tex;
            this.color = color;
            this.depth = depth;
            this.effects = effects;
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            SpriteBatchExtensions.Draw(spriteBatch, tex, new Vector2(globalPos.x, globalPos.y), color, globalRot, origin, scale, effects, depth);
            drawChildren(spriteBatch, gameTime);
        }
    }
}
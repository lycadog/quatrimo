using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Quatrimo
{
    public class sprite : drawObject
    {

        public float depth = 0;

        public Texture2D tex;
        public Color color;
        public SpriteEffects effects = SpriteEffects.None;

        public sprite() { }

        public sprite(Texture2D tex, Color color, float depth, SpriteEffects effects = SpriteEffects.None)
        {
            setParent(stateManager.baseParent);
            localPos = Vector2.Zero;
            this.tex = tex;
            this.color = color;
            this.depth = depth;
            this.effects = effects;
        }

        public sprite(Vector2 localPos, Texture2D tex, Color color, float depth, SpriteEffects effects = SpriteEffects.None)
        {
            setParent(stateManager.baseParent);
            this.localPos = localPos;
            this.tex = tex;
            this.color = color;
            this.depth = depth;
            this.effects = effects;
        }

        public sprite(Vector2I elementPos, Texture2D tex, Color color, float depth, SpriteEffects effects = SpriteEffects.None)
        {
            setParent(stateManager.baseParent);
            this.elementPos = elementPos;
            this.tex = tex;
            this.color = color;
            this.depth = depth;
            this.effects = effects;
        }

        public sprite(drawObject parent, Texture2D tex, Color color, float depth, SpriteEffects effects = SpriteEffects.None)
        {
            setParent(parent);
            this.tex = tex;
            this.color = color;
            this.depth = depth;
            this.effects = effects;
        }

        public sprite(drawObject parent, Vector2 localPos, Texture2D tex, Color color, float depth, SpriteEffects effects = SpriteEffects.None)
        {
            setParent(parent);
            this.localPos = localPos;
            this.tex = tex;
            this.color = color;
            this.depth = depth;
            this.effects = effects;
        }

        public sprite(drawObject parent, Vector2I elementPos, Texture2D tex, Color color, float depth, SpriteEffects effects = SpriteEffects.None)
        {
            setParent(parent);
            this.elementPos = elementPos;
            this.tex = tex;
            this.color = color;
            this.depth = depth;
            this.effects = effects;
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(tex, new Rectangle(globalPos.x, globalPos.y, size.x, size.y), null, color, globalRot, origin, effects, depth);
            drawChildren(spriteBatch, gameTime);
        }

    }
}
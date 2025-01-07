
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using System;
using System.Collections.Generic;

namespace Quatrimo
{
    public class regSprite : sprite
    {
        public new Texture2DRegion tex = texs.empty;
        public Vector2 scale = Vector2.One;
        new Vector2I size;

        public regSprite(Texture2DRegion tex, Color color, float depth = 0, SpriteEffects effect = SpriteEffects.None)
        {
            this.tex = tex;
            this.color = color;
            this.depth = depth;
            this.effect = effect;
        }

        public regSprite() { }

        public regSprite(Texture2DRegion tex, Color color, Vector2I boardPos, float depth = 0, SpriteEffects effect = SpriteEffects.None)
        {
            this.tex = tex;
            this.color = color;
            this.boardPos = boardPos;
            this.depth = depth;
            this.effect = effect;
        }

        public regSprite(Texture2DRegion tex, Vector2I elementPos, Color color, float depth = 0, SpriteEffects effect = SpriteEffects.None)
        {
            this.tex = tex;
            this.color = color;
            this.elementPos = elementPos;
            this.depth = depth;
            this.effect = effect;
        }

        /// <summary>
        /// Draws the sprite to the provided spritebatch
        /// </summary>
        /// <param name="spriteBatch"></param>
        protected override void drawState(SpriteBatch spriteBatch, GameTime gameTime, Action<List<drawable>> list)
        {
            SpriteBatchExtensions.Draw(spriteBatch, tex, new Vector2(worldPos.x, worldPos.y), color, rot, origin, scale, effect, depth);
        }

    }
}
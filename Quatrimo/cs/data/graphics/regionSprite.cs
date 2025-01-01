
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using System;
using System.Collections.Generic;

namespace Quatrimo
{
    public class regionSprite : sprite
    {
        public new Texture2DRegion tex = texs.empty;
        public Vector2 scale = Vector2.One;

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
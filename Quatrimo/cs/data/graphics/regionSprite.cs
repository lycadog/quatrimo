
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using System.Collections.Generic;

namespace Quatrimo
{
    public class regionSprite : spriteObject
    {
        public new Texture2DRegion tex = texs.empty;
        public Vector2 scale = Vector2.One;

        /// <summary>
        /// Draws the sprite to the provided spritebatch
        /// </summary>
        /// <param name="spriteBatch"></param>
        protected override void drawState(SpriteBatch spriteBatch, GameTime gameTime, List<drawable> list)
        {
            setState(0);
            SpriteBatchExtensions.Draw(spriteBatch, tex, new Vector2(pos.x, pos.y), color, rot, origin, scale, effect, depth);
        }

    }
}
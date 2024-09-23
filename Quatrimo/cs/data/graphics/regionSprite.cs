
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using Quatrimo;

namespace Quatrimo
{
    public class regionSprite : spriteObject
    {
        public Texture2DRegion tex = Game1.empty;
        public Vector2 scale = Vector2.One;

        /// <summary>
        /// Draws the sprite to the provided spritebatch
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void draw(SpriteBatch spriteBatch, GameTime gameTime, board board)
        {
            SpriteBatchExtensions.Draw(spriteBatch, tex, new Vector2(pos.x, pos.y), color, rot, origin, scale, effect, depth);
        }

    }
}
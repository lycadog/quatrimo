using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Quatrimo
{
    public abstract class drawable
    {
        /// <summary>
        /// Draws the sprite to the provided spritebatch
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to draw to</param>
        public abstract void draw(SpriteBatch spriteBatch, GameTime gameTime, board board);
    }
}

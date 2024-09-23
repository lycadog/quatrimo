using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quatrimo
{
    public class spriteObject
    {
        public Texture2D tex = Game1.none;
        public Color color = Color.White;
        public Vector2I pos = Vector2I.zero;
        public Vector2I size = Vector2I.zero;
        public float depth = 0;

        public float rot = 0;
        public Vector2 origin = Vector2.Zero;
        public SpriteEffects effect = SpriteEffects.None;

        /// <summary>
        /// Draws the sprite to the provided spritebatch
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to draw to</param>
        public virtual void draw(SpriteBatch spriteBatch, GameTime gameTime, board board)
        {
            spriteBatch.Draw(tex, new Rectangle(pos.x, pos.y, size.x, size.y), null, color, rot, origin, effect, depth);
        }

    }
}

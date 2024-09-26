
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Quatrimo
{
    /// <summary>
    /// for basic moving animated sprites
    /// </summary>
    public class movingSprite : dynamicAnim
    {
        Vector2 velocity = Vector2.Zero;
        Vector2 acceleration = Vector2.Zero; //applied to velocity over time

        public movingSprite(spriteObject sprite, Vector2 velocity, Vector2 acceleration) : base(sprite, null)
        { 
            this.velocity = velocity;
            this.acceleration = acceleration;
            drawD = update;
        }

        void update(spriteObject sprite, SpriteBatch spriteBatch, GameTime gameTime, board board)
        {
            double time = gameTime.ElapsedGameTime.TotalSeconds;
            velocity += new Vector2((float)(acceleration.X * time), (float)(acceleration.Y * time));

            floatpos += new Vector2((float)(velocity.X * time), (float)(velocity.Y * time));
        }
    }
}

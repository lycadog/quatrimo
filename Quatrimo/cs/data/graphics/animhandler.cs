
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Quatrimo
{
    public class animHandler : drawable
    {
        main main;
        public delegate void drawDelegate(SpriteBatch spriteBatch, GameTime gameTime, board board);

        public drawDelegate animState;
        public bool animationFinished = false;
        List<drawable> sprites = new List<drawable>();

        double timer = 0;
        short num = 0;
        byte counter1 = 0; byte counter2 = 0;
        Vector2I vector2 = Vector2I.zero;

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime, board board)
        {
            animState?.Invoke(spriteBatch, gameTime, board);
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// State before score animation processes to set up everything
        /// </summary>
        public void scoreAnimStart(SpriteBatch spriteBatch, GameTime gameTime, board board)
        {

        }

        public void score(SpriteBatch spriteBatch, GameTime gameTime, board board)
        {

        }
    }
}


using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Quatrimo
{
    public class dynamicAnim : drawable
    {
        protected Vector2 floatpos;
        protected spriteObject sprite;
        protected drawDelegate drawD;


        public delegate void drawDelegate(spriteObject sprite, SpriteBatch spriteBatch, GameTime gameTime, board board);


        public dynamicAnim(spriteObject sprite, drawDelegate drawMethod)
        {
            this.sprite = sprite;
            floatpos = new Vector2(sprite.pos.x, sprite.pos.y);
            drawD = drawMethod;
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime, board board)
        {
            drawD.Invoke(sprite, spriteBatch, gameTime, board);
            updatePos();
            sprite.draw(spriteBatch, gameTime, board);
        }
        
        protected void updatePos()
        {
            sprite.pos = new Vector2I((int)floatpos.X, (int)floatpos.Y);
        }
    }
}


using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Quatrimo
{
    public class dynamicAnim : drawable
    {
        protected Vector2 floatpos;
        protected spriteObject sprite;
        protected drawDelegate drawD;


        public delegate void drawDelegate(spriteObject sprite, SpriteBatch spriteBatch, GameTime gameTime, List<drawable> list);


        public dynamicAnim(spriteObject sprite, drawDelegate drawMethod)
        {
            this.sprite = sprite;
            floatpos = new Vector2(sprite.pos.x, sprite.pos.y);
            drawD = drawMethod;
        }

        protected override void drawState(SpriteBatch spriteBatch, GameTime gameTime, List<drawable> list)
        {
            drawD.Invoke(sprite, spriteBatch, gameTime, list);
            updatePos();
            sprite.draw(spriteBatch, gameTime, list);
        }
        
        protected void updatePos()
        {
            sprite.pos = new Vector2I((int)floatpos.X, (int)floatpos.Y);
        }
    }
}

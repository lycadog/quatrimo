using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using System;
using System.Collections.Generic;

namespace Quatrimo
{
    //rework to be more open
    public class blockSprite : drawable
    {
        public sprite sprite;
        public block block;
        
        public Vector2I offset = Vector2I.zero;

        public blockSprite(block block, sprite sprite)
        {
            this.block = block;
            this.sprite = sprite;
        }

        protected override void drawState(SpriteBatch spriteBatch, GameTime gameTime, ref Action<List<drawable>> listEditQueue)
        {
            sprite.draw(spriteBatch, gameTime, ref listEditQueue);
        }

        /// <summary>
        /// Set regSprite texture, crashes if sprite isn't a regSprite
        /// </summary>
        /// <param name="region"></param>
        public void setRegTexture(Texture2DRegion region)
        {
            ((regSprite)sprite).tex = region;
        }
        public void setTexture(Texture2D tex)
        {
            sprite.tex = tex;
        }

        public void setDepthOfAnimation(float depth)
        {
            foreach(var animFrame in ((animSprite)sprite).sequence)
            {
                animFrame.sprite.depth = depth;
            }
        }

        public void updatePos()
        {
            sprite.boardPos = block.boardpos + offset;
            checkOutOfBounds();
        }

        protected void checkOutOfBounds()
        {
            if (!isOutsideBoard()) //if block is inside of the board, set to draw
            {
                setState(0);
            }
            else { setState(1); } //if block is outside of the board, hide sprite
        }

        protected bool isOutsideBoard()
        {
            if (sprite.elementPos.x < board.offset.x + 1) { return true; }
            if (sprite.elementPos.x >= board.dimensions.x + board.offset.x + 1) { return true; }
            if (sprite.elementPos.y < 4) { return true; }
            if (sprite.elementPos.y >= board.dimensions.y + 2) { return true; }
            return false;
        }
    }
}
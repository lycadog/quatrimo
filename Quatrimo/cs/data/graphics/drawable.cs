using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Quatrimo
{
    public abstract class drawable
    {
        /// <summary>
        /// Run current draw method
        /// </summary>
        public Action<SpriteBatch, GameTime, List<drawable>> draw;
        public bool stale = false;

        public drawable()
        {
            draw = drawState;
        }
        public drawable(short state)
        {
            setState(state);
        }

        /// <summary>
        /// Set the drawable state ranging from 0 for draw, 1 for nodraw, 2 for stale
        /// </summary>
        /// <param name="state"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void setState(short state)
        {   //a bit silly maybe, but it's simple enough and protects the draw delegate
            switch (state)
            {
                case 0:
                    draw = drawState;
                    stale = false;
                    break;
                case 1:
                    draw = noDraw;
                    stale = false;
                    break;
                case 2:
                    draw = staleState;
                    stale = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Drawable state expects a number between 0 and 2. Number provided: " + state);
            }
        }

        protected abstract void drawState(SpriteBatch spriteBatch, GameTime gameTime, List<drawable> list);
        protected virtual void noDraw(SpriteBatch spriteBatch, GameTime gameTime, List<drawable> list) { }
        protected virtual void staleState(SpriteBatch spriteBatch, GameTime gameTime, List<drawable> list)
        {
            list.Remove(this);
        }
    }
}

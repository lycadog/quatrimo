﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quatrimo
{
    public abstract class drawableOld
    {
        public spriteManager manager;
        public delegate void drawDelegate(SpriteBatch spriteBatch, GameTime gameTime);

        public short state;
        public int renderLayer = 0;
        /// <summary>
        /// Run current draw method
        /// </summary>
        public drawDelegate draw;
        public bool stale = false;

        public drawableOld(short state = 0)
        {
            setState(state);
        }

        /// <summary>
        /// Set the drawable state ranging from 0 for draw, 1 for nodraw, 2 for stale
        /// </summary>
        /// <param name="state"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public virtual void setState(short state)
        {   //a bit silly maybe, but it's simple enough and protects the draw delegate
            this.state = state;
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

        protected abstract void drawState(SpriteBatch spriteBatch, GameTime gameTime);
        protected virtual void noDraw(SpriteBatch spriteBatch, GameTime gameTime) { }
        protected virtual void staleState(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //manager.remove(this, renderLayer);
        }
    }
}

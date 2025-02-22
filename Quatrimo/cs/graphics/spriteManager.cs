using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Quatrimo
{
    public class spriteManager
    {
        //multiple drawlayers might be unnecessary
        List<drawableAlsoOld> baseTargetSprites = [];
        List<drawableAlsoOld> rawDrawSprites = [];

        Action queuedOperations;

        public void drawBaseTarget(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach(var sprite in baseTargetSprites)
            {
                sprite.draw(spriteBatch, gameTime);
            }
        }

        public void drawRaw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var sprite in rawDrawSprites)
            {
                sprite.draw(spriteBatch, gameTime);
            }
        }

        public void add(drawableAlsoOld drawable, int renderLayer = 0)
        {
            //drawable.renderLayer = renderLayer;
            switch (renderLayer)
            {
                case 0:
                    baseTargetSprites.Add(drawable);
                    break;
                case 1:
                    rawDrawSprites.Add(drawable);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Drawable add list input is outside expected values of 0 to 1");
            }
        }

        public void remove(drawableAlsoOld drawable, int renderLayer = 0)
        {
            switch (renderLayer)
            {
                case 0:
                    baseTargetSprites.Remove(drawable);
                    break;
                case 1:
                    rawDrawSprites.Remove(drawable);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Drawable add list input is outside expected values of 0 to 1");
            }
        }

        public void queueAdd(drawableAlsoOld drawable, int renderLayer = 0)
        {
            queuedOperations += () => add(drawable, renderLayer);
        }

        public void queueRemove(drawableAlsoOld drawable, int renderLayer = 0)
        {
            queuedOperations += () => remove(drawable, renderLayer);
        }

        
    }
}
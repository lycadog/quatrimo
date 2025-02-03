using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Quatrimo
{
    public class spriteManager
    {
        List<drawable> baseResSprites = [];
        List<drawable> doubleResSprites = [];
        List<drawable> rawDrawSprites = [];

        Action queuedOperations;

        public void drawBaseRes(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach(var sprite in baseResSprites)
            {
                
            }
        }

        public void add(drawable drawable, int renderLayer)
        {
            drawable.renderLayer = renderLayer;
            switch (renderLayer)
            {
                case 0:
                    baseResSprites.Add(drawable);
                    break;
                case 1:
                    doubleResSprites.Add(drawable);
                    break;
                case 2:
                    rawDrawSprites.Add(drawable);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Drawable add list input is outside expected values of 0 to 2");
            }
        }

        public void remove(drawable drawable, int renderLayer)
        {
            switch (renderLayer)
            {
                case 0:
                    baseResSprites.Remove(drawable);
                    break;
                case 1:
                    doubleResSprites.Remove(drawable);
                    break;
                case 2:
                    rawDrawSprites.Remove(drawable);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Drawable add list input is outside expected values of 0 to 2");
            }
        }

        public void queueAdd(drawable drawable, int renderLayer)
        {
            queuedOperations += () => add(drawable, renderLayer);
        }

        public void queueRemove(drawable drawable, int renderLayer)
        {
            queuedOperations += () => remove(drawable, renderLayer);
        }

        
    }
}
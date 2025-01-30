
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quatrimo
{
    public class animSprite : sprite
    {
        public animFrame[] sequence;
        public int frame = 0;
        public double timer = 0;
        public bool loops;

        public animSprite(animFrame[] sequence, bool loops = false, int frame = 0) : base()
        {
            this.sequence = sequence;
            this.loops = loops;
            this.frame = frame;
        }

        protected override void drawState(SpriteBatch spriteBatch, GameTime gameTime, ref Action<List<drawable>> list)
        {
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > sequence[frame].hangTime)
            {
                timer = 0;
                if (frame < sequence.Length - 1) { frame += 1; }
                else
                {
                    if (loops)
                    { frame = 0; }

                    else { setState(2); return; }
                }
            }
            sequence[frame].sprite.worldPos += worldPos;
            sequence[frame].sprite.draw(spriteBatch, gameTime, ref list);
            sequence[frame].sprite.worldPos -= worldPos;
        }
        
    }
}

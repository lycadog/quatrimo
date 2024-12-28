
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Quatrimo
{
    public class animSprite : drawable
    {
        public animFrame[] sequence;
        public int frame = 0;
        public double timer = 0;
        public bool loops;

        public animSprite(animFrame[] sequence, bool loops = false, int frame = 0) : base(0)
        {
            this.sequence = sequence;
            this.loops = loops;
            this.frame = frame;
        }

        protected override void drawState(SpriteBatch spriteBatch, GameTime gameTime, Action<List<drawable>> list)
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
            sequence[frame].sprite.draw(spriteBatch, gameTime, list);
        }

        /// <summary>
        /// Sets the position of every frame
        /// </summary>
        /// <param name="pos"></param>
        public void setPosition(Vector2I pos)
        {
            foreach(var frame in sequence)
            {
                if(frame.sprite is element)
                {
                    element e = (element)frame.sprite;
                    e.updateWPos(pos);
                    break;
                }
                frame.sprite.pos = pos;
            }
        }

        
    }
}

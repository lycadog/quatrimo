
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Quatrimo
{
    public class animSprite : spriteObject
    {
        public animFrame[] sequence;
        public int frame = 0;
        public double counter = 0;
        public bool loops;

        public bool ended = false;

        public animSprite(animFrame[] sequence, bool loops = false, int frame = 0)
        {
            this.sequence = sequence;
            this.loops = loops;
            this.frame = frame;
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime, board board)
        {
            counter += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (counter > sequence[frame].hangTime)
            {
                counter = 0;
                if(frame < sequence.Length-1) { frame += 1; }
                else {
                    if (loops) 
                    { frame = 0; }
                  
                    else { board.staleSprites.Add(this); ended = true; return; } }
            }         
            sequence[frame].sprite.draw(spriteBatch, gameTime, board);
        }
    }
}

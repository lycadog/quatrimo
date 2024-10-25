
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quatrimo
{
    public class animSprite : spriteObject
    {
        public animFrame[] sequence;
        public int frame = 0;
        public double timer = 0;
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
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > sequence[frame].hangTime)
            {
                timer = 0;
                if(frame < sequence.Length-1) { frame += 1; }
                else {
                    if (loops) 
                    { frame = 0; }
                  
                    else { board.staleSprites.Add(this); ended = true; return; } }
            }         
            sequence[frame].sprite.draw(spriteBatch, gameTime, board);
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

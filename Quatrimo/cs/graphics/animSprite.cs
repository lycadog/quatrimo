
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quatrimo
{
    public class animSprite : drawObject
    {
        animFrame[] sequence;
        int frame = 0;
        double timer = 0;
        bool loops;

        public animSprite(animFrame[] sequence, Vector2I elementPos, bool loops = false, int frame = 0)
        {
            setParent(stateManager.baseParent);
            this.elementPos = elementPos;
            this.sequence = sequence;
            this.loops = loops;
            this.frame = frame;
            parentFrames();
        }

        public animSprite(animFrame[] sequence, Vector2 localPos, bool loops = false, int frame = 0)
        {
            setParent(stateManager.baseParent);
            this.localPos = localPos;
            this.sequence = sequence;
            this.loops = loops;
            this.frame = frame;
            parentFrames();
        }

        public animSprite(drawObject parent, animFrame[] sequence, Vector2I elementPos, bool loops = false, int frame = 0)
        {
            setParent(parent);
            this.elementPos = elementPos;
            this.sequence = sequence;
            this.loops = loops;
            this.frame = frame;
            parentFrames();
        }

        public animSprite(drawObject parent, animFrame[] sequence, Vector2 localPos, bool loops = false, int frame = 0)
        {
            setParent(parent);
            this.localPos = localPos;
            this.sequence = sequence;
            this.loops = loops;
            this.frame = frame;
            parentFrames();
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime)
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

                    else { dispose(); return; }
                }
            }
        }

        void parentFrames()
        {
            foreach(var frame in sequence)
            {
                frame.sprite.setParent(this);
            }
        }
        
    }
}

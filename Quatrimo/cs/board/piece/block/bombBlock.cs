using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quatrimo
{
    public class bombBlock : block
    {
        public short timer = 4;

        public override void animateScore(animation anim, bool forceAnim = false)
        {
            base.animateScore(anim, forceAnim);
            List<Vector2I> blocks = [
                boardpos.add(new Vector2I(1, 0)),
                boardpos.add(new Vector2I(-1, 0)),
                boardpos.add(new Vector2I(0, 1)),
                boardpos.add(new Vector2I(0, -1)),
                boardpos.add(new Vector2I(1, 1)),
                boardpos.add(new Vector2I(-1, 1)),
                boardpos.add(new Vector2I(1, -1)),
                boardpos.add(new Vector2I(-1, -1))
            ];
            new scoreBlocks(blocks, board).execute(encounter);
        }

        protected override void createGFXf(block block)
        {
            base.createGFXf(block);
            element.tex = Game1.bomb;
        }

        //bomb sprite looks weird rotated, so disable it
        protected override void rotateGFXf(int direction, block block)
        {
        }

        protected override void tickF(block block)
        {
            timer -= 1;
            Debug.WriteLine("BOMB TICKED timer: " + timer);

            if (timer > 0)
            {
                regionSprite sprite = new regionSprite();
                sprite.tex = Game1.nameQ;
                sprite.color = Color.Magenta;
                sprite.pos = new Vector2I(240, 240);
                sprite.depth = 1f;

                //board.sprites.Add(new movingSprite(sprite, new Vector2(0, -200f), new Vector2(0, 100f)));

                return;
            }
            
            List<Vector2I> blocks = [
                boardpos.add(new Vector2I(0, 0)),
                boardpos.add(new Vector2I(1, 0)),
                boardpos.add(new Vector2I(-1, 0)),
                boardpos.add(new Vector2I(0, 1)),
                boardpos.add(new Vector2I(0, -1)),
                boardpos.add(new Vector2I(1, 1)),
                boardpos.add(new Vector2I(-1, 1)),
                boardpos.add(new Vector2I(1, -1)),
                boardpos.add(new Vector2I(-1, -1))
            ];
            new scoreBlocks(blocks, board).execute(encounter);
            encounter.state.interrupted = true;
            score = base.scoreF;
        }
    }
}
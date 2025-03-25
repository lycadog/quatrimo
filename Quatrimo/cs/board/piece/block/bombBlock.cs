using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quatrimo
{
    public class bombBlock : block
    {
        //explosions are buggy and often end up on the wrong block
        //might be issue with boardpos not being up to date when the block explodes?
        public short timer = 4;

        protected override void animateScoreF(bool forceAnim = false)
        {
            base.animateScore(forceAnim);
            List<Vector2I> blocks = [
                boardpos + new Vector2I(1, 0),
                boardpos + new Vector2I(-1, 0),
                boardpos + new Vector2I(0, 1),
                boardpos + new Vector2I(0, -1),
                boardpos + new Vector2I(1, 1),
                boardpos + new Vector2I(-1, 1),
                boardpos + new Vector2I(1, -1),
                boardpos + new Vector2I(-1, -1)
            ];
            new scoreBlocks(blocks, board).execute(encounter);
        }

        protected override regSprite createGFXf(block block)
        {
            regSprite sprite = (regSprite)base.createGFXf(block);
            sprite.tex = content.bomb;
            return sprite;
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
                //add countdown timer particle later //**TODO
                /*regSprite sprite = new regSprite();
                sprite.tex = content.nameQ;
                sprite.color = Color.Magenta;
                sprite.worldPos = new Vector2I(240, 240);
                sprite.depth = 1f;

                //board.sprites.Add(new movingSprite(sprite, new Vector2(0, -200f), new Vector2(0, 100f)));

                return;*/
            }
            
            List<Vector2I> blocks = [
                boardpos + new Vector2I(0, 0),
                boardpos + new Vector2I(1, 0),
                boardpos + new Vector2I(-1, 0),
                boardpos + new Vector2I(0, 1),
                boardpos + new Vector2I(0, -1),
                boardpos + new Vector2I(1, 1),
                boardpos + new Vector2I(-1, 1),
                boardpos + new Vector2I(1, -1),
                boardpos + new Vector2I(-1, -1)
            ];
            new scoreBlocks(blocks, board).execute(encounter);
            encounter.state.interrupted = true;
            score = base.scoreF;
        }
    }
}
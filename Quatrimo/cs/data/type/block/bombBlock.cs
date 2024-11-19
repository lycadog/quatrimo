using System.Collections.Generic;
using System.Diagnostics;

namespace Quatrimo
{
    public class bombBlock : block
    {
        public short timer = 3;

        protected override void graphicsInit(block block)
        {
            base.graphicsInit(block);
            element.tex = Game1.bomb;
        }

        protected override void tickF(block block)
        {
            Debug.WriteLine("BOMB TICKED timer: " + timer);
            timer -= 1;

            if (timer > 0)
            {

                //regionSprite sprite = new regionSprite();
                //sprite.tex = nameQ;
                //sprite.color = Color.Magenta;
                //sprite.pos = new Vector2I(240, 240);
                //sprite.depth = 1f;

                //stateManager.encounter.board.sprites.Add(new movingSprite(sprite, new Vector2(0, -200f), new Vector2(0, 100f)));

                return;
            }

            List<block> blocks = [
                board.blocks[boardpos.x + 1, boardpos.y],
                board.blocks[boardpos.x - 1, boardpos.y],
                board.blocks[boardpos.x, boardpos.y + 1],
                board.blocks[boardpos.x, boardpos.y - 1]
            ];
            //tickOperation = new scoreBlocks(blocks);
        }
    }
}
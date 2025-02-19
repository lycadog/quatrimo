
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Quatrimo
{
    public class pieceBoxOld : spriteOld
    {
        public pieceBoxOld(Vector2I pos, Texture2D tex) : base()
        {
            this.worldPos = pos;
            this.tex = tex;
            this.size = new Vector2I(50, 50);
            depth = .925f;
            blocks = new spriteOld[0];
        }

        public spriteOld[] blocks;

        public void update(boardPiece piece)
        {
            blocks = new spriteOld[piece.blocks.Length];

            for (int i = 0; i < piece.blocks.Length; i++)
            {
                block block = piece.blocks[i];
                spriteOld sprite = block.createPreview(block);
                blocks[i] = sprite;

                Vector2I offset = new Vector2I((block.localpos.x * 5) + 25, (block.localpos.y * 5) + 25);

                sprite.worldPos = worldPos + offset;
            }
        }

        protected override void drawState(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.drawState(spriteBatch, gameTime);
            foreach (spriteOld sprite in blocks)
            {
                sprite.draw(spriteBatch, gameTime);
            }
        }
    }
}
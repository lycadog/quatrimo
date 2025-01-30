
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Quatrimo
{
    public class pieceBox : sprite
    {
        public pieceBox(Vector2I pos, Texture2D tex) : base()
        {
            this.worldPos = pos;
            this.tex = tex;
            this.size = new Vector2I(50, 50);
            depth = .925f;
            blocks = new sprite[0];
        }

        public sprite[] blocks;

        public void update(boardPiece piece)
        {
            blocks = new sprite[piece.blocks.Length];

            for (int i = 0; i < piece.blocks.Length; i++)
            {
                block block = piece.blocks[i];
                sprite sprite = block.createPreview(block);
                blocks[i] = sprite;

                Vector2I offset = new Vector2I((block.localpos.x * 5) + 25, (block.localpos.y * 5) + 25);

                sprite.worldPos = worldPos + offset;
            }
        }

        protected override void drawState(SpriteBatch spriteBatch, GameTime gameTime, ref Action<List<drawable>> list)
        {
            base.drawState(spriteBatch, gameTime, ref list);
            foreach (sprite sprite in blocks)
            {
                sprite.draw(spriteBatch, gameTime, ref list);
            }
        }
    }
}
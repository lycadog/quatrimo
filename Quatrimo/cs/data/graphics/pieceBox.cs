
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quatrimo
{
    public class pieceBox : spriteObject
    {
        public pieceBox(Vector2I pos, Texture2D tex) : base(new Vector2I(40, 40))
        {
            this.pos = pos;
            this.tex = tex;
            depth = .925f;
            blocks = new spriteObject[0];
        }

        public spriteObject[] blocks;

        public void update(boardPiece piece)
        {
            blocks = new spriteObject[piece.blocks.Length];

            for (int i = 0; i < piece.blocks.Length; i++)
            {
                block block = piece.blocks[i];
                spriteObject sprite = block.createPreview(block);
                blocks[i] = sprite;

                Vector2I offset = new Vector2I(20 + (block.localpos.x * 4), 16 + (block.localpos.y * 4));

                sprite.pos = pos.add(offset);
            }
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.draw(spriteBatch, gameTime);
            foreach (spriteObject sprite in blocks)
            {
                sprite.draw(spriteBatch, gameTime);
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quatrimo
{
    public class pieceBox : spriteObject
    {
        public pieceBox(Vector2I pos, Texture2D tex) : base()
        {
            this.pos = pos;
            this.tex = tex;
            this.size = new Vector2I(50, 50);
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

                Vector2I offset = new Vector2I((block.localpos.x * 5) + 25, (block.localpos.y * 5) + 25);

                sprite.pos = pos.add(offset);
            }
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime, board board)
        {
            base.draw(spriteBatch, gameTime, board);
            foreach (spriteObject sprite in blocks)
            {
                sprite.draw(spriteBatch, gameTime, board);
            }
        }
    }
}
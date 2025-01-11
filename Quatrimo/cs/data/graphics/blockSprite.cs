using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class blockSprite : regSprite
    {
        public block block;
        public Vector2I offset = Vector2I.zero;

        public blockSprite(block block, Texture2DRegion tex, Color color, float depth = 0.8f, SpriteEffects effect = SpriteEffects.None) : base(tex, color, new Vector2I(0, -5), depth, effect)
        {
            this.block = block;
        }

        public blockSprite(block block, Vector2I offset, Texture2DRegion tex, Color color, float depth = 0.8f, SpriteEffects effect = SpriteEffects.None) : base(tex, color, new Vector2I(0,-5), depth, effect)
        {
            this.block = block;
            this.offset = offset;
        }

        public void updatePos()
        {
            boardPos = block.boardpos + offset;
            checkOutOfBounds();
        }

        protected void checkOutOfBounds()
        {
            if (!isOutsideBoard()) //if block is inside of the board, set to draw
            {
                setState(0);
            }
            else { setState(1); } //if block is outside of the board, hide sprite
        }

        protected bool isOutsideBoard()
        {
            if (elementPos.x < board.offset.x + 1) { return true; }
            if (elementPos.x >= board.dimensions.x + board.offset.x + 1) { return true; }
            if (elementPos.y < 4) { return true; }
            if (elementPos.y >= board.dimensions.y + 2) { return true; }
            return false;
        }
    }
}
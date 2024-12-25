using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using System.Linq.Expressions;

namespace Quatrimo
{
    public class blockSprite : element
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

        public void play()
        {
            setEPosFromBoard(block.boardpos + offset);
        }

        public void move(int x, int y)
        {
            move(new Vector2I(x, y));
        }

        public void move(Vector2I offset)
        {
            //if beyond board boundary: hide block
            if (elementPos.x + offset.x > board.offset.x){

            }
            //offsetEPos(new Vector2I(x, y));
        }
    }
}
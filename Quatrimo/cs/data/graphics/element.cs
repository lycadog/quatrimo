﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class element : regionSprite
    {
        public Vector2I elementPos;
        public static readonly int eSize = 10;
        public element(Texture2DRegion tex, Color color, Vector2I epos, float depth = 0, SpriteEffects effect = SpriteEffects.None) : base()
        {
            this.tex = tex;
            this.color = color;
            elementPos = epos;
            this.depth = depth;
            this.effect = effect;
            origin = new Vector2(5, 5);
            pos = elementPos2WorldPos(epos);
            updatePos();
        }

        public void setEPos(Vector2I newPos)
        {
            elementPos = newPos;
            pos = elementPos2WorldPos(newPos);
            updatePos();
        }

        /// <summary>
        /// Updates position and element position using an offset
        /// </summary>
        /// <param name="offset"></param>
        public void offsetEPos(Vector2I offset)
        {
            elementPos = elementPos.add(offset);
            pos = elementPos2WorldPos(elementPos);
            updatePos();
        }

        protected void updatePos()
        {
            pos = pos.add(new Vector2I(5, 5));
        }


        /// <summary>
        /// Updates element position and world position using world position as input
        /// Recommended to use updateEPos instead
        /// </summary>
        /// <param name="newPos"></param>
        public void updateWPos(Vector2I newPos) //recommended to use updateEPos instead
        {
            pos = newPos;
            elementPos = worldPos2ElementPos(newPos);
        }

        /// <summary>
        /// Converts world position to element position
        /// </summary>
        /// <param name="wPos"></param>
        /// <returns></returns>
        public static Vector2I worldPos2ElementPos(Vector2I wPos)
        {
            return new Vector2I((int)(wPos.x / (float)eSize), (int)(wPos.y / (float)eSize));
        }
        /// <summary>
        /// Converts element position to world position
        /// </summary>
        /// <param name="epos"></param>
        /// <returns></returns>
        public static Vector2I elementPos2WorldPos(Vector2I epos)
        {
            return new Vector2I(epos.x * eSize, epos.y * eSize);
        }

        /// <summary>
        /// Converts board position to element position
        /// </summary>
        /// <param name="bpos"></param>
        /// <returns></returns>
        public static Vector2I boardPos2ElementPos(Vector2I bpos)
        {
            return new Vector2I(board.offset.x + bpos.x + 1, bpos.y - 4);
        }

        /// <summary>
        /// Converts board position to world position
        /// </summary>
        /// <param name="bpos"></param>
        /// <returns></returns>
        public static Vector2I boardPos2WorldPos(Vector2I bpos)
        {
            return new Vector2I((board.offset.x + bpos.x + 1) * eSize, (bpos.y - 4) * eSize);
        }
    }
}
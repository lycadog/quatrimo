﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class element : regSprite
    {
        public Vector2I elementPos;
        public static readonly int eSize = 10;
        public element(Texture2DRegion tex, Color color, Vector2I epos, float depth = 0, SpriteEffects effect = SpriteEffects.None)
        {
            this.tex = tex;
            this.color = color;
            elementPos = epos;
            this.depth = depth;
            this.effect = effect;
            origin = new Vector2(5, 5);
            worldPos = elementPos2WorldPos(epos);
        }

        public void updateEPos()
        {
            worldPos = elementPos2WorldPos(elementPos);
        }

        public void setEPosFromBoard(Vector2I boardpos)
        {
            setEPos(boardPos2ElementPos(boardpos));
        }

        public void setEPos(Vector2I newPos)
        {
            elementPos = newPos;
            worldPos = elementPos2WorldPos(newPos);
        }

        /// <summary>
        /// Updates position and element position using an offset
        /// </summary>
        /// <param name="offset"></param>
        public void offsetEPos(Vector2I offset)
        {
            elementPos += offset;
            worldPos = elementPos2WorldPos(elementPos);
        }

        /// <summary>
        /// Updates element position and world position using world position as input
        /// Recommended to use updateEPos instead
        /// </summary>
        /// <param name="newPos"></param>
        public void updateWPos(Vector2I newPos) //recommended to use updateEPos instead
        {
            worldPos = newPos;
            elementPos = worldPos2ElementPos(newPos);
        }

        /// <summary>
        /// Converts world position to element position
        /// </summary>
        /// <param name="wPos"></param>
        /// <returns></returns>
        public static Vector2I worldPos2ElementPos(Vector2I wPos)
        {
            return new Vector2I((int)(wPos.x / (float)eSize) - 5, (int)(wPos.y / (float)eSize) - 5);
        }
        /// <summary>
        /// Converts element position to world position
        /// </summary>
        /// <param name="epos"></param>
        /// <returns></returns>
        public static Vector2I elementPos2WorldPos(Vector2I epos)
        {
            return new Vector2I(epos.x * eSize + 5, epos.y * eSize + 5);
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
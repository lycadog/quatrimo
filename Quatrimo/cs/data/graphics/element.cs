
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class element : spriteObject
{
    public Vector2I elementPos;

    public element(Texture2D tex, Color color, Vector2I epos, float depth = 0, SpriteEffects effect = SpriteEffects.None) : base(new Vector2I(8,8))
    {
        this.tex = tex;
        this.color = color;
        elementPos = epos;
        this.depth = depth;
        this.effect = effect;
        this.origin = new Vector2(4, 4);
        pos = elementPos2WorldPos(epos);
    }

    public void updateEPos(Vector2I newPos)
    {
        elementPos = newPos;
        pos = elementPos2WorldPos(newPos);
    }

    /// <summary>
    /// Updates position and element position using an offset
    /// </summary>
    /// <param name="offset"></param>
    public void offsetEPos(Vector2I offset)
    {
        elementPos.add(offset);
        pos = elementPos2WorldPos(elementPos);
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
        return new Vector2I((int)(wPos.x / 8f), (int)(wPos.y / 8f));
    }
    /// <summary>
    /// Converts element position to world position
    /// </summary>
    /// <param name="epos"></param>
    /// <returns></returns>
    public static Vector2I elementPos2WorldPos(Vector2I epos)
    {
        return new Vector2I(epos.x * 8, epos.y * 8);
    }

    /// <summary>
    /// Converts board position to element position
    /// </summary>
    /// <param name="bpos"></param>
    /// <returns></returns>
    public static Vector2I boardPos2ElementPos(Vector2I bpos)
    {
        return new Vector2I(board.offset.x + bpos.x + 1, bpos.y - 5);
    }
}
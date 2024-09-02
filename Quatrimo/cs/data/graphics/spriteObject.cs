
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quatrimo;

public class spriteObject
{
    public Texture2D tex = Game1.empty;
    public Color color = Color.White;
    public Vector2I pos = Vector2I.zero;
    public Vector2I size = Vector2I.zero;
    public float depth = 0;

    public float rot = 0;
    public Vector2 origin = Vector2.Zero;
    public SpriteEffects effect = SpriteEffects.None;

    public spriteObject(Vector2I size)
    {
        this.size = size;
    }

    /// <summary>
    /// Draws the sprite to the provided spritebatch
    /// </summary>
    /// <param name="spriteBatch"></param>
    public virtual void draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(tex, new Rectangle(pos.x, pos.y, size.x, size.y), null, color, rot, origin, effect, depth);
    }

}
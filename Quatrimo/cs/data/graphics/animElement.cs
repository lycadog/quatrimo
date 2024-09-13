
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class animElement : element
{
    public animElement(Texture2D tex, Color color, Vector2I epos, float depth = 0, SpriteEffects effect = SpriteEffects.None) : base(tex, color, epos, depth, effect)
    {
    }


    public override void draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        base.draw(spriteBatch, gameTime);
    }
}
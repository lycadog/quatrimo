using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TileSprite : IRenderable
{
    public Texture2D tex { get; set; }
    public Color color { get; set; }
    public Vector2I pos { get; set; }
    public Vector2 origin;
    public float rot { get; set; }
    public float depth { get; set; }
    public SpriteEffects effect { get; set; }


    public TileSprite(Texture2D tex, Color color, Vector2I pos, float depth, Vector2 origin, float rot = 0, SpriteEffects effect = SpriteEffects.None)
    {
        this.tex = tex;
        this.color = color;
        this.pos = pos;
        this.origin = origin;
        this.rot = rot;
        this.depth = depth;
        this.effect = effect;
    }

    public void draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(tex, new Rectangle(pos.x * 16, pos.y * 16, 16, 16), null, color, rot, origin, effect, depth);
    }    
}


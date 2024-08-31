using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class BasicSprite : IRenderable
{
    public Texture2D tex { get; set; }
    public Color color { get; set; }
    public Vector2I pos { get; set; }
    public Vector2I size {  get; set; }
    public Vector2 origin;

    public BasicSprite(Texture2D tex, Color color, Vector2I pos, Vector2I size, float depth, Vector2 origin, float rot = 0, SpriteEffects effect = SpriteEffects.None)
    {
        this.tex = tex;
        this.color = color;
        this.pos = pos;
        this.size = size;
        this.origin = origin;
        this.rot = rot;
        this.depth = depth;
        this.effect = effect;
    }

    public float rot { get; set; }
    public float depth { get; set; }
    public SpriteEffects effect { get; set; }

    public void draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(tex, new Rectangle(pos.x, pos.y, size.x, size.y), null, color, rot, origin, effect, depth);
    }    
}


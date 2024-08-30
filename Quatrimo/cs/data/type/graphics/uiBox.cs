
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class uiBox
{
    public Texture2D tex;
    public Color color;
    public Vector2I pos;
    public Vector2I size;
    public uiText[] text;

    public uiBox(Texture2D tex, Color color, Vector2I pos, Vector2I size, uiText[] text)
    {
        this.tex = tex;
        this.color = color;
        this.pos = pos;
        this.size = size;
        this.text = text;
    }

    public void draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(tex, new Rectangle(pos.x, pos.y, size.x, size.y), color);

        foreach(uiText txt in text)
        {
            spriteBatch.DrawString(txt.font, txt.text, new Vector2(txt.pos.x + pos.x, txt.pos.y + pos.y), txt.color);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class uiText
{
    public Vector2I pos;
    public SpriteFont font;
    public string text;
    public Color color;

    public uiText(Vector2I pos, SpriteFont font, string text, Color color)
    {
        this.pos = pos;
        this.font = font;
        this.text = text;
        this.color = color;
    }
}
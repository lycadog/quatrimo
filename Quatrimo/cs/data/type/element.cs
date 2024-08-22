using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quatrimo;
using System.Diagnostics;

public class element
{ //ADD SUPPORT ofr animation
    public element(Vector2I pos, int z)
    {
        this.pos = pos;
        this.z = z;
        color = Color.White;
        tex = Game1.empty;
    }

    public animatable animatable { get; set; }
    public Vector2I pos {  get; set; }
    public int z { get; set; }
    public Texture2D tex { get; set; }
    public Color color { get; set; }

    public void draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        if (animatable != null) { animatable.update(this, gameTime); } // this may be bad!
        spriteBatch.Draw(tex, new Rectangle(Game1.res(pos.x*8) + Game1.xOffset, Game1.res(pos.y*8), Game1.res(8), Game1.res(8)), color);
    }
}

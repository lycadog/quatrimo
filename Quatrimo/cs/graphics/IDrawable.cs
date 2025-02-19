
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quatrimo
{
    public interface IDrawable
    {
        protected worldObject staleParent { get; set; }
        public bool stale { get; set; }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime);

        public void dispose();

    }
}

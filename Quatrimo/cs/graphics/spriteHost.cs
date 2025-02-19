using Microsoft.Xna.Framework.Graphics;

namespace Quatrimo
{
    public class spriteHost : worldObject, IDrawable
    {
        //COMBINE worldObject and iDrawable
        //FUCK i need parent-child system on base sprite Object
        bool _stale;
        public bool stale { get => stale; set => stale = value; }

        public void dispose()
        {
            throw new System.NotImplementedException();
        }

        public void draw(SpriteBatch spriteBatch, Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach(var child in children)
            {
                
            }
        }
    }
}
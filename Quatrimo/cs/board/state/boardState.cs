using Microsoft.Xna.Framework;

namespace Quatrimo
{
    public abstract class boardState
    {
        public encounter main;

        protected boardState(encounter main)
        {
            this.main = main;
        }

        public delegate void updateDelegate(GameTime gameTime);

        public updateDelegate update;


        public abstract void startState();


        
    }
}
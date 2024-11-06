using Microsoft.Xna.Framework;

namespace Quatrimo
{
    public abstract class boardState
    {
        public encounter encounter;

        protected boardState(encounter encounter)
        {
            this.encounter = encounter;
        }

        public delegate void updateDelegate(GameTime gameTime);

        public updateDelegate update;


        public abstract void startState();


        
    }
}
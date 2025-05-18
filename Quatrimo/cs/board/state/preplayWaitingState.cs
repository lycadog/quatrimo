using Microsoft.Xna.Framework;

namespace Quatrimo
{
    public class preplayWaitingState : boardState
    {

        public preplayWaitingState(encounter main) : base(main)
        {

        }

        public override void startState()
        {
            encounter.state = this;
            update = tick;
        }

        protected void tick(GameTime gameTime) //TODO: add code to select piece
        {


        }

    }
}
using Microsoft.Xna.Framework;

namespace Quatrimo
{
    public class animSuspendState : boardState
    {
        protected boardState suspendedState;
        protected animHandler.animDelegate suspendedAnimState;

        public animSuspendState(encounter main, boardState suspendedState, animHandler.animDelegate suspendedAnimState) : base(main)
        {
            this.suspendedState = suspendedState;
            this.suspendedAnimState = suspendedAnimState;
        }

        public override void startState()
        {
            encounter.state = this;
            update = tick;
        }

        protected void tick(GameTime gameTime)
        {
            if(encounter.animHandler.animState == encounter.animHandler.none)
            {
                encounter.state = suspendedState;
                encounter.animHandler.animState = suspendedAnimState;
                return;
            }
            encounter.animHandler.animState.Invoke(gameTime);
        }
    }
}
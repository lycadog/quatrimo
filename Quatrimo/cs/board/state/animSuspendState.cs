using Microsoft.Xna.Framework;

namespace Quatrimo
{
    public class animSuspendState : boardState
    {
        protected boardState suspendedState;
        protected animHandler.animDelegate suspendedAnimState;
        bool restartSuspendedState;

        public animSuspendState(encounter main, boardState suspendedState, bool restartSuspendedState = false) : base(main)
        {
            this.suspendedState = suspendedState;
            this.restartSuspendedState = restartSuspendedState;
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
                if (restartSuspendedState) { encounter.state.startState(); }
                return;
            }
            encounter.animHandler.animState.Invoke(gameTime);
        }
    }
}
using Microsoft.Xna.Framework;

namespace Quatrimo
{
    public class animSuspendState : boardState
    {
        protected boardState nextState;

        public animSuspendState(encounter main, boardState nextState) : base(main)
        {
            this.nextState = nextState;
        }

        public override void startState()
        {
            main.state = this;
            update = tick;
        }

        protected void tick(GameTime gameTime)
        {
            if(main.animHandler.animState == main.animHandler.none)
            {
                main.state = nextState;
                nextState.startState();
                return;
            }
            main.animHandler.animState.Invoke(gameTime);
        }
    }
}
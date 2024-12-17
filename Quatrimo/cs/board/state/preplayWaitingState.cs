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

        protected void tick(GameTime gameTime)
        {
            if (data.slamKey.keyDown)
            {
                //START piece fall
                encounter.currentPiece.play();

                midPiecefallState newState = new midPiecefallState(encounter);
                encounter.state = newState;
                newState.startState();
            }

        }

    }
}
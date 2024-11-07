using Microsoft.Xna.Framework;

namespace Quatrimo
{
    public class preplayWaitingState : boardState
    {
        public double pieceWaitTimer = 0;

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
            if (pieceWaitTimer >= 5000 || data.slamKey.keyDown)
            {
                //START piece fall
                encounter.currentPiece.play();

                midPiecefallState newState = new midPiecefallState(encounter);
                encounter.state = newState;
                newState.startState();
            }

            pieceWaitTimer += gameTime.ElapsedGameTime.Milliseconds;
        }

    }
}
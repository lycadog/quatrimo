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
            main.state = this;
            update = tick;
        }

        protected void tick(GameTime gameTime)
        {
            if (pieceWaitTimer >= 5000 || data.slamKey.keyDown)
            {
                //START piece fall
                main.currentPiece.play();
                main.canHold = true;

                midPiecefallState newState = new midPiecefallState(main);
                main.state = newState;
                newState.startState();
            }

            pieceWaitTimer += gameTime.ElapsedGameTime.Milliseconds;
        }

    }
}
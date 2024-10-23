using System.Diagnostics;

namespace Quatrimo
{
    public class turnStartState : boardState
    {
        public turnStartState(encounter main) : base(main)
        {
        }

        public override void startState()
        {
            main.state = this;
            main.turnRowsCleared = 0;

            main.currentPiece = main.nextPiece; //grab next piece
            main.nextPiece = main.bag.getPiece(main.board);
            main.board.nextbox.update(main.nextPiece);
            Debug.WriteLine($"[gamestate.turnStart] Now playing {main.currentPiece.name}");
            //update piece preview

            preplayWaitingState newState = new preplayWaitingState(main);
            newState.startState();
        }

    }
}
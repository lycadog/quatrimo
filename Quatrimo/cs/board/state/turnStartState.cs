﻿using System.Diagnostics;

namespace Quatrimo
{
    public class turnStartState : boardState
    {
        public turnStartState(encounter main) : base(main)
        {
        }

        public override void startState()
        {
            encounter.state = this;
            encounter.turnRowsCleared = 0;
            encounter.updatedRows = new bool[board.dimensions.y];

            encounter.bag.tickBag();

            encounter.currentPiece = encounter.nextPiece; //grab next piece
            encounter.nextPiece = encounter.bag.drawRandomPiece();
            encounter.board.nextbox.update(encounter.nextPiece);
            Debug.WriteLine($"[gamestate.turnStart] Now playing {encounter.currentPiece.name}");
            //update piece preview

            preplayWaitingState newState = new preplayWaitingState(encounter);
            newState.startState();
        }

    }
}
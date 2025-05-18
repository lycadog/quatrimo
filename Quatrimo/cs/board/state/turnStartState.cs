using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Runtime.InteropServices;

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
            update = tick;
            encounter.turnRowsCleared = 0;
            encounter.updatedRows = new bool[board.dimensions.y];

            encounter.bag.tickBag();
            encounter.bag.turnStartUpdate();
        }

        protected void tick(GameTime gameTime) //TODO: add code to select piece
        {   
            preplayWaitingState newState = new preplayWaitingState(encounter);
            encounter.state = newState;
            newState.startState();

        }

    }
}
using System.Diagnostics;
using System.Reflection.Emit;

namespace Quatrimo
{
    public class endTurnState : boardState
    {
        public endTurnState(encounter main) : base(main)
        {
        }

        public override void startState()
        {
            encounter.state = this;

            recalculateLevel(encounter.turnRowsCleared);



            turnStartState newState = new turnStartState(encounter);
            newState.startState();
        }


        public void recalculateLevel(short rows)
        {
            encounter.rowsCleared += rows;
            while (encounter.rowsCleared >= encounter.rowsRequired) //loop the level check incase the player levels up multiple times at once
            {
                encounter.level += 1;
                encounter.rowsCleared -= encounter.rowsRequired;
                encounter.rowsRequired += 2;
            }
            Debug.WriteLine("level: " + encounter.level);
            encounter.levelTimes = encounter.level / 2d + 1d;
        }
    }
}
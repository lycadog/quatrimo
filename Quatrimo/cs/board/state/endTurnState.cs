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
            main.state = this;
            turnStartState newState = new turnStartState(main);
            newState.startState();
        }


        public void recalculateLevel(short rows)
        {
            main.rowsCleared += rows;
            while (main.rowsCleared >= main.rowsRequired) //loop the level check incase the player levels up multiple times at once
            {
                main.level += 1;
                main.rowsCleared -= main.rowsRequired;
                main.rowsRequired += 2;
            }
            Debug.WriteLine("level: " + main.level);
            main.levelTimes = main.level / 2d + 1d;
        }
    }
}
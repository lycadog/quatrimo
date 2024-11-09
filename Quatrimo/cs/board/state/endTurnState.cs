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

            double levelTimes = 1;
            if(encounter.turnRowsCleared > 0)
            {
                encounter.turnScore += 10 * (encounter.turnRowsCleared - 1);

                if(encounter.turnRowsCleared == 3)
                {
                    levelTimes = (encounter.levelTimes / 2);
                } else if (encounter.turnRowsCleared >= 4)
                {
                    levelTimes = encounter.levelTimes;
                }
            }

            double score = encounter.turnScore * encounter.turnMultiplier * levelTimes;


            encounter.totalScore += (long)score; //save the total score

            encounter.turnScore = 0; //reset turn variables
            encounter.turnMultiplier = 1;
            encounter.turnRowsCleared = 0;

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
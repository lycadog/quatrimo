using System.Collections.Generic;
using System.Linq;

namespace Quatrimo
{
    public class pieceScoreState : boardState
    {
        public List<int> updatedRows = new List<int>();
        public pieceScoreState(encounter main) : base(main)
        {
        }

        public override void startState()
        {
            main.state = this;
            //RECORD updated rows
            //check updated rows for scorability
            //save scorable rows
            //score every block of those rows, adding up their score
            //set up the animationHandler to animate row decay
            //skip to animSuspendState with nextState as blockTickScoreState


            // ====== PLACED PIECE SCORE STEP ======
            foreach (block block in main.currentPiece.blocks)
            {
                updatedRows.Add( block.boardpos.y ); //record the height of every block of the recently placed piece
            }

            updatedRows = updatedRows.Distinct().ToList(); //remove duplicate entries

            foreach (int i in updatedRows)
            {
                if (isRowScoreable(i))
                { //check rows, add rows that are scored to the list
                    main.scorableRows.Add((short)i);
                    main.turnRowsCleared += 1;
                }
            }


            
            for (int x = 0; x < main.board.dimensions.x; x++) //process score of every tile in every scored row
            {
                foreach (int y in main.scorableRows) //process through rows
                {
                    block block = main.board.blocks[x, y];
                    if (block != null)
                    {
                        main.turnScore += block.getScore(block);
                        main.turnMultiplier += block.getTimes(block);

                        block.score(block);
                        main.scoredBlocks.Add(block);
                    }
                }
            }

            //SETUP ANIMATION STUFF HERE LATER

            blockTickScoreState stateAfterNext = new blockTickScoreState(main);
            animSuspendState newState = new animSuspendState(main, stateAfterNext);
            newState.startState();

        }

        public bool isRowScoreable(int y)
        {
            for (int x = 0; x < main.board.dimensions.x; x++)
            {
                if (main.board.blocks[x, y] == null) return false; //if any tile is empty, return false
                else { continue; } //if the tile isn't empty, keep looping
            }
            return true; //if no tiles return empty, this will run and return true
        }


    }
}
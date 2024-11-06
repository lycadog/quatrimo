using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

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
            encounter.state = this;
            //RECORD updated rows
            //check updated rows for scorability
            //save scorable rows
            //score every block of those rows, adding up their score
            //set up the animationHandler to animate row decay
            //skip to animSuspendState with nextState as blockTickScoreState
            foreach (block block in encounter.currentPiece.blocks)
            {
                encounter.board.updatedRows.Add(block.boardpos.y); //record the height of every block of the recently placed piece
            }

            updatedRows = updatedRows.Distinct().ToList(); //remove duplicate entries

            foreach (int i in updatedRows)
            {
                if (isRowScoreable(i))
                { //check rows, add rows that are scored to the queue
                    encounter.scoreQueue.Add(scoreRow.queueRowFromPiece(i, encounter.currentPiece));
                }
            }

            //PROCESS SCORE QUEUE AFTER HERE BLABLA





            // ====== PLACED PIECE SCORE STEP ======
            foreach (block block in encounter.currentPiece.blocks)
            {
                updatedRows.Add( block.boardpos.y ); //record the height of every block of the recently placed piece
            }

            updatedRows = updatedRows.Distinct().ToList(); //remove duplicate entries

            foreach (int i in updatedRows)
            {
                if (isRowScoreable(i))
                { //check rows, add rows that are scored to the queue
                    encounter.scoreQueue.Add(scoreRow.queueRowFromPiece(i, encounter.currentPiece));
                    encounter.turnRowsCleared += 1;
                }
            }


            
            for (int x = 0; x < encounter.board.dimensions.x; x++) //process score of every tile in every scored row
            {
                foreach (int y in encounter.scorableRows) //process through rows
                {
                    block block = encounter.board.blocks[x, y];
                    if (block != null)
                    {
                        encounter.turnScore += block.getScore(block);
                        encounter.turnMultiplier += block.getTimes(block);

                        block.score(block);
                        encounter.scoredBlocks.Add(block);
                    }
                }
            }

            //SETUP ANIMATION STUFF HERE LATER

            blockTickScoreState stateAfterNext = new blockTickScoreState(encounter);
            animSuspendState newState = new animSuspendState(encounter, stateAfterNext);
            newState.startState();

        }

        /// <summary>
        /// Processes updated rows into the scorequeue, piece param is optional
        /// </summary>
        /// <param name="piece"></param>
        protected void checkUpdatedRows(boardPiece piece = null)
        {
            foreach (int i in updatedRows)
            {
                if (isRowScoreable(i))
                {
                    if(piece == null) //if the row is not scored by a piece, start the score animation from the edges
                    {
                        //start score animation from the edges, as no piece is specified
                        break;
                    }
                    encounter.scoreQueue.Add(scoreRow.queueRowFromPiece(i, encounter.currentPiece));
                }
            }
        }

        protected bool isRowScoreable(int y)
        {
            for (int x = 0; x < encounter.board.dimensions.x; x++)
            {
                if (encounter.board.blocks[x, y] == null) return false; //if any tile is empty, return false
                else { continue; } //if the tile isn't empty, keep looping
            }
            return true; //if no tiles return empty, this will run and return true
        }

    }
}
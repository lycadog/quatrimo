using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;

namespace Quatrimo
{
    public class pieceScoreState : boardState
    {
        public List<int> updatedRows = [];
        public List<int> scoredRows = [];
        public pieceScoreState(encounter main) : base(main)
        {
        }

        public override void startState()
        {
            encounter.state = this;

            //RECORD updated rows
            //check updated rows for scorability
            //save scorable rows
            //set up the animationHandler to animate row decay
            //skip to animSuspendState with nextState as blockTickScoreState

            foreach (block block in encounter.currentPiece.blocks)
            {
                updatedRows.Add(block.boardpos.y); //record the height of every block of the recently placed piece
            }

            updatedRows = updatedRows.Distinct().ToList(); //remove duplicate entries

            foreach (int i in updatedRows)
            {
                if (isRowScoreable(i))
                { //check rows, add rows that are scored to the queue
                    encounter.scoreQueue.Add(scoreRow.queueRowFromPiece(i, encounter.currentPiece));
                    scoredRows.Add(i);
                }
            }

            //very messy for loops in here
            foreach (block block in encounter.currentPiece.blocks)
            {
                if (scoredRows.Contains(block.boardpos.y))
                {
                    animSprite sprite = animHandler.getDecayingAnim(block.boardpos);
                    encounter.board.sprites.Add(sprite);

                    block.hideGFX(block);
                    block.scoredAnim = true;
                    encounter.scoredBlocks.Add(block);
                }
                else
                {
                    element e = new element(Game1.boxsolid, Color.White, element.boardPos2ElementPos(block.boardpos), 0.85f);
                    animSprite anim = new animSprite([new animFrame(e, 200)]);
                    encounter.board.sprites.Add(anim);
                }
            }

            scoredRows.Clear();
            processQueue();

            //score queue should start the animations for every row, then we wait for the animation to finish
            //the animation stores each block along the animation in the scoredBlocks list
            //after finishing animation, process through scoredBlocks and actually score everything, then lower accordingly

            encounter.animHandler.animState = encounter.animHandler.waitForAnimations;

            //end the current state to wait for animations to play
            //skip to blockTickScoreState after, which finishes up the anim
            blockTickScoreState stateAfterNext = new blockTickScoreState(encounter);
            animSuspendState newState = new animSuspendState(encounter, stateAfterNext, true);
            newState.startState();

        }

        protected void processQueue()
        {
            foreach(var scoreOp in encounter.scoreQueue)
            {
                scoreOp.execute(encounter);
            }
            encounter.scoreQueue.Clear();
        }

        /// <summary>
        /// Processes updated rows into the scorequeue, piece param is optional
        /// </summary>
        /// <param name="piece"></param>
        protected void checkUpdatedRows(boardPiece piece = null)
        {
            foreach (int y in updatedRows)
            {
                if (isRowScoreable(y))
                {
                    if(piece == null) //if the row is not scored by a piece, start the score animation from the edges
                    {
                        encounter.scoreQueue.Add(scoreRow.queueNonpieceRow(y, encounter.board));
                        //start score animation from the edges, as no piece is specified
                        break;
                    }
                    encounter.scoreQueue.Add(scoreRow.queueRowFromPiece(y, encounter.currentPiece));
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
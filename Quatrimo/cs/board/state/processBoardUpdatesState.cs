using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quatrimo
{
    public class processBoardUpdatesState : boardState
    {
        readonly boardPiece scoredPiece;
        readonly boardState stateAfterAnim;
        List<int> scoredRows = [];

        //override stateAfterAnim when the blocktick is suspended, to maintain the same state
        public processBoardUpdatesState(encounter encounter, boardPiece scoredPiece = null, boardState stateAfterAnim = null) : base(encounter)
        {
            this.scoredPiece = scoredPiece;
            if(stateAfterAnim == null) { stateAfterAnim = new blockTickScoreState(encounter); }
            this.stateAfterAnim = stateAfterAnim;
        }

        public override void startState()
        {
            encounter.state = this;
            encounter.boardUpdated = false;

            findScoredRows();

            if(scoredPiece != null) //run score and place animations on the piece, if present
            {
                foreach (block block in scoredPiece.blocks)
                {
                    if (scoredRows.Contains(block.boardpos.y)) //add score decay anim to current piece blocks that are scored
                    {
                        block.animateScore(null);
                    }
                    else //if not scored, then add brief highlight
                    {
                        element e = new element(Game1.boxsolid, Color.White, element.boardPos2ElementPos(block.boardpos), 0.85f);
                        animSprite anim = new animSprite([new animFrame(e, 200)]);
                        encounter.board.sprites.Add(anim);
                    }
                }
            }

            foreach (var scoreOp in encounter.scoreQueue)
            {
                scoreOp.execute(encounter);
            }
            encounter.scoreQueue.Clear();

            //set animhandler to wait
            encounter.animHandler.animState = encounter.animHandler.waitForAnimations;

            //end the current state to wait for animations to play
            //skip to blockTickScoreState after, which finishes up the anim
            animSuspendState newState = new animSuspendState(encounter, stateAfterAnim, true);
            newState.startState();
        }

        void findScoredRows()
        {
            for(int y = 0; y < encounter.board.dimensions.y; y++)
            {
                if (encounter.updatedRows[y])
                {
                    
                    if (isRowScoreable(y))
                    {
                        scoredRows.Add(y);

                        if (scoredPiece == null) //if the row is not scored by a piece, start the score animation from the edges
                        { //rework this so the piece check is outside the loop, as it only needs to be checked once
                            encounter.scoreQueue.Add(scoreRow.queueNonpieceRow(y, encounter.board));
                            continue;
                        }
                        encounter.scoreQueue.Add(scoreRow.queueRowFromPiece(y, encounter.currentPiece, encounter.board));
                    }
                }
            }
        }

        protected bool isRowScoreable(int y) //UPDATE THIS LATER to allow for some empty spots with items - like sleight of hand
        {
            for (int x = 0; x < encounter.board.dimensions.x; x++)
            {
                if (!encounter.board.blocks[x, y].occupiedForScoring) return false; //if any block is empty, return false
                else { continue; } //if the block isn't empty, keep looping
            }
            return true; //if no block return empty, this will run and return true
        }
    }
}

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Quatrimo
{
    public class blockTickScoreState : pieceScoreState
    {
        short scoreIndex = 0;
        short tickIndex = 0;
        List<block> untickedBlocks = new List<block>();
        public blockTickScoreState(encounter main) : base(main)
        {
        }

        //finalize pieceScoreState's operations
        public override void startState()
        {
            encounter.state = this;
            update = tick;
            
            while(scoreIndex < encounter.scoredBlocks.Count)
            {
                block block = encounter.scoredBlocks[scoreIndex];
                scoreIndex++;

                if (block.scored)
                {
                    continue; //skip over scored blocks
                }

                block.removeFromBoard(block); //remove and score block

                block.score(block);
                block.scored = true;
                block.scoreOperation.execute(encounter);

                if (block.scoreOperation.interrupt(encounter)) //if the score operation has an interrupt, suspend the state
                {
                    interruptState();
                    return; //interrupt the stateStart
                }
            }

            //maybe merge this foreach into the while loop
            foreach(var block in encounter.scoredBlocks) //lower all scored blocks and process their score
            {
                //if the block is empty (has been removed) then lower blocks above to fill it in
                if (block.removedFromBoard) { encounter.board.lowerBlock(block); }
                encounter.turnScore += block.getScore(block);
                encounter.turnMultiplier += block.getTimes(block);

                encounter.scoredBlocks.Clear(); //we are done with all of these, so they go byebye!
            }

            scoreIndex = 0;

            //SORT the tickable block list to tick the blocks in order of left -> right, top -> bottom
            sortTickableBlocks();

            //piece score state has been finalized


        }

        protected void tick(GameTime gameTime)
        {
            //tick through the unticked block list
            //interrupting the state to wait for animations where applicable

            //the while loop should tick everything in one go, UNLESS it is interrupted
            //then the state will run startState() again, processing anything scored
            while (tickIndex < untickedBlocks.Count)
            {
                block block = untickedBlocks[tickIndex];
                tickIndex++;

                if (block.ticked) //skip over ticked blocks
                {
                    continue;
                }


                block.tick(block);
                block.ticked = true;
                block.tickOperation.execute(encounter);

                if (block.tickOperation.interrupt(encounter))
                {
                    interruptState();
                    return; //interrupt
                }

            }

            //END OF STATE HERE

            endTurnState newState = new endTurnState(encounter);
            encounter.state = newState;
            newState.startState();
        }
        
        void interruptState()
        {
            encounter.animHandler.animState = encounter.animHandler.waitForAnimations;
            animSuspendState newState = new animSuspendState(encounter, this, true);
            newState.startState();
        }

        /// <summary>
        /// Sort blocks into the unticked block list, going left -> right and top -> bottom
        /// </summary>
        void sortTickableBlocks()
        {
            untickedBlocks.Clear();
            for(int x = 0; x < encounter.board.dimensions.x; x++)
            {
                for(int y = 24; y >= 0; y--) {
                    {
                        block block = encounter.board.blocks[x, y];
                        if (block != null && !block.ticked)
                        {
                            untickedBlocks.Add(block);
                        }
                    } 
                }
            }
        }

        //update function: ticks through every block, when a block needs to handle an animation it interrupts the loop
        //and suspends the state to render the entire animation, then returns and resumes the loop
        //also needs code to handle lowering any rows scored before resuming

        //TICK every block from a LIST, do NOT go through the 2d array of board directly or things may be ticked multiple times!
        //keep track of the current point in the list as the loop WILL be interrupted to wait for animations
    }
}
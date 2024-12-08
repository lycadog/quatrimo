using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quatrimo
{
    public class blockTickScoreState : boardState
    {
        short tickIndex = 0;
        List<block> untickedBlocks = [];//blocks yet to be ticked
        public blockTickScoreState(encounter main) : base(main)
        {
        }

        //finalize previous state's operations
        public override void startState() //process through all scored blocks
        {
            encounter.state = this;
            update = tick;
            interrupted = false;

            Debug.WriteLine($"block tick start: SCOREDBLOCKS COUNT: {encounter.scoredBlocks.Count}");

            while (encounter.scoredBlocks.Count > 0)
            {
                block block = encounter.scoredBlocks[0];
                encounter.scoredBlocks.Remove(block);

                if (block.scored)
                {
                    continue; //skip over scored blocks
                }

                block.finalizeScoring(block);
                if (interrupted) //if the block score method interrupts the state, suspend it
                {
                    Debug.WriteLine("blockTickScoreState: interrupting state due to scoreOperation");
                    interruptState();
                    return; //interrupt the stateStart
                }
            }

            encounter.emptyBlocks.Clear();

            if (encounter.boardUpdated)
            {
                Debug.WriteLine("blockTickScoreState: interrupting state due to boardupdate");
                interruptState();
                return;
                //start updateBoard state
            }
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

                //Debug.WriteLine($"block {block.boardpos.x}, {block.boardpos.y} ticked");

                block.tick(block);
                if (interrupted)
                {
                    interruptState();
                    return; //interrupt
                }
            }

            //END OF STATE HERE

            foreach(var block in untickedBlocks)
            {
                block.ticked = false;
            }

            endTurnState newState = new endTurnState(encounter);
            encounter.state = newState;
            newState.startState();
        }
        
        void interruptState()
        {
            //block score events updating the board might mesh poorly with new updatedRows list, fix later

            processBoardUpdatesState processState = new processBoardUpdatesState(encounter, updatedRows, null, this);
            animSuspendState animState = new animSuspendState(encounter, processState, true);
            animState.startState();
        }

        /// <summary>
        /// Sort blocks into the unticked block list, going left -> right and top -> bottom
        /// </summary>
        void sortTickableBlocks()
        {
            untickedBlocks.Clear();
            for(int x = 0; x < encounter.board.dimensions.x; x++)
            {
                for(int y = 0; y < encounter.board.dimensions.y; y++) {
                    {
                        block block = encounter.board.blocks[x, y];
                        if (block is not emptyBlock && !block.ticked) //remove emptyblock check later
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
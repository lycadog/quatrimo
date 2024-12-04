using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Quatrimo
{
    public class blockTickScoreState : boardState
    {
        short tickIndex = 0;
        List<block> emptyBlocks = [];   //blocks to be filled in by lowering the column above
        List<int> updatedRows = [];
        List<block> untickedBlocks = [];//blocks yet to be ticked
        public blockTickScoreState(encounter main) : base(main)
        {
        }

        //finalize pieceScoreState's operations
        public override void startState() //process through all scored blocks
        {
            encounter.state = this;
            update = tick;

            Debug.WriteLine($"block tick start: SCOREDBLOCKS COUNT: {encounter.scoredBlocks.Count}");

            while (encounter.scoredBlocks.Count > 0)
            {
                block block = encounter.scoredBlocks[0];
                encounter.scoredBlocks.Remove(block);

                if (block.scored)
                {
                    continue; //skip over scored blocks
                }
                emptyBlocks.Add(block);

                block.score(block); //score block and flag for removal
                block.scoreOperation.execute(encounter);
                if (block.scoreOperation.interrupt(encounter)) //if the score operation has an interrupt, suspend the state
                {
                    Debug.WriteLine($"Blocktick state has been interrupted");

                    interruptState();
                    return; //interrupt the stateStart
                }
            }

            //lower all scored blocks and process their score
            foreach (var block in emptyBlocks)
            {
                //if the block is empty (has been removed) then lower blocks above to fill it in
                if (block.markedForRemoval) 
                { 
                    encounter.board.lowerBlock(block); 
                    encounter.boardUpdated = true;
                    updatedRows.Add(block.boardpos.y);
                    updatedRows.Add(block.boardpos.y-1);
                }
                encounter.turnScore += block.getScore(block);
                encounter.turnMultiplier += block.getTimes(block);
            }
            updatedRows = updatedRows.Distinct().ToList();
            emptyBlocks.Clear();

            if (encounter.boardUpdated)
            {
                Debug.WriteLine("board updates processed");
                var nextState = new processBoardUpdatesState(encounter, updatedRows, null, this);
                nextState.startState();
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

            //encounter.animHandler.animState = encounter.animHandler.waitForAnimations;
            //animSuspendState newState = new animSuspendState(encounter, this, true);
            processBoardUpdatesState newState = new processBoardUpdatesState(encounter);
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
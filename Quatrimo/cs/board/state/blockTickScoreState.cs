using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Quatrimo
{
    public class blockTickScoreState : pieceScoreState
    {
        short index = 0;
        List<block> untickedBlocks = new List<block>();
        bool interrupted;
        public blockTickScoreState(encounter main) : base(main)
        {
        }

        //finalize pieceScoreState's operations
        public override void startState()
        {
            encounter.state = this;
            update = tick;
            
            while(index < encounter.scoredBlocks.Count)
            {
                block block = encounter.scoredBlocks[index];
                index++;

                if (block.scored)
                {
                    continue; //skip over scored blocks
                }

                block.score(block);
                block.scoreOperation.execute(encounter);

                if (block.scoreOperation.interrupt(encounter)) //if the score operation has an interrupt, suspend the state
                { 
                    encounter.animHandler.animState = encounter.animHandler.waitForAnimations;
                    animSuspendState newState = new animSuspendState(encounter, this, true);
                    newState.startState();
                    return; //interrupt the stateStart
                }
            }
            
            //LOWER all scored blocks

            //SORT the tickable block list to tick the blocks in order of left -> right, top -> bottom
            //use our sort function i made

            //tick through the unticked block list in the state UPDATE/TICK function,
            //interrupting the state to wait for animations where applicable



            //LOWER rows cleared from pieceScoreState IMMEDIATELY if applicable
            if(encounter.scorableRows.Count > 0)
            {
                encounter.scorableRows.Sort();
                encounter.board.lowerRows(encounter.scorableRows);
            }
            
            
            foreach(block block in encounter.board.blocks)
            {
                if(block != null)
                {
                    untickedBlocks.Add(block);
                }
            }

        }

        protected void tick(GameTime gameTime)
        {
            for(; index < untickedBlocks.Count; index++)
            {
                //tick the block and interrupt the loop if the tick returns true for an interrupt
                if (untickedBlocks[index].tick(untickedBlocks[index]))
                {

                    
                }
            }
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
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Quatrimo
{
    public class blockTickScoreState : pieceScoreState
    {
        short index = 0;
        List<block> blocks = new List<block>();
        bool interrupted;
        public blockTickScoreState(encounter main, bool interrupted = false) : base(main)
        {
            this.interrupted = interrupted;
        }

        public override void startState()
        {
            main.state = this;
            update = tick;

            if (interrupted)
            {

                return;
            }
            

            //LOWER rows cleared from pieceScoreState IMMEDIATELY if applicable
            if(main.scorableRows.Count > 0)
            {
                main.scorableRows.Sort();
                main.board.lowerRows(main.scorableRows);
            }
            
            
            foreach(block block in main.board.blocks)
            {
                if(block != null)
                {
                    blocks.Add(block);
                }
            }

        }

        protected void tick(GameTime gameTime)
        {
            for(; index < blocks.Count; index++)
            {
                //tick the block and interrupt the loop if the tick returns true for an interrupt
                if (blocks[index].tick(blocks[index]))
                {

                    
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
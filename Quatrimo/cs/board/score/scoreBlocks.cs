using System.Collections.Generic;

namespace Quatrimo
{
    public class scoreBlocks : scoreOperation
    {
        List<block> blocks;
        animSprite scoreAnim;

        List<block> scoredBlocks;
        public scoreBlocks(List<block> blocks)
        {
            this.blocks = blocks;
            scoreAnim = animHandler.getDecayingAnim(Vector2I.zero);
        }
        
        public void execute(encounter encounter)
        {
            foreach(var block in blocks)
            {
                if (!block.scored)
                {
                    block.score(block);

                    animSprite anim = new animSprite(scoreAnim.sequence);
                    anim.setPosition(element.boardPos2WorldPos(block.boardpos));
                    encounter.animHandler.animatons.Add(anim);

                    scoredBlocks.Add(block);
                }
            }
        }

        //NEED support for lowering the scoredBlocks list properly later !!!!
        //score operations are used for queueing up scored things
        //the piece score step will queue up and get rid of everything at once, then begin animhandler drawing and waiting on the animations
        //block ticks will be able to use these to interrupt the current state - add another method for that later!

        //when a block is created it will create two score operations, one for score event and tick event
        //these are grabbed off them and ran during the respective piece score state and tick score state
        //we will need another method on scoreOperation interface for interrupts - since interrupts are only needed during ticking
        
        //the scoreOperation tick method should be ran AFTER the execute method on block ticks - execute sets up the animation, the interrupt suspends the state and waits for the animation to finish
        //after this our future lowering rows queue will be ran - it should only have the one operation queued up though

    }
}
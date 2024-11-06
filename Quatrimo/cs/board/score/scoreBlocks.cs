using System.Collections.Generic;

namespace Quatrimo
{
    public class scoreBlocks : scoreOperation
    {
        List<block> blocks;
        animSprite scoreAnim = animHandler.getDecayingAnim(Vector2I.zero);

        List<block> scoredBlocks = [];
        
        public override void execute(encounter encounter)
        {
            foreach(var block in blocks)
            {
                if (!block.scored)
                {
                    block.score(block);

                    animSprite anim = new animSprite(scoreAnim.sequence);
                    anim.setPosition(element.boardPos2WorldPos(block.boardpos));
                    encounter.animHandler.animations.Add(anim);

                    scoredBlocks.Add(block);
                }
            }
        }
        //add new scoreOperation functionality for setting blocks as updated !!!!!
        //maybe put it in a new class?
        //actually i think i will do this in the board, since blocks have a board reference
        //so they can just add the updated block to a list or something similar in the board

        //NEED support for lowering the scoredBlocks list properly later !!!!
        //score operations are used for queueing up scored things
        //the piece score step will queue up and get rid of everything at once, then begin animhandler drawing and waiting on the animations
        //block ticks will be able to use these to interrupt the current state - add another method for that later!
        
        //the scoreOperation interrupt method should be ran AFTER the execute method on block ticks - execute sets up the animation, the interrupt suspends the state and waits for the animation to finish
        //after this our future lowering rows queue will be ran - it should only have the one operation queued up though

    }
}
﻿using System.Collections.Generic;

namespace Quatrimo
{
    public class scoreBlocks : scoreOperation
    {
        List<block> blocks;
        board board;
        animSprite scoreAnim = animHandler.getDecayingAnim(Vector2I.zero);

        public scoreBlocks(List<Vector2I> blocks, board board)
        {
            addBlocks(blocks);
            this.board = board;
        }

        protected void addBlocks(List<Vector2I> blocks)
        {
            foreach (var pos in blocks)
            {
                if (pos.x > 0 | pos.x <= board.dimensions.x) { continue; } //skip if position is out of bounds
                if(pos.y > 0 | pos.y <= board.dimensions.x) { continue; } // ^^^

                this.blocks.Add(board.blocks[pos.x, pos.y]); //add non-out of bounds blocks to the list
            }
        }

        public override void execute(encounter encounter)
        {
            foreach(var block in blocks)
            {
                if (block.scoredAnim) { continue; }

                block.animateScore(encounter, null, );

                
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
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quatrimo
{
    public class scoreBlocks : scoreOperation
    {
        List<block> blocks = [];
        board board;
        animSprite scoreAnim = animHandler.getDecayingAnim(Vector2I.zero);

        public scoreBlocks(List<Vector2I> blocks, board board)
        {
            this.board = board;
            addBlocks(blocks);
        }

        protected void addBlocks(List<Vector2I> positions)
        {

            foreach (var pos in positions)
            {
                Debug.WriteLine($"POS ADDED: {pos.x}, {pos.y}");
                Debug.WriteLine(board.dimensions.x);
                Debug.WriteLine(board.dimensions.y);
                if (pos.x < 0 || pos.x >= board.dimensions.x) { continue; } //skip if position is out of bounds
                if(pos.y < 0 || pos.y >= board.dimensions.y) { continue; } // ^^^
                Debug.WriteLine($"VALID POS: {pos.x}, {pos.y}");

                blocks.Add(board.blocks[pos.x, pos.y]); //add non-out of bounds blocks to the list
            }
        }

        //scored blocks add their blocks to the start of the list, contrary to scored rows
        public override void execute(encounter encounter)
        {
            int counter = 0;
            foreach(var block in blocks)
            {
                if (block.scoredAnim) { continue; }
                Debug.WriteLine("SCOREBLOCK SCORED");
                block.animateScore(null, true);
                encounter.scoredBlocks.Insert(counter, block);
                counter++;
            }
        }
       
        //the scoreOperation interrupt method should be ran AFTER the execute method on block ticks - execute sets up the animation, the interrupt suspends the state and waits for the animation to finish
        //after this our future lowering rows queue will be ran - it should only have the one operation queued up though

    }
}
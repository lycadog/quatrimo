﻿
using System.Diagnostics;

namespace Quatrimo
{
    public class emptyBlock : block
    {
        public emptyBlock(encounter encounter, board board, Vector2I boardpos)
        {
            this.boardpos = boardpos;
            this.encounter = encounter;
            this.board = board;
            occupiedForScoring = false;
        }

        protected override void animateScoreF(bool forceAnim = false)
        {
            if (forceAnim)
            {
                scoredAnim = true;

                animSprite sprite = content.getDecayingAnim(board, board.animationRoot, boardpos);

            }
        }

        protected override bool collidesPlacedF(block falling, block block)
        {
            return false;
        }

        protected override double getMultF(block block)
        {
            return 0;
        }

        protected override long getScoreF(block block)
        {
            return 0;
        }

        protected override void movePlacedF(Vector2I offset, block block)
        {
        }

        protected override bool placedBlockClippedF(block fallingBlock, block block)
        {
            return true;
        }

        protected override void playF(block block)
        {
        }

        protected override void removeFromBoardF(block block)
        {
        }

        protected override void removeSpritesF(block block)
        {
        }
    }
}

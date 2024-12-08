﻿
namespace Quatrimo
{
    public class emptyBlock : block
    {
        public emptyBlock(encounter encounter, Vector2I boardpos)
        {
            this.boardpos = boardpos;
            this.encounter = encounter;
            occupiedForScoring = false;
        }

        public override void animateScore(animation anim, int index = -1, bool forceAnim = false)
        {
            if (index < 0) { encounter.scoredBlocks.Add(this); }
            else { encounter.scoredBlocks.Insert(index, this); }

            if (forceAnim)
            {
                scoredAnim = true;

                animSprite sprite = animHandler.getDecayingAnim(new Vector2I(boardpos.x, boardpos.y));

                board.queuedSprites.Add(sprite);
                encounter.animHandler.animations.Add(sprite);
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

        protected override void createGFXf(block block)
        {
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

        protected override void hideGFXf(block block)
        {
        }
    }
}

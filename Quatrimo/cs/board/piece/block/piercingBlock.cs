namespace Quatrimo
{
    public class piercingBlock : block
    {
        protected override bool collidesFallingF(Vector2I checkPos, block block)
        {
            return isOutOfBounds(checkPos);
        }

        protected override bool collidesPlacedF(block falling, block block)
        {
            base.collidesPlacedF(falling, block);
            return false;
        }

        protected override void createGFXf(block block)
        {
            base.createGFXf(block);
            blockSprite.setRegTexture(content.piercing);
        }

        protected override bool fallingBlockClippedF(block placedBlock, block block)
        {
            placedBlock.animateScore(null);
            placedBlock.score(placedBlock);
            if (placedBlock.removed) { placedBlock.removeFromBoard(placedBlock); return false; }
            return true;
        }

        protected override bool placedBlockClippedF(block fallingBlock, block block)
        {
            animateScore(null);
            score(this);
            removeFromBoard(this);
            return true;
        }
    }
}
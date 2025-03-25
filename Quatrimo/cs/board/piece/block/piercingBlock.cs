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

        protected override regSprite createGFXf(block block)
        {
            regSprite sprite = (regSprite)base.createGFXf(block);
            sprite.tex = content.piercing;
            return sprite;
        }

        protected override bool fallingBlockClippedF(block placedBlock, block block)
        {
            placedBlock.animateScore(false);
            placedBlock.score(placedBlock);
            if (placedBlock.removed) { placedBlock.removeFromBoard(placedBlock); return false; }
            return true;
        }

        protected override bool placedBlockClippedF(block fallingBlock, block block)
        {
            animateScore(false);
            score(this);
            removeFromBoard(this);
            return true;
        }
    }
}
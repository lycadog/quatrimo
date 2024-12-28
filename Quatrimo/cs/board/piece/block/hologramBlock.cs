namespace Quatrimo
{
    public class hologramBlock : block
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
            sprite.tex = texs.hologram1;
        }

        protected override bool placedBlockClippedF(block fallingBlock, block block)
        {
            removeFromBoard(this);
            return true;
        }

        protected override void rotateGFXf(int direction, block block)
        {
        }
    }
}
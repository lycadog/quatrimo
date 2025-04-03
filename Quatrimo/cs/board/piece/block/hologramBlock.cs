using Microsoft.Xna.Framework;

namespace Quatrimo
{
    //next up: maybe some debug tools?
    //bug: sometimes blocks being removed from clipping event mess up and don't remove their sprite/remove block wrongly
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

        protected override animSprite createGFXf(block block)
        {
            animFrame frame1 = new animFrame(new regSprite(content.hologram1, color, 0.8f), 120);
            animFrame frame2 = new animFrame(new regSprite(content.hologram2, color, 0.8f), 120);
            return new animSprite([frame1, frame2], true);
        }

        protected override bool placedBlockClippedF(block fallingBlock, block block)
        {
            removeFromBoard(this);
            return true;
        }

        protected override void placeF(block block)
        {
            base.placeF(block);
            
        }

        protected override void rotateGFXf(int direction, block block)
        {
        }
    }
}
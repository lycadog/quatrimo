namespace Quatrimo
{
    public class piercingBlock : block
    {
        protected override bool collidesFallingF(Vector2I checkPos, block block)
        {
            if (checkPos.x < 0) { return true; }
            if (checkPos.x >= piece.board.dimensions.x) { return true; } //if the tile is outside the board dimensions return true (invalid move)
            if (checkPos.y < 0) { return true; }
            if (checkPos.y >= piece.board.dimensions.y) { return true; }

            return false;
        }

        protected override bool collidesPlacedF(block falling, block block)
        {
            return false;
        }

        protected override void fallingBlockClippedF(block placedBlock, block block)
        {
            placedBlock.removeScored(placedBlock);
            if (!placedBlock.removed) { block.removeFalling(block); }
        }

        protected override bool placedBlockClippedF(block fallingBlock, block block)
        {
            
            return true;
        }
    }
}
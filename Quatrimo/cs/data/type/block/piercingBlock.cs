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

        protected override void createGFXf(block block)
        {
            base.createGFXf(block);
            element.tex = Game1.piercing;
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
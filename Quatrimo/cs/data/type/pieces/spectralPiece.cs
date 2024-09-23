
namespace Quatrimo
{
    internal class spectralPiece : boardPiece
    {
        public override void initializeBlock(block block)
        {
            block.collidesFalling = collidesFallingF;
        }

        bool collidesFallingF(Vector2I checkPos, block block)
        {
            if (checkPos.x < 0) { return true; }
            if (checkPos.x >= board.dimensions.x) { return true; } //if the tile is outside the board dimensions return true (invalid move)
            if (checkPos.y < 0) { return true; }
            if (checkPos.y >= board.dimensions.y) { return true; }

            return false;
        }

        public override void updateDropPos()
        {
            dropOffset = 0;
        }
    }
}

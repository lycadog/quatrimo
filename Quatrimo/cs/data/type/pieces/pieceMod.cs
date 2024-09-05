
using System.Diagnostics;
using static System.Reflection.Metadata.BlobBuilder;

public abstract class pieceMod
{
    public abstract boardPiece piece { get; set; }


    // ======== PIECE METHODS ========
    public virtual bool pCollidesFalling(int xOffset, int yOffset)
    {
        foreach (block block in piece.blocks)
        {
            if (block.collidesFalling(xOffset, yOffset)) { return true; }
        }
        return false;
    }

    public virtual void pPlace()
    {
        foreach (block block in piece.blocks)
        {
            bPlace(block);
        }
    }

    // ======== BLOCK METHODS ========

    /// <summary>
    /// Play the falling block into the board
    /// </summary>
    public virtual void bPlay(block block)
    {
        block.play();
    }

    /// <summary>
    /// Place the block on the board at its current position
    /// </summary>
    public virtual void bPlace(block block)
    {
        block.place();
    }

    /// <summary>
    /// Tick placed block at the end of a turn
    /// </summary>
    public virtual void bTick(block block)
    {
        block.tick();
    }

    /// <summary>
    /// Score block, remove later
    /// </summary>
    public virtual void bScore(block block)
    {
        block.score();
    }

    public virtual bool bCollidesFalling(block block, int xOffset, int yOffset)
    {
        return block.collidesFalling(xOffset, yOffset);
    }
    public virtual bool bCollidesFalling(block block, Vector2I checkPos)
    {
        return block.collidesFalling(checkPos);
    }

    /// <summary>
    /// Returns if the placed block will collide with the falling block
    /// </summary>
    /// <param name="falling"></param>
    /// <returns></returns>
    public virtual bool bCollidesPlaced(block block, block falling)
    {
        return block.collidesPlaced(falling);
    }

    public virtual void bCollidedFalling(block block, Vector2I collisionPos)
    {
        block.collidedFalling(collisionPos);
    }

    public virtual void bCollidedPlaced(block block, block falling)
    {
        block.collidedPlaced(falling);
    }

    /// <summary>
    /// Runs on a falling block when it tries to place on an already placed block, runs AFTER placedBlockClipped
    /// </summary>
    /// <param name="block"></param>
    public virtual void bFallingBlockClipped(block block, block placedBlock)
    {
        block.fallingBlockClipped(placedBlock);
    }

    /// <summary>
    /// Runs on a placed block when a falling block tries to place on its position, runs before placed
    /// </summary>
    /// <param name="fallingBlock"></param>
    public virtual void bPlacedBlockClipped(block block, block fallingBlock)
    {
        block.placedBlockClipped(fallingBlock);
    }

}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class blockType
{   //needs some updating after block rework
    public blockType(block baseBlock, board board)
    {
        this.baseBlock = baseBlock;
        this.board = board;
    }
    public block baseBlock { get; set; }
    public board board { get; set; }
    public abstract Texture2D tex { get; }
    public abstract Color color { get; }
    public abstract long scoreValue { get; set; }
    public abstract long multiplier { get; set; }

    public virtual void rotate(int direction) { }

    public virtual void place() { }
    public virtual void tick() { }
    public virtual void score() { }
    public virtual void removePlaced() { }


    /// <summary>
    /// Returns if the current falling block will collide with the placed block at checkpos position
    /// </summary>
    /// <param name="checkPos"></param>
    /// <returns></returns>
    public virtual bool collidesFalling(block placedBlock)
    {
        return placedBlock.collidesPlaced(baseBlock);
    }

    /// <summary>
    /// Returns if the placed block will collide with the falling block
    /// </summary>
    /// <param name="falling"></param>
    /// <returns></returns>
    public abstract bool collidesPlaced(block fallingBlock);

    /// <summary>
    /// Runs when on a falling block when it collides with a placed block
    /// Will run right before block is placed
    /// </summary>
    public abstract void collidedFalling(Vector2I collisionPos);

    /// <summary>
    /// Runs on a placed block when it collides with a falling block
    /// </summary>
    public abstract void collidedPlaced(block fallingBlock);

    /// <summary>
    /// Runs on a falling block when it tries to place on an already placed block, runs AFTER placedBlockClipped
    /// </summary>
    /// <param name="block"></param>
    public virtual void fallingBlockClipped(block placedBlock)
    {
        placedBlock.removePlaced(); //default behavior is to delete clipped block
    }

    /// <summary>
    /// Runs on a placed block when a falling block tries to place on its position, runs before placed
    /// </summary>
    /// <param name="fallingBlock"></param>
    public abstract void placedBlockClipped(block fallingBlock);

}
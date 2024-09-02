
using Microsoft.Xna.Framework;
using System;

public class block
{
    public board board;
    public block(board board, element element, blockType type, boardPieceold piece, Vector2I localpos)
    {
        this.board = board;
        this.element = element;
        this.type = type;
        this.piece = piece;
        this.localpos = localpos;
    }
    public element element { get; set; }
    public blockType type { get; set; }
    public boardPieceold piece {  get; set; }
    public Vector2I boardpos { get; set; }
    public Vector2I localpos {  get; set; }
    
    /// <summary>
    /// Play the falling block into the board
    /// </summary>
    public void play()
    {
        updatePos();
        board.sprites.Add(element);
    }

    /// <summary>
    /// Rotates the block along the piece's origin by changing the local position
    /// *** DIRECTION MUST BE -1 OR 1 ***
    /// </summary>
    /// <param name="direction"></param>
    public void rotate(int direction)
    {
        localpos =  new Vector2I(localpos.y * -direction, localpos.x * direction);
        type.rotate(direction);
        element.rot += MathHelper.ToRadians(90 * direction);
    }

    /// <summary>
    /// Place the block on the board at its current position
    /// </summary>
    public void place()
    {
        board.blocks[boardpos.x, boardpos.y] = this;
        type.place();
    }

    /// <summary>
    /// Update the board and element position
    /// </summary>
    public void updatePos()
    {
        boardpos = piece.pos.add(localpos);
        element.elementPos = element.boardPos2ElementPos(boardpos);
    }

    /// <summary>
    /// Tick at the end of a turn
    /// </summary>
    public void tick()
    {
        type.tick();
    }

    /// <summary>
    /// Remove falling block from the board
    /// </summary>
    public void removeFalling()
    {
        board.sprites.Remove(element);
    }

    /// <summary>
    /// Remove placed block from the board
    /// </summary>
    public void removePlaced()
    {
        board.blocks[boardpos.x, boardpos.y] = null;
        board.sprites.Remove(element);
        type.removePlaced();
    }
   
    /// <summary>
    /// Score block
    /// </summary>
    public void score()
    {
        type.score();
    }
    
    /// <summary>
    /// Get block score
    /// </summary>
    /// <returns></returns>
    public long getScore()
    {
        return type.scoreValue;
    }


    /// <summary>
    /// Returns whether the current falling tile will collide with the offset position
    /// True = Collision
    /// </summary>
    /// <param name="xOffset"></param>
    /// <param name="yOffset"></param>
    /// <returns></returns>
    public bool collidesFalling(int xOffset, int yOffset) 
    {
        Vector2I checkPos = new Vector2I(boardpos.x + xOffset, boardpos.y + yOffset);

        if (checkPos.x < 0) { return true; }
        if (checkPos.x >= board.dimensions.x) { return true; } //if the tile is outside the board dimensions return true (invalid move)
        if (checkPos.y < 0) { return true; }
        if (checkPos.y >= board.dimensions.y) { return true; }

        block block = board.blocks[checkPos.x, checkPos.y];
        if(block == null) { return false; }
        return type.collidesFalling(block);
    }

    /// <summary>
    /// Returns if the placed block will collide with the falling block
    /// </summary>
    /// <param name="falling"></param>
    /// <returns></returns>
    public bool collidesPlaced(block falling)
    {
        return type.collidesPlaced(falling);
    }

    /// <summary>
    /// Runs when on a falling block when it collides with a placed block
    /// Will run right before block is placed
    /// </summary>
    public void collidedFalling(Vector2I collisionPos)
    {
        type.collidedFalling(collisionPos);
    }

    /// <summary>
    /// Runs on a placed block when it collides with a falling block
    /// </summary>
    public void collidedPlaced(block fallingBlock)
    {
        type.collidedPlaced(fallingBlock);
    }

    /// <summary>
    /// Runs on a falling block when it tries to place on an already placed block, runs AFTER placedBlockClipped
    /// </summary>
    /// <param name="block"></param>
    public void fallingBlockClipped(block placedBlock)
    {
        type.fallingBlockClipped(placedBlock);
    }

    /// <summary>
    /// Runs on a placed block when a falling block tries to place on its position, runs before placed
    /// </summary>
    /// <param name="fallingBlock"></param>
    public void placedBlockClipped(block fallingBlock)
    {
        type.placedBlockClipped(fallingBlock);
    }

}

using Microsoft.Xna.Framework;
using Quatrimo;
using System.Diagnostics;

public class block
{
    public board board;
    public block(board board, blockType type, boardPiece piece, Vector2I localpos)
    {
        this.board = board;
        this.type = type;
        this.piece = piece;
        this.localpos = localpos;
        piece.mod.bGraphicsInit(this);
    }
    public element element { get; set; }
    public element dropElement { get; set; }
    public blockType type { get; set; }
    public boardPiece piece {  get; set; }
    public Vector2I boardpos { get; set; }
    public Vector2I localpos {  get; set; }
    
    /// <summary>
    /// Play the falling block into the board
    /// </summary>
    public void play()
    {
        updatePos();
        updateSpritePos();
        board.sprites.Add(element);
        board.sprites.Add(dropElement);
    }

    public void graphicsInit()
    {
        element = new element(type.getTex(piece), type.getColor(piece), new Vector2I(0, -5), 0.8f); //create new sprite element
        dropElement = new element(Game1.full25, Color.LightGray, new Vector2I(0, -10), 0.79f);
    }

    /// <summary>
    /// Used to move PLACED PIECES only
    /// </summary>
    /// <param name="xoffset"></param>
    /// <param name="yoffset"></param>
    public void movePlaced(int xoffset, int yoffset)
    {
        board.blocks[boardpos.x, boardpos.y] = null;
        board.blocks[boardpos.x + xoffset, boardpos.y + yoffset] = this;
        boardpos = new Vector2I(boardpos.x + xoffset, boardpos.y + yoffset);
        element.offsetEPos(new Vector2I(xoffset, yoffset));
    }

    /// <summary>
    /// Rotates the block along the piece's origin by changing the local position
    /// *** DIRECTION MUST BE -1 OR 1 ***
    /// </summary>
    /// <param name="direction"></param>
    public void rotate(int direction)
    {
        localpos = getRotatePos(direction);
        type.rotate(direction);
        element.rot += MathHelper.ToRadians(90 * direction);
    }

    /// <summary>
    /// Returns the local position after a rotation operation in the specificed direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Vector2I getRotatePos(int direction)
    {
        return new Vector2I(localpos.y * -direction, localpos.x * direction);
    }

    /// <summary>
    /// Place the block on the board at its current position
    /// </summary>
    public void place()
    {
        board.blocks[boardpos.x, boardpos.y] = this;
        element.depth = .75f;
        type.place();
        board.sprites.Remove(dropElement);
    }

    /// <summary>
    /// Update the board and element position
    /// </summary>
    public void updatePos()
    {
        boardpos = localpos.add(piece.pos);
    }

    public void updateSpritePos()
    {
        if(boardpos.y > 7)
        {
            element.updateEPos(element.boardPos2ElementPos(boardpos));
        }
        else { element.updateEPos(new Vector2I(0, -5)); }
        dropElement.updateEPos(element.boardPos2ElementPos(new Vector2I(boardpos.x, boardpos.y + piece.dropOffset)));
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
        board.sprites.Remove(dropElement);
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
    /// Score block, remove later
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


    public spriteObject createPreview()
    {
        spriteObject sprite = new spriteObject(new Vector2I(4, 4));
        sprite.depth = .93f;
        sprite.tex = Game1.full; sprite.color = element.color;
        return sprite;
    }

    /// <summary>
    /// Returns whether the current falling tile will collide with the offset position
    /// True = Collision
    /// </summary>
    /// <param name="xOffset"></param>
    /// <param name="yOffset"></param>
    /// <returns></returns>

    public bool collidesFalling(Vector2I checkPos)
    {
        block block = board.blocks[checkPos.x, checkPos.y];
        if (block == null) { return false; }
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
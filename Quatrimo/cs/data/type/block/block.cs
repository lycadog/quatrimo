﻿
using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class block
    {
        public board board;
        public block()
        {
            linkDelegates();
        }
        public scoreOperation scoreOperation = new emptyScoreOperation();
        public scoreOperation tickOperation = new emptyScoreOperation();
        public element element { get; set; }
        public element dropElement { get; set; }
        public boardPiece piece { get; set; }
        public Vector2I boardpos { get; set; }
        public Vector2I localpos { get; set; }
        public Texture2DRegion tex { get; set; }
        public Color color { get; set; }
        public bool scoredAnim { get; set; } //if the block has had the scoring animation run over it
        public bool scored { get; set; } //if the block has been actually scored
        public bool markedForRemoval { get; set; } //if the block has been removed from the board and should be filled in
        public bool ticked { get; set; }

        public void linkDelegates()
        {
            play = new blockD(playF);
            place = new blockD(placeF);
            score = new blockD(scoreF);
            tick = new tickD(tickF);
            createGFX = new blockD(graphicsInit);
            createPreview = new spriteD(createPreviewF);
            updatePos = new blockD(updatePosF);
            updateSpritePos = new blockD(updateSpritePosF);
            removeFalling = new blockD(removeFallingF);
            hideGFX = new blockD(removeGFXF);
            removeFromBoard = new blockD(removeFromBoardF);

            collidedFalling = new coordinateD(collidedFallingF);
            collidedPlaced = new blockVoidD(collidedPlacedF);
            movePlaced = new coordinateD(movePlacedF);

            placedBlockClipped = new blockBoolD(placedBlockClippedF);
            fallingBlockClipped = new blockVoidD(fallingBlockClippedF);

            collidesFalling = new coordinateBoolD(collidesFallingF);
            collidesPlaced = new blockBoolD(collidesPlacedF);

            rotate = new rotationD(rotateF);
            getRotatePos = new rotationVectorD(getRotatePosF);

            getScore = new scoreD(getScoreF);
            getTimes = new scoreD(getMultF);
        }
        /// <summary>
        /// Play the falling block into the board
        /// </summary>
        public blockD play;

        /// <summary>
        /// Place block at current board position
        /// </summary>
        public blockD place;

        /// <summary>
        /// Score block, does not remove the block
        /// </summary>
        public blockD score;

        /// <summary>
        /// Ticks block at the end of a turn, returns whether or not it creates an animation that needs to be waited on (to suspend the state)
        /// </summary>
        public tickD tick;

        /// <summary>
        /// Initialize the graphics needed for the block
        /// </summary>
        public blockD createGFX;

        /// <summary>
        /// Initialize the drop preview for the currently falling block
        /// </summary>
        public spriteD createPreview;

        /// <summary>
        /// Update the block's board position
        /// </summary>
        public blockD updatePos;

        /// <summary>
        /// Update the sprites of the block to match its board position
        /// </summary>
        public blockD updateSpritePos;

        /// <summary>
        /// Remove the falling block from play
        /// </summary>
        public blockD removeFalling;

        /// <summary>
        /// Hide the block's sprites
        /// </summary>
        public blockD hideGFX;

        /// <summary>
        /// Remove the placed block from the board
        /// </summary>
        public blockD removeFromBoard;

        /// <summary>
        /// Run collided while falling event
        /// </summary>
        public coordinateD collidedFalling;

        /// <summary>
        /// Run collided while placed event
        /// </summary>
        public blockVoidD collidedPlaced;

        /// <summary>
        /// Runs on a placed block has a falling block attempt placement on its position, runs before fallingBlockClipped
        /// </summary>
        public blockBoolD placedBlockClipped;

        /// <summary>
        /// Runs when a falling block attempts to place on top of a placed block, runs AFTER placedBlockClipped
        /// </summary>
        public blockVoidD fallingBlockClipped;

        /// <summary>
        /// Used to move placed pieces
        /// </summary>
        public coordinateD movePlaced;

        /// <summary>
        /// Checks falling block collision with the specified offset
        /// </summary>
        public coordinateBoolD collidesFalling;

        /// <summary>
        /// Checks if the placed block and falling block should collide
        /// </summary>
        public blockBoolD collidesPlaced;

        /// <summary>
        ///  Rotates the block along the piece's origin by changing the local position
        /// *** DIRECTION MUST BE -1 OR 1 ***
        /// </summary>
        public rotationD rotate;

        /// <summary>
        /// Returns the local position after a rotation operation in the specificed direction
        /// </summary>
        public rotationVectorD getRotatePos;

        /// <summary>
        /// Gets the score value of the block
        /// </summary>
        public scoreD getScore;

        /// <summary>
        /// Gets the multiplier the block will add to the current score operation
        /// </summary>
        public scoreD getTimes;

        //really really messy but whatever
        public delegate void blockD(block block);
        public delegate bool tickD(block block);
        public delegate void coordinateD(Vector2I pos, block block);
        public delegate bool coordinateBoolD(Vector2I pos, block block);
        public delegate void blockVoidD(block otherBlock, block block);
        public delegate bool blockBoolD(block otherBlock, block block);
        public delegate void rotationD(int direction, block block);
        public delegate Vector2I rotationVectorD(int direction, block block);
        public delegate spriteObject spriteD(block block);
        public delegate long scoreD(block block);


        protected virtual void playF(block block)
        {
            updatePos(this);
            updateSpritePos(this);
            board.sprites.Add(element);
            board.sprites.Add(dropElement);
        }

        protected virtual void graphicsInit(block block)
        {
            element = new element(tex, color, new Vector2I(0, -5), 0.8f); //create new sprite element
            dropElement = new element(Game1.dropmark, Color.LightGray, new Vector2I(0, -10), 0.79f);
        }

        protected virtual void movePlacedF(Vector2I offset, block block)
        {
            board.blocks[boardpos.x, boardpos.y] = null;
            board.blocks[boardpos.x + offset.x, boardpos.y + offset.y] = this;
            boardpos = new Vector2I(boardpos.x + offset.x, boardpos.y + offset.y);
            element.offsetEPos(new Vector2I(offset.x, offset.y));
        }


        protected virtual void rotateF(int direction, block block)
        {
            localpos = getRotatePos(direction, this);
            element.rot += MathHelper.ToRadians(90 * direction);
        }

        protected Vector2I getRotatePosF(int direction, block block) //direction is 1 or -1
        {
            return new Vector2I(localpos.y * -direction, localpos.x * direction);
        }

        protected virtual void placeF(block block)
        {
            block clipped = board.blocks[boardpos.x, boardpos.y];
            if (clipped != null)
            {
                if (!clipped.placedBlockClipped(block, clipped))
                {
                    block.fallingBlockClipped(clipped, block);
                    return;
                }
            }
            board.blocks[boardpos.x, boardpos.y] = this;
            element.depth = .75f;
            board.sprites.Remove(dropElement);
        }

        protected void updatePosF(block block)
        {
            boardpos = localpos.add(piece.pos);
            //updateSpritePos(block);
        }

        protected virtual void updateSpritePosF(block block)
        {
            if (boardpos.y > 7)
            {
                element.setEPos(element.boardPos2ElementPos(boardpos));
            }
            else { element.setEPos(new Vector2I(0, -5)); }
            dropElement.setEPos(element.boardPos2ElementPos(new Vector2I(boardpos.x, boardpos.y + piece.dropOffset)));
        }

        protected virtual bool tickF(block block)
        {
            return false;
        }

        protected virtual void removeFallingF(block block)
        {
            board.sprites.Remove(element);
            board.sprites.Remove(dropElement);
        }

        protected virtual void removeGFXF(block block)
        {
            board.staleSprites.Add(element);
        }

        protected virtual void removeFromBoardF(block block)
        {
            board.blocks[boardpos.x, boardpos.y] = null;
        }

        

        protected virtual void scoreF(block block)
        {
            scored = true;
        }


        protected virtual long getScoreF(block block)
        {
            return 1;
        }

        protected virtual long getMultF(block block)
        {
            return 0;
        }

        protected virtual spriteObject createPreviewF(block block)
        {
            spriteObject sprite = new spriteObject();
            sprite.size = new Vector2I(5, 5);
            sprite.depth = .93f;
            sprite.tex = Game1.solid; sprite.color = element.color;
            
            return sprite;
        }

        protected virtual bool collidesFallingF(Vector2I checkPos, block block)
        {
            if (checkPos.x < 0) { return true; }
            if (checkPos.x >= piece.board.dimensions.x) { return true; } //if the tile is outside the board dimensions return true (invalid move)
            if (checkPos.y < 0) { return true; }
            if (checkPos.y >= piece.board.dimensions.y) { return true; }

            block hitBlock = board.blocks[checkPos.x, checkPos.y];
            if (hitBlock != null)
            {
                return hitBlock.collidesPlaced(block, hitBlock);
            }
            return false;
        }

        protected virtual bool collidesPlacedF(block falling, block block)
        {
            return true;
        }

        protected virtual void collidedFallingF(Vector2I collisionPos, block block) { }

        protected virtual void collidedPlacedF(block fallingBlock, block block) { }

        /// <summary>
        /// Runs on a falling block when it tries to place on an already placed block, runs AFTER placedBlockClipped
        /// </summary>
        /// <param name="block"></param>
        protected virtual void fallingBlockClippedF(block placedBlock, block block)
        {
            block.removeFalling(block);
        }

        /// <summary>
        /// Runs on a placed block when a falling block tries to place on its position; Returns if the placed block will remove itself or not
        /// </summary>
        /// <param name="fallingBlock"></param>
        protected virtual bool placedBlockClippedF(block fallingBlock, block block)
        {
            return false;
        }

    }
}
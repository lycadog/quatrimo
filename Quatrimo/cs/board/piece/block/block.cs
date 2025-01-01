
using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using System;

namespace Quatrimo
{
    public class block
    {
        public encounter encounter;
        public board board;
        public block()
        {
            linkDelegates();
        }

        public void createBlock(encounter encounter, boardPiece piece, Vector2I localpos, Texture2DRegion tex, Color color)
        {
            this.encounter = encounter;
            board = encounter.board;
            this.piece = piece;
            this.localpos = localpos;
            this.tex = tex;
            this.color = color;
            createGFX(this);
        }

        public boardPiece piece;

        public Vector2I boardpos;
        public Vector2I localpos;

        public blockSprite sprite;
        public blockSprite dropSprite;
        public blockSprite dropCorners;
        public Texture2DRegion tex;
        public Color color;

        public long scoreValue = 1;
        public double multiplier = 0;

        public bool occupiedForScoring = true;  //whether or not the block can fill rows for scoring - almost all blocks should!

        public bool scoredAnim = false;         //if the block has had the scoring animation run over it
        public bool scored = false;             //if the block has been actually scored
        public bool removed = false;   //if the block has been removed from the board and should be filled in
        public bool ticked = false;             //ticked by blockTickScoreState

        public void linkDelegates()
        {
            play = new Action<block>(playF);
            place = new Action<block>(placeF);
            score = new Action<block>(scoreF);
            finalizeScoring = new Action<block>(finalizeScoringF);
            tick = new Action<block>(tickF);
            createGFX = new Action<block>(createGFXf);
            createPreview = new Func<block, sprite>(createPreviewF);
            updatePos = new Action<block>(updatePosF);
            updateSpritePos = new Action<block>(updateSpritePosF);
            removeFalling = new Action<block>(removeFallingF);
            removeSprites = new Action<block>(removeSpritesF);
            removeFromBoard = new Action<block>(removeFromBoardF);

            scoreRemoveGFX = new Action<block>(scoreRemoveGFXf);

            collidedFalling = new Action<block, block>(collidedFallingF);
            collidedPlaced = new Action<block, block>(collidedPlacedF);
            movePlaced = new Action<Vector2I, block>(movePlacedF);

            placedBlockClipped = new Func<block, block, bool>(placedBlockClippedF);
            fallingBlockClipped = new Func<block, block, bool>(fallingBlockClippedF);

            collidesFalling = new Func<Vector2I, block, bool>(collidesFallingF);
            collidesPlaced = new Func<block, block, bool>(collidesPlacedF);

            rotate = new Action<int, block>(rotateF);
            rotateGFX = new Action<int, block>(rotateGFXf);
            getRotatePos = new Func<int, block, Vector2I>(getRotatePosF);

            getScore = new Func<block, long>(getScoreF);
            getTimes = new Func<block, double>(getMultF);
        }

        // =|||||||= [ NONDELEGATE METHODS ] =|||||||= >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        /// <summary>
        /// Adds the block to the scored block list and renders the score animation on top of it; forceAnim is for enabling animations on empty blocks only
        /// </summary>
        /// <param name="encounter"></param>
        /// <param name="anim"></param>
        public virtual void animateScore(drawable anim, bool forceAnim = false) //TODO: add support for overriding the default anim
        {
            scoreRemoveGFX(this);
            scoredAnim = true;

            animSprite sprite = animHandler.getDecayingAnim(new Vector2I(boardpos.x, boardpos.y));

            board.addSprite(sprite);
            encounter.animHandler.animations.Add(sprite);
        }

        // =|||||||= [ DELEGATE DECLARATIONS ] =|||||||= >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        /// <summary>
        /// Play the falling block into the board
        /// </summary>
        public Action<block> play;
        protected virtual void playF(block block)
        {
            updatePos(this);

            sprite.setState(0);
            dropSprite.setState(0);
            dropCorners.setState(0);

            board.addSprite(sprite);
            board.addSprite(dropSprite);
            board.addSprite(dropCorners);
        }

        /// <summary>
        /// Place block at current board position
        /// </summary>
        public Action<block> place;
        protected virtual void placeF(block block)
        {
            block clipped = board.blocks[boardpos.x, boardpos.y];

            if (!clipped.placedBlockClipped(block, clipped))
            {
                if (block.fallingBlockClipped(clipped, block)) { return; }
            }

            board.blocks[boardpos.x, boardpos.y] = this;

            sprite.depth = .75f;
            dropSprite.setState(2);
            dropCorners.setState(2);
        }

        /// <summary>
        /// Called by blockTickState to fully score and remove a block, do NOT call anywhere else
        /// </summary>
        public Action<block> finalizeScoring;
        protected virtual void finalizeScoringF(block block)
        {
            score(this);
            encounter.board.lowerBlock(block);
        }

        /// <summary>
        /// Score block, does not remove the block
        /// </summary>
        public Action<block> score;
        protected virtual void scoreF(block block)
        {
            scored = true;
            removed = true; //marks if the block is/will be removed
            encounter.turnScore += getScore(this);
            encounter.turnMultiplier += getTimes(this);
        }

        /// <summary>
        /// Ticks block at the end of a turn, returns whether or not it creates an animation that needs to be waited on (to suspend the state)
        /// </summary>
        public Action<block> tick;
        protected virtual void tickF(block block)
        {
            ticked = true;
        }

        /// <summary>
        /// Initialize the graphics needed for the block
        /// </summary>
        public Action<block> createGFX;
        protected virtual void createGFXf(block block)
        {
            sprite = new blockSprite(this, tex, color); //create new sprite element

            dropSprite = new blockSprite(this, texs.dropCrosshair, new Color(180, 180, 220)); //create new sprite element
            dropCorners = new blockSprite(this, texs.dropCorners, Color.White, 0.81f); //create new sprite element
        }

        /// <summary>
        /// Initialize the drop preview for the currently falling block
        /// </summary>
        public Func<block,sprite> createPreview;
        protected virtual sprite createPreviewF(block block)
        {
            sprite sprite = new sprite();
            sprite.size = new Vector2I(5, 5);
            sprite.depth = .93f;
            sprite.tex = texs.solid; sprite.color = block.color;

            return sprite;
        }

        /// <summary>
        /// Update the block's board position
        /// </summary>
        public Action<block> updatePos;
        protected void updatePosF(block block)
        {
            boardpos = localpos + piece.pos;
        }

        /// <summary>
        /// Update the sprites of the block to match its board position
        /// </summary>
        public Action<block> updateSpritePos;
        protected virtual void updateSpritePosF(block block)
        {
            sprite.updatePos();
            sprite.checkOutOfBounds();

            dropSprite.offset = new Vector2I(0,piece.dropOffset);
            dropCorners.offset = new Vector2I(0, piece.dropOffset);

            dropSprite.updatePos();
            dropCorners.updatePos();
        }

        /// <summary>
        /// Remove the falling block from play
        /// </summary>
        public Action<block> removeFalling;
        protected virtual void removeFallingF(block block)
        {
            removeSprites(block);
        }

        /// <summary>
        /// Hide the block's sprites
        /// </summary>
        public Action<block> removeSprites;
        protected virtual void removeSpritesF(block block)
        {
            sprite.setState(2);
            dropSprite.setState(2);
            dropCorners.setState(2);
        }

        /// <summary>
        /// Forcefully remove the placed block from the board, does NOT lower the column above - be careful
        /// </summary>
        public Action<block> removeFromBoard;
        protected virtual void removeFromBoardF(block block)
        {
            removeSprites(block);
            board.markEmpty(boardpos);
        }

        /// <summary>
        /// Attempt to remove the gfx for scoring
        /// </summary>
        public Action<block> scoreRemoveGFX;
        //Used during the score step to attempt gfx removal - if block should not be scored, do not remove gfx
        protected virtual void scoreRemoveGFXf(block block)
        {
            removeSprites(block);
        }

        /// <summary>
        /// Block collided with another while falling
        /// </summary>
        public Action<block, block> collidedFalling;
        protected virtual void collidedFallingF(block otherblock, block block) { }

        /// <summary>
        /// Run collided while placed event | T1:otherBlock - T2:block
        /// </summary>
        public Action<block, block> collidedPlaced;
        protected virtual void collidedPlacedF(block fallingBlock, block block) { }

        /// <summary>
        /// Runs on a placed block has a falling block attempt placement on its position, runs before fallingBlockClipped - returns if the block will remove itself
        /// </summary>
        public Func<block, block, bool> placedBlockClipped;

        /// <param name="fallingBlock"></param>
        protected virtual bool placedBlockClippedF(block fallingBlock, block block)
        {
            return false;
        }

        /// <summary>
        /// Runs when a falling block attempts to place on top of a placed block, runs AFTER placedBlockClipped - returns if it will remove itself or not
        /// </summary>
        public Func<block, block, bool> fallingBlockClipped;
        protected virtual bool fallingBlockClippedF(block placedBlock, block block)
        {
            block.removeFalling(block);
            return true;
        }

        /// <summary>
        /// Used to move placed pieces
        /// </summary>
        public Action<Vector2I,block> movePlaced;
        protected virtual void movePlacedF(Vector2I offset, block block)
        {
            board.markEmpty(boardpos);
            board.blocks[boardpos.x + offset.x, boardpos.y + offset.y] = this;

            boardpos = new Vector2I(boardpos.x + offset.x, boardpos.y + offset.y);
            updateSpritePos(block);

            encounter.boardUpdated = true;
            encounter.updatedRows[boardpos.y] = true;
        }

        /// <summary>
        /// Checks falling block collision with the specified offset
        /// </summary>
        public Func<Vector2I, block, bool> collidesFalling;
        protected virtual bool collidesFallingF(Vector2I checkPos, block block)
        {
            if (isOutOfBounds(checkPos)) { return true; }

            block hitBlock = board.blocks[checkPos.x, checkPos.y];

            return hitBlock.collidesPlaced(block, hitBlock);
        }

        protected virtual bool isOutOfBounds(Vector2I checkPos)
        {
            //if the tile is outside the board dimensions return true (invalid move)
            if (checkPos.x < 0) { return true; }
            if (checkPos.x >= board.dimensions.x) { return true; }
            if (checkPos.y < 0) { return true; }
            if (checkPos.y >= board.dimensions.y) { return true; }
            else return false;
        }

        /// <summary>
        /// Checks if the placed block and falling block should collide
        /// </summary>
        public Func<block, block, bool> collidesPlaced;
        protected virtual bool collidesPlacedF(block falling, block block)
        {
            falling.collidedFalling(block, falling);
            block.collidedPlaced(falling, block);
            return true;
        }

        /// <summary>
        ///  Rotates the block along the piece's origin by changing the local position
        /// *** DIRECTION MUST BE -1 OR 1 ***
        /// </summary>
        public Action<int, block> rotate;
        protected virtual void rotateF(int direction, block block)
        {
            localpos = getRotatePos(direction, this);
            rotateGFX(direction, this);
        }

        /// <summary>
        /// Rotates the block's sprites
        /// </summary>
        public Action<int, block> rotateGFX;
        protected virtual void rotateGFXf(int direction, block block)
        {
            sprite.rot += MathHelper.ToRadians(90 * direction);
        }

        /// <summary>
        /// Returns the local position after a rotation operation in the specificed direction
        /// </summary>
        public Func<int, block, Vector2I> getRotatePos;
        protected Vector2I getRotatePosF(int direction, block block) //direction is 1 or -1
        {
            return new Vector2I(localpos.y * -direction, localpos.x * direction);
        }

        /// <summary>
        /// Gets the score value of the block
        /// </summary>
        public Func<block, long> getScore;
        protected virtual long getScoreF(block block)
        {
            return scoreValue;
        }

        /// <summary>
        /// Gets the multiplier the block will add to the current score operation
        /// </summary>
        public Func<block, double> getTimes;
        protected virtual double getMultF(block block)
        {
            return multiplier;
        }
    }
}
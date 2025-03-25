
using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using System;
using System.Diagnostics;

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
        }

        public boardPiece piece;

        public Vector2I boardpos;
        public Vector2I localpos;

        //update code to move the elementPos when moving block
        //parent block to board on creation
        //parent sprites to block and set drop position accordingly
        //parent dropCorners to dropSprite
        public drawObject blockRoot;
        public regSprite blockSprite;
        public regSprite dropPreview;

        public sprite blockSpriteOLD;
        public sprite dropSpriteOLD;
        public sprite dropCornersOLD;
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
            animateScore = new Action<bool>(animateScoreF);
            finalizeScoring = new Action<block>(finalizeScoringF);
            tick = new Action<block>(tickF);
            createGFX = new Func<block, drawObject>(createGFXf);
            createPreview = new Func<block, spriteOld>(createPreviewF);
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

        /// <summary>
        /// Play the falling block into the board
        /// </summary>
        public Action<block> play;
        protected virtual void playF(block block)
        {
            blockRoot.setParent(board.boardRoot);
            //create sprite
            blockSprite = (regSprite)createGFX(this);
            blockSprite.setParent(blockRoot);

            dropPreview = new(blockRoot, content.dropCrosshair, new Color(180, 180, 220), 0.79f);
            new regSprite(dropPreview, content.dropCorners, Color.White, 0.81f);
            //create drop preview
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

            blockSprite.depth = .75f;
            dropPreview.dispose();
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
        /// Adds the block to the scored block list and renders the score animation on top of it; forceAnim is for enabling animations on empty blocks only
        /// </summary>
        /// <param name="encounter"></param>
        /// <param name="anim"></param>
        public Action<bool> animateScore;
        protected virtual void animateScoreF(bool forceAnim = false) //TODO: add support for overriding the default anim
        {
            scoreRemoveGFX(this);
            scoredAnim = true;

            content.getDecayingAnim(board, board.animationRoot, boardpos); //create animation and parent it to animation root
        }

        /// <summary>
        /// Ticks block at the end of a turn
        /// </summary>
        public Action<block> tick;
        protected virtual void tickF(block block)
        {
            ticked = true;
        }

        /// <summary>
        /// Initialize the graphics needed for the block, then returns them
        /// </summary>
        public Func<block, drawObject> createGFX;
        protected virtual drawObject createGFXf(block block)
        {
            return new regSprite(tex, color, 0.8f);
        }

        /// <summary>
        /// Initialize the drop preview for the currently falling block
        /// DEPRECATED, REMOVE SOON
        /// </summary>
        public Func<block,spriteOld> createPreview;
        protected virtual spriteOld createPreviewF(block block)
        {
            spriteOld sprite = new spriteOld();
            sprite.size = new Vector2I(5, 5);
            sprite.depth = .93f;
            sprite.tex = content.solid; sprite.color = block.color;

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
            blockRoot.elementPos = boardpos;
            if (boardpos.y < 4) { blockSprite.hide(); } //if sprite is above the board, hide it
            else { blockSprite.unhide(); }
            dropPreview.elementPos = new Vector2I(0, piece.dropOffset);

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
        /// Delete the block's sprites
        /// </summary>
        public Action<block> removeSprites;
        protected virtual void removeSpritesF(block block)
        {
            blockRoot.dispose();
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
            blockSprite.localRot += MathHelper.ToRadians(90 * direction);
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
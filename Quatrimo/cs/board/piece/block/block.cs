
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

        public element element { get; set; }
        public element dropElement { get; set; }
        public boardPiece piece { get; set; }
        public Vector2I boardpos { get; set; }
        public Vector2I localpos { get; set; }
        public Texture2DRegion tex { get; set; }
        public Color color { get; set; }

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
            createPreview = new Func<block, spriteObject>(createPreviewF);
            updatePos = new Action<block>(updatePosF);
            updateSpritePos = new Action<block>(updateSpritePosF);
            removeFalling = new Action<block>(removeFallingF);
            hideGFX = new Action<block>(hideGFXf);
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
        public virtual void animateScore(animation anim, bool forceAnim = false) //TODO: add support for overriding the default anim
        {
            scoreRemoveGFX(this);
            scoredAnim = true;

            animSprite sprite = animHandler.getDecayingAnim(new Vector2I(boardpos.x, boardpos.y));

            board.queuedSprites.Add(sprite);
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
            updateSpritePos(this);
            board.sprites.Add(element);
            board.sprites.Add(dropElement);
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
            element.depth = .75f;
            board.sprites.Remove(dropElement);
        }

        //todo: rework block scoring methods for easier use
        //different methods depending on boardstate and other things like fully removing the block or animating it
        //need a seperate one for scoring it and getting it outta here.
        //and a new one for animating but not adding it to be removed later

        //i think weve pretty much done this now, what next?
        //piercing blocks!!!!!


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
            element = new element(tex, color, new Vector2I(0, -5), 0.8f); //create new sprite element
            dropElement = new element(texs.dropmark, Color.LightGray, new Vector2I(0, -10), 0.79f);
        }

        /// <summary>
        /// Initialize the drop preview for the currently falling block
        /// </summary>
        public Func<block,spriteObject> createPreview;
        protected virtual spriteObject createPreviewF(block block)
        {
            spriteObject sprite = new spriteObject();
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
            if (boardpos.y > 7)
            {
                element.setEPos(element.boardPos2ElementPos(boardpos));
            }
            else { element.setEPos(new Vector2I(0, -5)); }
            dropElement.setEPos(element.boardPos2ElementPos(new Vector2I(boardpos.x, boardpos.y + piece.dropOffset)));
        }

        /// <summary>
        /// Remove the falling block from play
        /// </summary>
        public Action<block> removeFalling;
        protected virtual void removeFallingF(block block)
        {
            board.sprites.Remove(element);
            board.sprites.Remove(dropElement);
        }

        /// <summary>
        /// Hide the block's sprites
        /// </summary>
        public Action<block> hideGFX;
        protected virtual void hideGFXf(block block)
        {
            board.staleSprites.Add(element);
        }

        /// <summary>
        /// Forcefully remove the placed block from the board, does NOT lower the column above - be careful
        /// </summary>
        public Action<block> removeFromBoard;
        protected virtual void removeFromBoardF(block block)
        {
            board.markEmpty(boardpos);
        }

        /// <summary>
        /// Attempt to remove the gfx for scoring
        /// </summary>
        public Action<block> scoreRemoveGFX;
        //Used during the score step to attempt gfx removal - if block should not be scored, do not remove gfx
        protected virtual void scoreRemoveGFXf(block block)
        {
            hideGFX(block);
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
            if (checkPos.x < 0) { return true; }
            if (checkPos.x >= board.dimensions.x) { return true; } //if the tile is outside the board dimensions return true (invalid move)
            if (checkPos.y < 0) { return true; }
            if (checkPos.y >= board.dimensions.y) { return true; }

            block hitBlock = board.blocks[checkPos.x, checkPos.y];

            return hitBlock.collidesPlaced(block, hitBlock);
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
            element.rot += MathHelper.ToRadians(90 * direction);
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
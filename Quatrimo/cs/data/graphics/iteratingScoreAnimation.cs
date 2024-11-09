﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Quatrimo
{
    public class iteratingScoreAnimation : animation
    {
        encounter encounter;
        board board;
        animHandler animHandler;

        int y;                  //the row being scored
        int[] positions = [2];  //the positions of the left and right iterator

        animSprite decayAnim = animHandler.getDecayingAnim(Vector2I.zero);

        double timer = 0;
        bool animFinished = false;

        public iteratingScoreAnimation(encounter encounter, animHandler animHandler, int y, int[] positions)
        {
            this.encounter = encounter;
            board = encounter.board;
            this.animHandler = animHandler;
            this.y = y;
            this.positions = positions;
        }

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime, board board)
        {
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer < 25) { return; }
                
            animFinished = true;
    
            iterateDirection(0, -1);               
            iterateDirection(1, 1);

            timer = 0;
    
            if (animFinished)
            {
                completed = true;
                board.staleSprites.Add(this);
            }

        }

        public override void terminate()
        {
            throw new System.NotImplementedException();
        }

        void iterateDirection(int index, int offset) //offset is either 1 or -1
        {
            int pos = positions[index] + offset;
            positions[index] = pos;

            if (pos < 0 || pos >= board.dimensions.x)
            {
                //iterator has completed
                return;
            }

            animFinished = false;

            block block = board.blocks[pos, y];

            if(block == null) { return; }

            if(!block.scoredAnim)
            {
                
                encounter.scoredBlocks.Add(block);

                block.hideGFX(block);
                block.scoredAnim = true;

                decayAnim.returnValues(out animFrame[] sequence, out _, out _);
                animSprite spriteold = new animSprite(sequence);
                spriteold.setPosition(element.boardPos2WorldPos(new Vector2I(pos, y)));

                animSprite sprite = animHandler.getDecayingAnim(new Vector2I(pos,y));

                board.queuedSprites.Add(sprite);
                animHandler.animations.Add(sprite);
                
            }
        }
    }
}
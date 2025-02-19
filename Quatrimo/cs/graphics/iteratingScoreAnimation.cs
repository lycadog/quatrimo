using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quatrimo
{
    public class iteratingScoreAnimation : IDrawable
    {
        encounter encounter;
        board board;
        animHandler animHandler;

        public spriteManager manager { get; set; }
        public bool stale { get; set; }

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

        

        public void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer < 25) { return; }

            animFinished = true;

            iterateDirection(0, -1);
            iterateDirection(1, 1);

            timer = 0;

            if (animFinished)
            {
               dispose();
            }
        }

        void iterateDirection(int index, int offset) //offset is either 1 or -1
        {
            int pos = positions[index] + offset;
            positions[index] = pos;

            if (pos < 0 || pos >= board.dimensions.x) { return; } //return if iterator has completed
            animFinished = false; //flag anim as not completed, since iterator is not done

            block block = board.blocks[pos, y];

            if(!block.scoredAnim)
            {
                block.animateScore(null, true); //TODO: change anim later so we can override it from the default
                encounter.scoredBlocks.Add(block);
            }
        }

        public void dispose()
        {
            stale = true;
            manager.remove(this);
        }
    }
}
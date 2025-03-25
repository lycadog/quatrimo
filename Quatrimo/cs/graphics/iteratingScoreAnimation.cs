using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quatrimo
{
    public class iteratingScoreAnimation : drawObject
    {
        encounter encounter;
        board board;
        animHandler animHandler;

        int y;                  //the row being scored
        int[] positions = [2];  //the positions of the left and right iterator

        double timer = 0;

        Func<board, drawObject, Vector2I, animSprite> animFactory;

        public iteratingScoreAnimation(drawObject parent, encounter encounter, animHandler animHandler, int y, int[] positions, Func<animSprite> animOverride = null)
        {
            setParent(parent);
            if(animOverride == null) { animFactory = content.getDecayingAnim; }
            this.encounter = encounter;
            board = encounter.board;
            this.animHandler = animHandler;
            this.y = y;
            this.positions = positions;
        }

        

        public override void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer < 25) { return; }

            stale = true;

            iterateDirection(0, -1);
            iterateDirection(1, 1);

            timer = 0;

            if (stale)
            {
               dispose();
            }
        }

        void iterateDirection(int index, int offset) //offset is either 1 or -1
        {
            int pos = positions[index] + offset;
            positions[index] = pos;

            if (pos < 0 || pos >= board.dimensions.x) { return; } //return if iterator has completed
            stale = false; //flag anim as not completed, since iterator is not done

            block block = board.blocks[pos, y];

            if(!block.scoredAnim)
            {
                block.animateScore(true); //TODO: change anim later so we can override it from the default
                encounter.scoredBlocks.Add(block);
            }
        }
    }
}
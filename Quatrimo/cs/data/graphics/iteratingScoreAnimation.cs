using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        bool animFinished;

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
            animFinished = true;

            if(timer > 25)
            {
                iterateDirection(0, -1);
                iterateDirection(1, 1);

                timer = 0;
            }

            if (animFinished)
            {
                completed = true;
            }

            timer += gameTime.ElapsedGameTime.TotalMilliseconds;

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

            if(block.scored)
            {
                encounter.scoredBlocks.Add(block);
                block.removeGFX(block);
                block.scoredAnim = true;
                //MARK BLOCK AS SCORED SOMEHOW, MAYBE INTERRUPT THE ANIM TO SCORE THEM

                decayAnim.returnValues(out animFrame[] sequence, out _, out _);
                animSprite sprite = new animSprite(sequence);
                sprite.setPosition(element.boardPos2WorldPos(new Vector2I(pos, y)));

                board.sprites.Add(sprite);
                animHandler.animations.Add(sprite);
                
            }


        }


    }
}
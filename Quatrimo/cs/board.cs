using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quatrimo
{
    public class board
    {
        public encounter encounter;

        public block[,] blocks;

        public List<int> updatedRows = []; //each int corresponds to the Y position of a row
        public List<Vector2I> queuedLowerBlocks = []; //empty scored blocks queued to be filled by lowering blocks above, no duplicates allowed

        public List<drawable> sprites = [];
        public List<drawable> queuedSprites = [];
        public List<drawable> staleSprites = [];
        public Vector2I dimensions;
        public static readonly Vector2I eDimensions = new Vector2I(48, 27); //element dimensions
        public static Vector2I offset;
        public pieceBox nextbox;
        public pieceBox holdbox;
        public spriteObject pauseText = new spriteObject();

        public static readonly int boardy = 22;

        public board(encounter encounter, Vector2I dim)
        {
            this.encounter = encounter;
            dimensions = dim;
            blocks = new block[dimensions.x, dimensions.y];
            
            for(int x = 0; x < dimensions.x; x++)
            {
                for(int y = 0; y < dimensions.y; y++)
                {
                    blocks[x, y] = new emptyBlock(encounter, this, new Vector2I(x, y));
                }
            }

            offset = new Vector2I((eDimensions.x - dimensions.x) / 2 - 1, 4);
            createBoardElements();
        }

        /// <summary>
        /// Draws the entire board scene
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach(var sprite in queuedSprites)
            {
                sprites.Add(sprite);
            }
            queuedSprites.Clear();

            foreach (var sprite in sprites)
            {
                sprite.draw(spriteBatch, gameTime, this);
            }

            foreach (var staleSprite in staleSprites)
            {
                sprites.Remove(staleSprite);
            }
            staleSprites.Clear();
        }


        /// <summary>
        /// Creates sprites needed for the board border and background
        /// </summary>
        public void createBoardElements()
        {
            nextbox = new pieceBox(new Vector2I(110, 200), Game1.nextBox);
            holdbox = new pieceBox(new Vector2I(110, 140), Game1.holdBox);
            pauseText.tex = Game1.pausedtext;
            pauseText.depth = 1;
            pauseText.size = new Vector2I(120, 40);
            pauseText.pos = new Vector2I(180, 100);
            sprites.Add(nextbox);
            sprites.Add(holdbox);

            sprites.Add(new element(Game1.borderDL, Color.White, new Vector2I(1, 0), 0.9f)); //top bar
            sprites.Add(new element(Game1.borderDR, Color.White, new Vector2I(46, 0), 0.9f));

            for (int i = 2; i < eDimensions.x - 2; i++) //create top border
            {
                if (i == 19) //create title
                {
                    sprites.Add(new element(Game1.nameQ, new Color(new Vector3(1f, 0.067f, 0.933f)), new Vector2I(20, 0), 0.9f));
                    sprites.Add(new element(Game1.nameU, new Color(new Vector3(1f, 0.067f, 0.933f)), new Vector2I(21, 0), 0.9f));
                    sprites.Add(new element(Game1.nameA, new Color(new Vector3(1f, 0.067f, 0.933f)), new Vector2I(22, 0), 0.9f));
                    sprites.Add(new element(Game1.nameT, new Color(new Vector3(0.133f, 1f, 0.8f)), new Vector2I(23, 0), 0.9f));
                    sprites.Add(new element(Game1.nameR, new Color(new Vector3(0.133f, 1f, 0.8f)), new Vector2I(24, 0), 0.9f));
                    sprites.Add(new element(Game1.nameI, new Color(new Vector3(0.133f, 1f, 0.8f)), new Vector2I(25, 0), 0.9f));
                    sprites.Add(new element(Game1.nameM, new Color(new Vector3(0.667f, 0f, 1f)), new Vector2I(26, 0), 0.9f));
                    sprites.Add(new element(Game1.nameO, new Color(new Vector3(0.667f, 0f, 1f)), new Vector2I(27, 0), 0.9f));
                }
                else if (i > 19 && i < 28) { continue; }
                sprites.Add(new element(Game1.borderD, Color.White, new Vector2I(i, 0), 0.9f));
            }

            //create board border

            sprites.Add(new element(Game1.borderUL, Color.White, new Vector2I(offset.x, offset.y - 1), 0.9f)); // create corner pieces
            sprites.Add(new element(Game1.borderDL, Color.White, new Vector2I(offset.x, boardy + 2), 0.9f));
            sprites.Add(new element(Game1.borderUR, Color.White, new Vector2I(offset.x + dimensions.x + 1, offset.y - 1), 0.9f));
            sprites.Add(new element(Game1.borderDR, Color.White, new Vector2I(offset.x + dimensions.x + 1, boardy + 2), 0.9f));


            for (int x = 0; x < dimensions.x; x++) //create top/bottom border
            {
                sprites.Add(new element(Game1.borderU, Color.White, new Vector2I(offset.x + x + 1, offset.y - 1), 0.9f));
                sprites.Add(new element(Game1.borderD, Color.White, new Vector2I(offset.x + x + 1, boardy + 2), 0.9f));
            }

            for (int y = 0; y < boardy - 2; y++) //create left/right border
            {
                sprites.Add(new element(Game1.borderL, Color.White, new Vector2I(offset.x, y + offset.y), 0.9f));
                sprites.Add(new element(Game1.borderR, Color.White, new Vector2I(offset.x + dimensions.x + 1, y + offset.y), 0.9f));
            }

            var sprite = new spriteObject(); //create black box behind board
            sprite.size = element.elementPos2WorldPos(new Vector2I(14, 22));
            sprite.tex = Game1.solid;
            sprite.color = Color.Black;
            sprite.pos = element.elementPos2WorldPos(new Vector2I(offset.x, offset.y - 1));
            sprite.depth = 0.05f;
            sprites.Add(sprite);

            short counter = 0;
            Color[] colors = [new Color(new Vector3(0.04f, 0.04f, 0.04f)), new Color(new Vector3(0.06f, 0.06f, 0.06f))];
            for (int x = 0; x < dimensions.x; x++) //generate board square background
            {
                for (int y = 0; y < boardy - 2; y++)
                {
                    int dif = Math.Abs(y - (boardy / 2) + 1);

                    float multiplier = (float)((Math.Pow(dif, 2) * 0.02f) + 1);
                    Vector3 vector = colors[counter % 2].ToVector3() * new Vector3(multiplier, multiplier, multiplier);
                    Color color = new Color(vector);
                    sprites.Add(new element(Game1.box, color, new Vector2I(offset.x + x + 1, y + offset.y), 0.1f));
                    counter++;
                }
                counter++;
            }

        }

        /// <summary>
        /// Lowers rows to fill the empty space left behind by the specified scored block
        /// </summary>
        /// <param name="block"></param>
        public void lowerBlock(block block)
        {
            int x = block.boardpos.x;
            block.removeFromBoard(block);

            for (int y = block.boardpos.y-1; y > 1; y--)
            {

                block loweredBlock = blocks[x, y];

                if(loweredBlock == null) { continue; }

                blocks[x, y+1] = loweredBlock;
                loweredBlock.movePlaced(new Vector2I(0, 1), loweredBlock);
            }
        }

        public void markEmpty(Vector2I pos)
        {
            blocks[pos.x, pos.y] = new emptyBlock(encounter, this, pos);
        }

    }
}
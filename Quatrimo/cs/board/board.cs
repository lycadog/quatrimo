using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quatrimo
{
    public class board
    {
        //messy code using queuedSpriteChanges list in drawable class, requires ref parameters and general messiness
        //maybe make a new system for that eventually - perhaps it could do sorting as well
        
        public encounter encounter;

        public block[,] blocks;

        public List<drawable> sprites = [];
        public Action<List<drawable>> queuedSpriteChanges; //REMOVE THIS completely later

        public static Vector2I dimensions;
        public static readonly Vector2I eDimensions = new Vector2I(48, 27); //element dimensions
        public static Vector2I offset;
        public pieceBox nextbox;
        public pieceBox holdbox;
        public sprite pauseText = new sprite();

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
            queuedSpriteChanges?.Invoke(sprites);
            queuedSpriteChanges = null;

            foreach (var sprite in sprites)
            {
                sprite.draw(spriteBatch, gameTime, ref queuedSpriteChanges);
            }
        }

        public void addSprite(drawable sprite)
        {
            queuedSpriteChanges += ((List<drawable> list) => { list.Add(sprite); });
        }

        /// <summary>
        /// Creates sprites needed for the board border and background
        /// </summary>
        public void createBoardElements()
        {
            nextbox = new pieceBox(new Vector2I(110, 200), content.nextBox);
            holdbox = new pieceBox(new Vector2I(110, 140), content.holdBox);
            pauseText.tex = content.pausedtext;
            pauseText.depth = 1;
            pauseText.size = new Vector2I(120, 40);
            pauseText.worldPos = new Vector2I(180, 100);
            sprites.Add(nextbox);
            sprites.Add(holdbox);

            sprites.Add(new regSprite(content.borderDL, new Vector2I(1, 0), Color.White, 0.9f)); //top bar
            sprites.Add(new regSprite(content.borderDR, new Vector2I(46, 0), Color.White, 0.9f));

            //new Color(255, 17, 237), new Color(34, 255, 204), new Color(172, 0, 255)
            for (int i = 2; i < eDimensions.x - 2; i++) //create top border
            {
                if (i == 19) //create title
                {
                    sprites.Add(new regSprite(content.nameQ, new Vector2I(20, 0), new Color(255, 17, 237), 0.9f));
                    sprites.Add(new regSprite(content.nameU, new Vector2I(21, 0), new Color(255, 17, 237), 0.9f));
                    sprites.Add(new regSprite(content.nameA, new Vector2I(22, 0), new Color(255, 17, 237), 0.9f));
                    sprites.Add(new regSprite(content.nameT, new Vector2I(23, 0), new Color(34, 255, 204), 0.9f));
                    sprites.Add(new regSprite(content.nameR, new Vector2I(24, 0), new Color(34, 255, 204), 0.9f));
                    sprites.Add(new regSprite(content.nameI, new Vector2I(25, 0), new Color(34, 255, 204), 0.9f));
                    sprites.Add(new regSprite(content.nameM, new Vector2I(26, 0), new Color(172, 0, 255), 0.9f));
                    sprites.Add(new regSprite(content.nameO, new Vector2I(27, 0), new Color(172, 0, 255), 0.9f));
                }
                else if (i > 19 && i < 28) { continue; }
                sprites.Add(new regSprite(content.borderD, new Vector2I(i, 0), Color.White, 0.9f));
            }

            //create board border

            sprites.Add(new regSprite(content.borderUL, Color.White, new Vector2I(-1, 7), 0.9f)); // create corner pieces
            sprites.Add(new regSprite(content.borderDL, Color.White, new Vector2I(-1, 28), 0.9f));
            sprites.Add(new regSprite(content.borderUR, Color.White, new Vector2I(dimensions.x, 7), 0.9f));
            sprites.Add(new regSprite(content.borderDR, Color.White, new Vector2I(dimensions.x, 28), 0.9f));


            for (int x = 0; x < dimensions.x; x++) //create top/bottom border
            {
                sprites.Add(new regSprite(content.borderU, Color.White, new Vector2I(x, 7), 0.9f));
                sprites.Add(new regSprite(content.borderD, Color.White, new Vector2I(x, 28), 0.9f));
            }

            for (int y = 8; y < 28; y++) //create left/right border
            {
                sprites.Add(new regSprite(content.borderL, Color.White, new Vector2I(-1, y), 0.9f));
                sprites.Add(new regSprite(content.borderR, Color.White, new Vector2I(dimensions.x, y), 0.9f));
            }

            var sprite = new sprite(content.solid, new Vector2I(offset.x, offset.y - 1), Color.Black, 0.05f); //create black box behind board
            sprite.size = new Vector2I(140, 220);
            sprite.origin = Vector2.Zero;
            sprites.Add(sprite);

            short counter = 0;
            Color[] colors = [new Color(new Vector3(0.04f, 0.04f, 0.04f)), new Color(new Vector3(0.06f, 0.06f, 0.06f))];
            for (int x = 0; x < dimensions.x; x++) //generate board square background
            {
                for (int y = 7; y < 27; y++)
                {
                    int dif = Math.Abs(y - 7 - (22 / 2) + 1); //MATH HERE might be broken with new values for height, fix later

                    float multiplier = (float)((Math.Pow(dif, 2) * 0.02f) + 1);
                    Vector3 vector = colors[counter % 2].ToVector3() * new Vector3(multiplier, multiplier, multiplier);
                    Color color = new Color(vector);
                    sprites.Add(new regSprite(content.box, color, new Vector2I(x, y + 1), 0.1f));
                    counter++;
                }
                counter++;
            }

            Color[] colrs = [new Color(255, 17, 237), new Color(34, 255, 204), new Color(172, 0, 255)];
            for(int i = 0; i < 200; i++)
            {
                var qSprite = new regSprite
                {
                    tex = content.nameQ,
                    color = colrs[util.rand.Next(0, 3)],
                    worldPos = new Vector2I(150, 200),
                    depth = 1,
                };

                var qJumper = new movingParticle(4, qSprite, new Vector2(240, 135), new Vector2(0, 0), new Vector2(0, 0), 3000, true);
                sprites.Add(qJumper);
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
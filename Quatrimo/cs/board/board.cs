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

        public spriteManager spritesOLD = new spriteManager();

        public static Vector2I dimensions;
        public static readonly Vector2I eDimensions = new Vector2I(48, 27); //element dimensions
        public static Vector2I offset;
        public pieceBoxOld nextbox;
        public pieceBoxOld holdbox;
        public spriteOld pauseText = new spriteOld();

        public static readonly int boardy = 22;
        public static drawObject baseParent = stateManager.baseParent;
        public drawObject boardRoot = new drawObject() { elementPos = new Vector2I(offset.x, -3) };

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
        /// Creates sprites needed for the board border and background
        /// </summary>
        public void createBoardElements()
        {
            /*nextbox = new pieceBoxOld(new Vector2I(110, 200), content.nextBox);
            holdbox = new pieceBoxOld(new Vector2I(110, 140), content.holdBox);
            
            spritesOLD.add(nextbox);
            spritesOLD.add(holdbox);*/
            pauseText.tex = content.pausedtext;
            pauseText.depth = 1;
            pauseText.size = new Vector2I(120, 40);
            pauseText.worldPos = new Vector2I(180, 100);

            new regSprite(new Vector2I(1, 0), content.borderDL, Color.White, 0.9f);
            new regSprite(new Vector2I(46, 0), content.borderDR, Color.White, 0.9f);

            //new Color(255, 17, 237), new Color(34, 255, 204), new Color(172, 0, 255)
            for (int i = 2; i < eDimensions.x - 2; i++) //create top border
            {
                if (i == 19) //create title
                {
                    new regSprite(new Vector2I(20, 0), content.nameQ, new Color(255, 17, 237), 0.9f);
                    new regSprite(new Vector2I(21, 0), content.nameU, new Color(255, 17, 237), 0.9f);
                    new regSprite(new Vector2I(22, 0), content.nameA, new Color(255, 17, 237), 0.9f);
                    new regSprite(new Vector2I(23, 0), content.nameT, new Color(34, 255, 204), 0.9f);
                    new regSprite(new Vector2I(24, 0), content.nameR, new Color(34, 255, 204), 0.9f);
                    new regSprite(new Vector2I(25, 0), content.nameI, new Color(34, 255, 204), 0.9f);
                    new regSprite(new Vector2I(26, 0), content.nameM, new Color(172, 0, 255), 0.9f);
                    new regSprite(new Vector2I(27, 0), content.nameO, new Color(172, 0, 255), 0.9f);

                }
                else if (i > 19 && i < 28) { continue; }
                new regSprite(new Vector2I(i, 0), content.borderD, Color.White, 0.9f);
            }

            //create board border

            new regSprite(boardRoot, new Vector2I(-1, 7), content.borderUL, Color.Wheat, 0.9f);
            new regSprite(boardRoot, new Vector2I(19, 7), content.borderUR, Color.Wheat, 0.9f);
            new regSprite(boardRoot, new Vector2I(-1, 28), content.borderDL, Color.Wheat, 0.9f);
            new regSprite(boardRoot, new Vector2I(19, 28), content.borderDR, Color.Wheat, 0.9f);

            for (int x = 0; x < dimensions.x; x++) //create top/bottom border
            {
                new regSprite(boardRoot, new Vector2I(x, 7), content.borderU, Color.Wheat, 0.9f);
                new regSprite(boardRoot, new Vector2I(x, 28), content.borderD, Color.Wheat, 0.9f);
            }

            for (int y = 8; y < 28; y++) //create left/right border
            {
                new regSprite(boardRoot, new Vector2I(-1, y), content.borderL, Color.Wheat, 0.9f);
                new regSprite(boardRoot, new Vector2I(12, y), content.borderR, Color.Wheat, 0.9f);
            }

            new sprite(boardRoot, new Vector2I(-1, 7), content.solid, Color.Black, 0.05F) {
                size = new Vector2I(140, 220),
                origin = Vector2I.zero
            };

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
                    
                    new regSprite(boardRoot, new Vector2I(x, y + 1), content.box, color, 0.1f);
                    counter++;
                }
                counter++;
            }

            /*Color[] colrs = [new Color(255, 17, 237), new Color(34, 255, 204), new Color(172, 0, 255)];
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
                spritesOLD.add(qJumper);
            }*/
            
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
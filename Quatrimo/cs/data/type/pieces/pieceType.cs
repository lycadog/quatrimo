using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Quatrimo
{
    public class pieceType
    {
        public pieceType(int xSize, int ySize, baseblock[,] blocks, wSet<basePiece> mod, Vector2I origin, int blockCount, string name, colorSet color, Texture2D tex)
        {
            dimensions = new Vector2I(xSize, ySize);
            blockSet = blocks;
            this.mod = mod;
            this.origin = origin;
            this.blockCount = blockCount;
            this.name = name;
            this.colorSet = color;
            this.tex = tex;
        }

        public pieceType(int xSize, int ySize, wSet<baseblockType>[,] blocks, wSet<basePiece> mod, Vector2I origin, int blockCount, string name, colorSet color, Texture2D tex)
        {
            dimensions = new Vector2I(xSize, ySize);
            this.mod = mod;
            this.origin = origin;
            this.blockCount = blockCount;
            this.name = name;
            this.colorSet = color;
            this.tex = tex;

            blockSet = new baseblock[xSize, ySize];
            
            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    if (blocks[x,y] != null)
                    {
                        blockSet[x,y] = new baseblock(blocks[x, y]);
                    }
                }
            }
        }

        public baseblock[,] blockSet { get; set; }
        public wSet<basePiece> mod { get; set; }
        public Vector2I dimensions { get; set; }
        public Vector2I origin { get; set; }
        public int blockCount { get; set; }
        public string name { get; set; }
        public rarity rarity { get; set; }
        public colorSet colorSet { get; set; } //properly define and use colorsets
        public Texture2D tex { get; set; }

        public void addToBag(bag bag) //add new piece to player's bag
        {
            baseblock[] blocks = new baseblock[blockCount];
            Color color = colorSet.getRandomColor();

            int index = 0;

            for (int x = 0; x < dimensions.x; x++)
            {
                for (int y = 0; y < dimensions.y; y++)
                {

                    if (blockSet[x, y] != null)
                    {

                        baseblock block = blockSet[x, y];
                        block.chosenBlock = block.blocks.getRandom();

                        block.localpos = new Vector2I(x - origin.x, y - origin.y);

                        if (block.tex == null)
                        {
                            block.tex = tex;
                        }
                        if (!block.isColored)
                        {
                            block.color = color;
                        }

                        blocks[index] = block;
                        index++;
                    }
                }
            }

            bagPiece piece = new bagPiece(blocks, mod.getRandom(), dimensions, origin, blockCount, name, rarity, colorSet.getRandomColor(), tex); //create new piece

            bag.pieces.Add(piece); //add new piece to player's bag
            Debug.WriteLine("piece " + piece.name + " added!");
        }
    }
}
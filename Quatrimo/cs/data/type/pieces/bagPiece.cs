using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using System.Diagnostics;

namespace Quatrimo
{
    public class bagPiece //used for pieces held in the bag
    { //nest this in board piece maybe?
        public bagPiece(baseblock[] blocks, basePiece mod, Vector2I dimensions, Vector2I origin, int blockCount, string name, rarity rarity, Color color, Texture2DRegion tex)
        {
            this.blocks = blocks;
            this.mod = mod;
            this.dimensions = dimensions;
            this.origin = origin;
            this.blockCount = blockCount;
            this.name = name;
            this.rarity = rarity;
            this.color = color;
            this.tex = tex;
            weight = 1;
        }

        /// <summary>
        /// Serializes the bag piece into a playable board piece
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public boardPiece getBoardPiece(board board)
        {
            block[] newBlocks = new block[blockCount];

            boardPiece piece = mod.getPiece();

            for(int i = 0; i < blockCount; i++)
            {
                block block = blocks[i].getBlock();
                block.board = board;
                block.piece = piece;
                block.localpos = blocks[i].localpos;

                block.tex = blocks[i].tex;
                block.color = blocks[i].color;
                piece.initializeBlock(block);
                block.createGFX(block);
                Debug.WriteLine(block.tex);

                newBlocks[i] = block;

            }

            piece.setData(board, newBlocks, dimensions, origin, name, tex, color);

            return piece;
        }

        public baseblock[] blocks { get; set; }
        public basePiece mod { get; set; }
        public Vector2I dimensions { get; set; }
        public Vector2I origin { get; set; }
        public int blockCount { get; set; }
        public float weight { get; set; }
        public string name { get; set; }
        public Color color { get; set; }
        public Texture2DRegion tex { get; set; }
        public rarity rarity { get; set; }
    }
}
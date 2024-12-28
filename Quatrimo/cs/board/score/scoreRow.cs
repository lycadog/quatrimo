using MonoGame.Extended.ViewportAdapters;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace Quatrimo
{
    public class scoreRow : scoreOperation
    {
        int y;
        Vector2I leftBounds; //the left outer and inner bounds of the piece, used for iteratoranim 1
        Vector2I rightBounds; //the right outer and inner bounds of the piece, used for iteratoranim 2

        public scoreRow(int y, Vector2I leftBounds, Vector2I rightBounds)
        {
            this.y = y;
            this.leftBounds = leftBounds;
            this.rightBounds = rightBounds;
        }

        public override void execute(encounter encounter)
        {
            //also figure out how to properly suspend animhandler state for animSuspendState
            iteratingScoreAnimation leftIterator = new iteratingScoreAnimation(encounter, encounter.animHandler, y, [leftBounds.x, leftBounds.y]);
            iteratingScoreAnimation rightIterator = new iteratingScoreAnimation(encounter, encounter.animHandler, y, [rightBounds.x, rightBounds.y]);
            
            encounter.board.addSprite(leftIterator);
            encounter.board.addSprite(rightIterator);
            
            encounter.animHandler.animations.Add(leftIterator);
            encounter.animHandler.animations.Add(rightIterator);

            encounter.turnRowsCleared += 1;
            Debug.WriteLine("row executed");
        }


        /// <summary>
        /// return a scoreRow operation with animation starting at the bounds of a piece
        /// </summary>
        /// <param name="y"></param>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static scoreRow queueRowFromPiece(int y, boardPiece piece, board board) // ============ DO LATER ==============
        {
            Vector2I left = new Vector2I(0, 0);
            Vector2I right = new Vector2I(0, -1);

            bool outer = true; //fix bad code later
            
            for(int x = 0; x < board.dimensions.x; x++)
            {
                Debug.WriteLine(x);
                if (outer)
                {
                    if (board.blocks[x, y].piece == piece)
                    {
                        left.x = x;
                        outer = false;
                    }
                }
                
                if(!outer)
                {
                    if (board.blocks[x, y].piece != piece)
                    {
                        left.y = x - 1;
                        break;
                    }
                }
            }

            outer = true;

            for (int x = board.dimensions.x-1; x > -1; x--)
            {
                if (outer)
                {
                    if (board.blocks[x, y].piece == piece)
                    {
                        right.x = x;
                        outer = false;
                    } 
                }

                if(!outer)
                {
                    if (board.blocks[x, y].piece != piece)
                    {
                        right.y = x + 1;
                        break;
                    }
                }
            }

            Debug.WriteLine($"VECTORS: L({left.x},{left.y}), R({right.x},{right.y})");

            return new scoreRow(y, left, right);
        }

        public static scoreRow queueNonpieceRow(int y, board board)
        {
            return new scoreRow(y, new Vector2I(0, 0), new Vector2I(board.dimensions.x, board.dimensions.x));
        }

    }
}
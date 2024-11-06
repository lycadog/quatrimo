using MonoGame.Extended.ViewportAdapters;
using System.Numerics;

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
            //NEXT DAY CODING: FIGURE OUT WHAT THE HELL TO PUT HERE ====================
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// return a scoreRow operation with animation starting at the bounds of a piece
        /// </summary>
        /// <param name="y"></param>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static scoreRow queueRowFromPiece(int y, boardPiece piece) // ============ DO LATER ==============
        {
            foreach (var block in piece.blocks) //find the start points for row score animation for each row !!!!
            {
                if (block.boardpos.y == y)
                {
                    //find score animation initialization algorithm later
                }
            }
        }

        public static scoreRow queueNonpieceRow(int y, board board)
        {
            return new scoreRow(y, new Vector2I(0, 0), new Vector2I(board.dimensions.x, board.dimensions.x));
        }

    }
}
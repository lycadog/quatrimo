using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Quatrimo
{
    public class encounter
    {

        public board board;
        public animHandler animHandler;
        public boardState state;

        public bag bag;

        public short level = 0;
        public short rowsRequired = 4;
        public short rowsCleared = 0;
        public short turnRowsCleared = 0;

        public long totalScore = 0;
        public long turnScore = 0;

        public double levelTimes = 1;
        public double turnMultiplier = 1;

        public boardPiece currentPiece;
        public boardPiece heldPiece = null;
        public boardPiece nextPiece;

        public bool boardUpdated = false;

        public List<int> updatedRows = [];
        public List<scoreOperation> scoreQueue = []; //move to processBoardUpdatesState
        public List<block> scoredBlocks = [];

        public encounter(bag bag)
        {
            this.bag = bag;

            animHandler = new animHandler(this);
            board = new board(new Vector2I(12, 28), animHandler);

            nextPiece = bag.getPiece(board);

            turnStartState newState = new turnStartState(this);
            newState.startState();
        }

        public void update(GameTime gameTime)
        {
             state.update(gameTime);
        }
    }
}
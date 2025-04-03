using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Quatrimo
{
    public class encounter
    {
        public board board;
        public animHandler animHandler;
        public boardState state;
        public runData runData;

        public bag bag;

        public short level = 0;
        public short rowsRequired = 4;
        public short rowsCleared = 0;
        public short turnRowsCleared = 0;

        public long totalScore = 0;
        public long turnScore = 0;

        public double levelTimes = 1;
        public double turnMultiplier = 1;

        public pieceCardOld[] hand = [];

        public boardPiece currentPiece;
        public boardPiece heldPiece = null;
        public boardPiece nextPiece;

        public bool boardUpdated = false;
        public bool[] updatedRows;

        public List<int> updatedRowsOLD = [];
        public List<scoreOperation> scoreQueue = []; //move to processBoardUpdatesState

        public List<block> scoredBlocks = [];
        public List<block> emptyBlocks = [];

        public encounter(bag bag, runData data)
        {
            animHandler = new animHandler(this);
            board = new board(this, new Vector2I(12, 28));

            this.bag = bag;
            runData = data;
            bag.encounter = this;

            nextPiece = bag.drawRandomPiece();

            turnStartState newState = new turnStartState(this);
            newState.startState();
        }

        public void update(GameTime gameTime)
        {
             state.update(gameTime);
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace Quatrimo
{
	//main is for all things encounters!
	public class mainOLD
	{
		/*
		//this class and board needs a complete rewrite

		public gameState stateOld;

		public board board;
		public animHandler animHandler;
		public boardState state;

		public bag bag;

		//board variables
		public short level = 0;
		public short rowsRequired = 4;
		public short rowsCleared = 0;
		public short turnRowsCleared = 0;

		public long turnScore = 0;
		public long totalScore = 0;
		public long turnMultiplier = 1;
		public long scoreRequired = 10000000; //this is enemy health, rework later to initialize along with custom enemy class health

		public double levelTimes = 1;

		public double pieceWaitTimer = 0;
		public double piecefallTimer = 0;
		public double placeTimer = 0;

		public short leftMoveCooldown = 0;
		public short rightMoveCooldown = 0;
		public short fastfallCooldown = 0;

		public bool canHold = false;

		public boardPiece currentPiece;
		public boardPiece heldPiece = null;
		public boardPiece nextPiece;

		public List<short> updatedRows = new List<short>();
		public List<short> scorableRows = new List<short>();
		public List<block> scoredBlocks = new List<block>();

		public mainOLD(bag bag)
		{
			this.bag = bag;
			encounterStart();
		}

		public void encounterStart()
		{
            animHandler = new animHandler(this);
            board = new board(new Vector2I(12, 28), animHandler);

			heldPiece = null;
			nextPiece = bag.getPiece(board);

			level = 0;
			levelTimes = 1;
			totalScore = 0;
			rowsRequired = 4;
			rowsCleared = 0;
			turnStartState newState = new turnStartState(this);
			newState.startState();
		}

		public void coreGameLoop(GameTime gameTime)
		{
            state.update(gameTime);
        }

		public void coreGameLoopOLD(GameTime gameTime)
		{

			

            //gamesteps:
            //ROUND START is ran from a method instead before coreGameLoop is called

            //TURN START: RUNS ONCE
            //grab new piece and update piece previews

            //PIECE WAIT: LOOPS
            //wait briefly until input or a timer ends to turn to MIDTURN

            //MID TURN: LOOPS
            //process input & move piece
            //update graphics on move
            //move piece down & place properly

            //SCORE STEP: RUNS ONCE
            //process score
            //save scored rows

            //SCORE ANIM: LOOPS
            //runs end of turn animations like score

            //END TURN: RUNS ONCE
            //ticks tiles
            //finalizes score
            //lowers rows
            //ends encounter



            //GAMESTEP REVAMP

            // ====== SCORE PROCESSING method ======
            //tallies up the total value of the scored blocks, counts scored rows, and lowers the board accoardingly
            //requires distinction between scoring individual blocks vs scoring whole rows, since some effects only score a few blocks
            //needs to run score animations for specific blocks and rows scored
            // ====== when does it run? check VVV ======
            //runs after piece placing during score step
            //runs again if any block ticks update the board

            // ====== new endTurn state functionality ======
            //uses the new values from the score processing method
            //all rows scored, whether through the piece or through block events or whatnot are tallied up
            //all level multiplication and such happens here at the very end

			
            switch (stateOld)
			{
				// ================ TURN START ================
				case gameState.turnStart:

					pieceWaitTimer = 0;
					turnRowsCleared = 0;

                    //put this all in one method later, main.drawPiece(); or something, updates all next boxes
                    currentPiece = nextPiece; //grab next piece
					nextPiece = bag.getPiece(board); 
					board.nextbox.update(nextPiece);
					Debug.WriteLine($"[gamestate.turnStart] Now playing {currentPiece.name}");
					//update piece preview

					piecefallTimer = -600; //set timers to negative to give more reaction time when a piece is first placed
					placeTimer = -600;
					stateOld = gameState.pieceWait;
					break;

				// ================ PIECE WAIT ================
				case gameState.pieceWait:

					if (pieceWaitTimer >= 5000 || data.slamKey.keyDown)
					{
						//START piece fall
						currentPiece.play();
						canHold = true;
						stateOld = gameState.midTurn; break;
					}

					pieceWaitTimer += gameTime.ElapsedGameTime.Milliseconds;
					break;

				// ================ MID TURN ================
				case gameState.midTurn:
					//PROCESS INPUT here

					//FALL & PLACE PIECE
					if (piecefallTimer >= 600)
					{

						if (currentPiece.collidesFalling(0, 1))
						{
							if (placeTimer >= 1000)
							{

								currentPiece.place();
								stateOld = gameState.scoreStep;
								break;
							}
						}
						else
						{
							currentPiece.move(0, 1);
							piecefallTimer = 0;
							placeTimer = 0;
							break;
						}
					}
                    parseTurnInput(gameTime);

                    //increment timers
                    piecefallTimer += gameTime.ElapsedGameTime.Milliseconds;
					placeTimer += gameTime.ElapsedGameTime.Milliseconds;
					break;

				// ================ SCORE STEP ================
				case gameState.scoreStep:
                    //check for scorable lines, tally up score, increase level & multiplier


                    // ====== PLACED PIECE SCORE STEP ======
                    foreach (block block in currentPiece.blocks) //grab every tile's Y level
					{
						updatedRows.Add((short)block.boardpos.y);
					}

					updatedRows = updatedRows.Distinct().ToList(); //remove duplicate entries

					foreach (int i in updatedRows)
					{
						if (isRowScoreable(i)) { //check rows, add rows that are scored to the list
							scorableRows.Add((short)i);
							turnRowsCleared += 1;
						} 
					}

					for (int x = 0; x < board.dimensions.x; x++) //process score of every tile in every scored row
					{
						foreach (int y in scorableRows) //process through rows
						{
							block block = board.blocks[x, y];
							if(block != null)
							{
                                turnScore += block.getScore(block);
                                turnMultiplier += block.getTimes(block);

                                block.score(block);
                                scoredBlocks.Add(block);
                            }
						}
					}

                    if (turnRowsCleared > 0)
                    {
                        animHandler.animState = animHandler.scoreAnimStart;
                    } else { animHandler.animState = animHandler.highlightPlacedPiece; }

                    stateOld = gameState.scoreAnim;
					break;

				case gameState.scoreAnim:

					if(animHandler.animState != animHandler.none)
					{
						animHandler.animState.Invoke(gameTime);
						break;
					}

                    stateOld = gameState.tickStep;

					break;

				case gameState.tickStep:
					


					

					break;

				case gameState.endTurn:
					
					if(turnRowsCleared > 0)
					{
						turnScore += (turnRowsCleared - 1) * 10;
						turnScore *= turnMultiplier;

						if(turnRowsCleared == 3)
						{
							turnScore = (long)(turnScore * (levelTimes / 4));
						}
						else if(turnRowsCleared >= 4)
						{
							turnScore = (long)(turnScore * levelTimes);
						}
						recalculateLevel(turnRowsCleared);
					}

                    totalScore += turnScore;
                    turnScore = 0;
					turnRowsCleared = 0;

                    updatedRows.Clear();
                    scorableRows.Clear();

                    Debug.WriteLine($"Current score: {totalScore}");
                    if (totalScore >= scoreRequired) //if the player has enough score to beat the encounter, end the encounter
                    {
                        Debug.WriteLine("ROUND ENDED!!!!!");
                        //end encounter
                    }


                    /*short scoredRowCount = (short)scorableRows.Count;

                    foreach (block block in scoredBlocks)
                    {
                        block.removePlaced(block);
                    }
                    scoredBlocks.Clear();

                    board.lowerRows(scorableRows); //lower rows

                    foreach (block block in board.blocks)
					{ //tick every non null tile
						block?.tick.Invoke(block);
					}

					//final score operation
					if (scoredRowCount > 0) //get level multiplier and row bonus
					{
						turnScore += (scoredRowCount - 1) * 10; //increase by 10 for every extra row scored
						turnScore *= turnMultiplier;

						if (scoredRowCount == 3)
						{
							turnScore = (long)(turnScore * (levelTimes / 4));
						}
						else if (scoredRowCount >= 4)
						{
							turnScore = (long)(turnScore * levelTimes);
						}
						recalculateLevel(scoredRowCount);
					}


					totalScore += turnScore;
					turnScore = 0;

					updatedRows.Clear();
					scorableRows.Clear();

					Debug.WriteLine($"Current score: {totalScore}");
					if (totalScore >= scoreRequired) //if the player has enough score to beat the encounter, end the encounter
					{
						Debug.WriteLine("ROUND ENDED!!!!!");
						//end encounter
					}

                    stateOld = gameState.turnStart;

					break;
			}

		}

		public void processUpdatedRows()
		{

		}

		public void recalculateLevel(short rows)
		{
			rowsCleared += rows;
			while (rowsCleared >= rowsRequired) //loop the level check incase the player levels up multiple times at once
			{
				level += 1;
				rowsCleared -= rowsRequired;
				rowsRequired += 2;
			}
			Debug.WriteLine("level: " + level);
			levelTimes = level / 2d + 1d;
		}

	

		public enum gameState
		{
			turnStart, //once at the start of the turn
			pieceWait, //waiting briefly before dropping piece
			midTurn, //handling piece falling and collision
			scoreStep, //processing score, lowering rows
			scoreAnim, //running score animations
			tickStep,
			endTurn //ticking blocks, running scoreStep again if the board is updated, finalizing score, & ending encounter
		}*/
	}
}
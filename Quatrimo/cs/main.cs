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
	public class main
	{ //IDEA: eventlistener base class for our custom objects so they can be processed through by Events
	  //REWORK the gamestate steps, endState should be split INTO TWO: the first one processes everything and stores it, then the second one animates it and finalizes it

		//this class and board needs a complete rewrite

		public static gameState state;

		public board board;

		public bag bag;

		//board variables
		public int level = 0;
		public int rowsRequired = 4;
		public int rowsCleared = 0;
		public int enemyCooldown;

		public long turnScore = 0;
		public long totalScore = 0;
		public long turnMultiplier = 1;
		public long scoreRequired = 10000000; //this is enemy health, rework later to initialize along with custom enemy class health

		public double levelTimes = 1;

		public double pieceWaitTimer = 0;
		public double piecefallTimer = 0;
		public double placeTimer = 0;
		public double moveCooldown = 1;
		public double fastfallTimer = 0;
		public double scoreAnimationTimer = 400;

		public bool canHold = false;
		public bool fastfallReset = false;

		public boardPiece currentPiece;
		public boardPiece heldPiece = null;
		public boardPiece nextPiece;

		public List<int> updatedRows = new List<int>();
		public List<int> scorableRows = new List<int>();
		public List<block> scoredBlocks = new List<block>();

		public main(bag bag)
		{
			this.bag = bag;
			encounterStart();
		}

		public void encounterStart()
		{
			board = new board(new Vector2I(12, 28));

			heldPiece = null;
			nextPiece = bag.getPiece(board);

			level = 0;
			levelTimes = 1;
			totalScore = 0;
			rowsRequired = 4;
			rowsCleared = 0;
			state = gameState.turnStart;
		}

		public void coreGameLoop(GameTime gameTime)
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

			parseInput();

			switch (state)
			{ //will need graphics update code

				// ================ TURN START ================
				case gameState.turnStart:

					pieceWaitTimer = 0;

					currentPiece = nextPiece; //grab next piece
					nextPiece = bag.getPiece(board);
					board.nextbox.update(nextPiece);
					Debug.WriteLine($"[gamestate.turnStart] Now playing {currentPiece.name}");
					//update piece preview

					piecefallTimer = -600; //set timers to negative to give more reaction time when a piece is first placed
					placeTimer = -600;
					state = gameState.pieceWait;
					break;

				// ================ PIECE WAIT ================
				case gameState.pieceWait:

					if (pieceWaitTimer >= 5000 || data.slamKey.keyDown)
					{
						//START piece fall
						currentPiece.play();
						canHold = true;
						state = gameState.midTurn; break;
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
								state = gameState.scoreStep;
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

					foreach (block block in currentPiece.blocks) //grab every tile's Y level
					{
						updatedRows.Add(block.boardpos.y);
					}

					updatedRows = updatedRows.Distinct().ToList(); //remove duplicate entries

					foreach (int i in updatedRows)
					{
						if (isRowScoreable(i)) { scorableRows.Add(i); } //check rows, add rows that are full to the list
					}


					for (int x = 0; x < board.dimensions.x; x++) //process score of every tile in every scored row
					{
						foreach (int y in scorableRows) //process through rows
						{
							block block = board.blocks[x, y];
							turnScore += block.getScore(block);
							turnMultiplier += block.getTimes(block);

							block.score(block);
							scoredBlocks.Add(block);
						}
					}


					state = gameState.scoreAnim;
					break;

				case gameState.scoreAnim:
	
					state = gameState.endTurn;
					
					//lol
					//add animations later
					break;

				case gameState.endTurn:
					
					byte scoredRowCount = (byte)scorableRows.Count;

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

					state = gameState.turnStart;

					break;
			}

		}


		public void recalculateLevel(int rows)
		{
			rowsCleared += rows;
			while (rowsCleared >= rowsRequired)
			{
				level += 1;
				rowsCleared -= rowsRequired;
				rowsRequired += 2;
			}
			Debug.WriteLine("level: " + level);
			levelTimes = level / 2d + 1d;
		}

		public void parseTurnInput(GameTime gameTime)
		{
			//for movement keys, when key holds: do action once, wait until timeheld, then move often
			if ((data.leftKey.keyDown || data.leftKey.timeHeld > 120) && moveCooldown > 20)
			{
				if (!currentPiece.collidesFalling(-1, 0))
				{
					currentPiece.move(-1, 0);
					moveCooldown = 0;
				}
			}
			else if ((data.rightKey.keyDown || data.rightKey.timeHeld > 120) && moveCooldown > 20)
			{
				if (!currentPiece.collidesFalling(1, 0))
				{
					currentPiece.move(1, 0);
					moveCooldown = 0;
				}
			}

			if (data.downKey.timeHeld > 16 )
			{
				piecefallTimer += 600;
				fastfallReset = true;
				data.downKey.timeHeld = 0; //might cause issues - use a seperate timer instead if so
			}
			else if(data.downKey.keyUp && fastfallReset)
			{
				piecefallTimer = 0;
				placeTimer = 0;
			}
			
			
			if (data.upKey.keyHeld)
			{
				//piecefallTimer -= gameTime.ElapsedGameTime.TotalMilliseconds * 0.2;
			}

			if (data.leftRotateKey.keyDown)
			{
				if (currentPiece.canRotate(-1))
				{
					currentPiece.rotate(-1);
				}
			}
			else if (data.rightRotateKey.keyDown)
			{
				if (currentPiece.canRotate(1))
				{
					currentPiece.rotate(1);
				}
			}

			if (data.slamKey.keyDown)
			{
				currentPiece.move(0, currentPiece.dropOffset);
				currentPiece.place();
				state = gameState.scoreStep;
			}

			if (data.holdKey.keyDown && canHold)
			{
				holdPiece();
			}

			moveCooldown += gameTime.ElapsedGameTime.TotalMilliseconds;
		}

		public void parseInput()
		{
			if (data.restartKey.keyDown)
			{
				encounterStart();
			}
		}

		public bool isRowScoreable(int y)
		{
			for (int x = 0; x < board.dimensions.x; x++)
			{
				if (board.blocks[x, y] == null) return false; //if any tile is empty, return false
				else { continue; } //if the tile isn't empty, keep looping
			}
			return true; //if no tiles return empty, this will run and return true
		}

		public void holdPiece()
		{
			if (heldPiece == null)
			{
				heldPiece = nextPiece;
				nextPiece = bag.getPiece(board);
			}

			boardPiece formerlyHeld = heldPiece;

			heldPiece = currentPiece;
			while (heldPiece.rotation != 0)
			{
				heldPiece.rotate(1);
			}

			board.holdbox.update(heldPiece);

			currentPiece.removeFalling();
			currentPiece = formerlyHeld;
			currentPiece.play();
			canHold = false;
		}

		public enum gameState
		{
			turnStart, //once at the start of the turn
			pieceWait, //waiting briefly before dropping piece
			midTurn, //handling piece falling and collision
			scoreStep, //processing score
			scoreAnim, //running score animations
			endTurn //finalizing score, lowering rows & ending encounter
		}
	}
}
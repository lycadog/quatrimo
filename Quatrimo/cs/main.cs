using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

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
	public int rowsCleared = 0;
	public int enemyCooldown;

	public long turnScore = 0;
    public long totalScore = 0;
    public long turnMultiplier = 1;
    public long scoreRequired = 10000000; //this is enemy health, rework later to initialize along with custom enemy class health

	public double levelTimes = 1;
	public double piecefallTimer = 0;
	public double inputCooldownTimer = 1;
	public double scoreAnimationTimer = 0;

	public bool canHold = false;

	public boardPiece currentPiece;
	public boardPiece heldPiece = null;
	public boardPiece nextPiece;

	public List<int> updatedRows = new List<int>();
	public List<int> scorableRows = new List<int>();

    public void coreGameLoop(double deltaTime)
    {
		//COMPLETELY rewrite core game loop

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
		//lowers rows
		//ticks tiles
		//ends encounter

		switch (state)
		{
			case gameState.turnStart:

				currentPiece = nextPiece; //grab next piece
				nextPiece = bag.getPiece(board);
				Debug.WriteLine($"playing {currentPiece.name}");
				//update piece preview
				state = gameState.pieceWait;
				break;

			case gameState.pieceWait:
				
				break;
		}






		//DEPRECATED
			switch (state)
		{
            // ========== ROUND START ==========
            case gameState.roundStart: //run once when entering a battle (round)!

                initializeRound();
				state = gameState.turnStart;
                break;

            // ========== TURN START ==========
            case gameState.turnStart: //run once at the start of a turn

                currentPiece = nextPiece;
				nextPiece = bag.getPiece(board);
				Debug.WriteLine(currentPiece.name);
				board.updatePiecePreview(currentPiece, nextPiece);
				state = gameState.pieceWait;
                break;

            // ========== PIECE WAIT ==========
            case gameState.pieceWait: //run until input

                /*if (Input.IsActionJustPressed("pieceDrop")) //change this to a configurable 5 second timer that can be ended early with an input eventually
                {   //initiate piecefall
                    currentPiece.playPiece();
					canHold = true;
                    state = gameState.midTurn;
                }*/
				break;

            // ========== MID TURN ==========
            case gameState.midTurn: //run continuously during a turn, while a piece is falling (ie piecefall!)

                //add input code to move falling piece

                piecefallTimer += deltaTime; //increment timer
				inputCooldownTimer += deltaTime;

                //rework the input later to be a bit more natural (like with pressing vs holding move keys
                if (inputCooldownTimer >= 0.1)
                {
					parseTurnInput(deltaTime);
				}

                if (piecefallTimer >= 0.6)
					{
                        if (currentPiece.fallPiece())//check for collision
                        {
                        Debug.WriteLine("piece placed!");
                            state = gameState.endTurnStart;
                        }
                        
						Debug.WriteLine(currentPiece.pos.x + ", " + currentPiece.pos.y);
						piecefallTimer = 0;
                    }
				

                break;

				// ========== END TURN START ==========
			case gameState.endTurnStart: //process everything once

                

                currentPiece.placePiece();
                Debug.WriteLine("piece placed at " + currentPiece.pos);
                

                foreach (tile tile in currentPiece.tiles) //when a piece is placed, add each row it intersects to updatedRows
				{
					if(tile != null) { updatedRows.Add(tile.boardPos.Y); }
				}
				updatedRows = updatedRows.Distinct().ToList(); //remove duplicate rows

				foreach(int row in updatedRows)	{ //checks each row that was just updated, if any are scorable they stored in scorableRows
					if (isRowScoreable(row)){ //process through each scored row and set them to be scored
						scorableRows.Add(row);
					}
				}
				scorableRows.Sort();

				for(int x = 0; x < board.dimensions.X; x++) //process top to bottom, left to right through every scorable line
				{
					foreach(int y in scorableRows)
					{
						tile tile = board.tiles[x,y];
						turnScore += tile.score(turnScore);
						turnMultiplier += tile.type.getMultiplier(tile);
						board.markBoardStale(tile.boardPos, 1);
						animatable anim = new animatable(new List<string> { "█", "▓", "▒", "░", " " }, false, tile.boardPos, 0.05, board);
						board.animatables.Add(anim);
					}
				}
				board.updateBoard();

                state = gameState.endTurnFinish;
                break;

				// ========== END TURN FINISH ==========
			case gameState.endTurnFinish: //finalize everything
				if(scoreAnimationTimer >= 0.25 | scorableRows.Count == 0) //THIS SHOULD BE CHANGED LATER, waits for animation to finish then runs end turn
				{ //finalize everything once animations are over
                    scoreAnimationTimer = 0;


                    board.lowerRows(scorableRows);

                    foreach (tile tile in board.tiles)
                    { //tick every tile
                        if (tile != null)
                        { tile.tick(); }
                    }

                    if (scorableRows.Count > 0)
                    {
                        turnScore += (scorableRows.Count - 1) * 10; //increase by 10 for every extra row scored
                        turnScore *= turnMultiplier;

                        if (scorableRows.Count == 3)
                        {
                            turnScore = (long)(turnScore * (levelTimes / 4));
                        }
                        else if (scorableRows.Count >= 4)
                        {
                            turnScore = (long)(turnScore * levelTimes);
                        }
                        recalculateLevel(scorableRows.Count);

                    }

                    totalScore += turnScore;
                    turnScore = 0;

                    updatedRows.Clear();
                    scorableRows.Clear();

                    board.updateBoard();

                    board.updateScore(totalScore);
                    Debug.WriteLine($"Current score: {totalScore}");
                    if (totalScore >= scoreRequired) //if the player has enough score to beat the encounter, end the encounter
                    {
                        Debug.WriteLine("ROUND ENDED!!!!!");
                        state = gameState.endRound; break;
                    }

                    //run enemy behavior like lowering enemy cooldown and playing enemy lines



                    state = gameState.turnStart; break;
                }
				scoreAnimationTimer += deltaTime;
                break;

            case gameState.endRound:

				break;

			default:
				break;
		}

		parseGlobalInput();
		board.animationTick(deltaTime);
		board.updateGraphics();

    }


	public void startEncounter(bag bag) //add code to initialize board graphics properly
	{ //add code for enemy eventually
		board = new board(new Vector2I(12, 22));
		this.bag = bag;
		
		heldPiece = null;
		nextPiece = bag.getPiece(board);

		level = 0;
		totalScore = 0;
		state = gameState.turnStart;

    }

    public void recalculateLevel(int rows)
	{
		rowsCleared += rows;
		level = rowsCleared / 10;
		Debug.WriteLine("level: " + level);
		levelTimes = level / 2d + 1d;
		//board.updateLevelUI(level, levelTimes);
	}

	//DEPRECATED - REWRITE all input code
	public void parseTurnInput(double deltaTime) //runs during piecefall, checks input to move piece, determines move validity and executes move
	{
		bool isMoveValid = false;
		/*if (Input.IsActionPressed("boardLeft"))
		{
			if (currentPiece.isMoveValid(-1))
			{
				currentPiece.moveFallingPiece(-1, 0);
				isMoveValid = true;
			}

		}
		else if (Input.IsActionPressed("boardRight"))
		{
			if (currentPiece.isMoveValid(1))
			{
				currentPiece.moveFallingPiece(1, 0);
				isMoveValid = true;
			}
		}
		else if (Input.IsActionJustPressed("boardRotateLeft"))
		{
			if(isMoveValid = currentPiece.isRotationValid(1))
			{
				currentPiece.rotatePiece(1);
			}
        }
        else if (Input.IsActionJustPressed("boardRotateRight"))
        {
            if (isMoveValid = currentPiece.isRotationValid(-1))
            {
                currentPiece.rotatePiece(-1);
            }
        }
		else if (Input.IsActionJustPressed("holdPiece") && canHold)
		{
			holdPiece();
			isMoveValid = true;
		}
		else if (Input.IsActionPressed("boardUp"))
		{
			piecefallTimer -= deltaTime * 0.5;
		}
        else if (Input.IsActionPressed("boardDown"))
		{
			piecefallTimer += deltaTime * 5;
		}
		else if (Input.IsActionJustPressed("boardSlam"))
		{
			bool isSlamming = true;
			while (isSlamming)
			{
				if (currentPiece.fallPiece())
				{
					isSlamming = false;
					state = gameState.endTurnStart;
					break;
				}
			}
		}*/
		
		if (isMoveValid)
		{
			
            piecefallTimer = 0;
            inputCooldownTimer = 0;
        }
    }

	public void parseGlobalInput()
	{
        /*if (Input.IsActionJustPressed("restart"))
        {
            board.clearGraphics();
            board.resetUI();
			resetVariables();
            state = gameState.roundStart;
        }*/
    }

	public void resetVariables()
	{
		totalScore = 0;
		level = 0;
		levelTimes = 1;
	}
	
	public bool isRowScoreable(int y)
	{
		for(int x = 0; x < board.dimensions.x; x++)
		{
			if (board.tiles[x, y] == null) return false; //if any tile is empty, return false
			else { continue; } //if the tile isn't empty, keep looping
		}
		return true; //if no tiles return empty, this will run and return true
    }

	public void holdPiece()
	{
		//needs to be remade
		if(heldPiece == null)
		{
			heldPiece = nextPiece;
			nextPiece = bag.getPiece(board);
		}
		boardPiece temp = heldPiece;

		temp.pos = new Vector2I(5, 5);

		heldPiece = currentPiece;
        while (heldPiece.rotation != 0)
        {
            heldPiece.rotatePiece(1);
        }
		
        currentPiece = temp;
		canHold = false;
		//board.updatePiecePreview(currentPiece, nextPiece);
		//board.updateHeldUI(heldPiece);

		Debug.WriteLine(currentPiece.pos);
		currentPiece.playPiece();
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

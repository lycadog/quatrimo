using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

	public double pieceWaitTimer = 0;
	public double piecefallTimer = 0;
	public double placeTimer = 0;
	public double inputCooldown = 1;
	public double scoreAnimationTimer = 0;

	public bool canHold = false;

	public boardPiece currentPiece;
	public boardPiece heldPiece = null;
	public boardPiece nextPiece;

	public List<int> updatedRows = new List<int>();
	public List<int> scorableRows = new List<int>();

    public main(bag bag)
    {
        this.bag = bag;

        board = new board(new Vector2I(12, 22));

        heldPiece = null;
        nextPiece = bag.getPiece(board);

        level = 0;
        totalScore = 0;
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

        switch (state)
		{ //will need graphics update code

            // ================ TURN START ================
            case gameState.turnStart:

                pieceWaitTimer = 0;

                currentPiece = nextPiece; //grab next piece
				nextPiece = bag.getPiece(board);
				Debug.WriteLine($"playing {currentPiece.name}");
				//update piece preview

				piecefallTimer = -600; //set timers to negative to give more reaction time when a piece is first placed
				placeTimer = -600;

				state = gameState.pieceWait;
				break;

            // ================ PIECE WAIT ================
            case gameState.pieceWait:

				if(pieceWaitTimer >= 5000 || data.slamKey.keyDown)
				{
					//START piece fall
					currentPiece.playPiece();
					canHold = true;
					state = gameState.midTurn; break;
				}

				pieceWaitTimer += gameTime.ElapsedGameTime.Milliseconds;
				break;

            // ================ MID TURN ================
            case gameState.midTurn:
				//PROCESS INPUT here

				parseInput(gameTime);
				currentPiece.renderFalling();
				Debug.WriteLine("========= POSITION: " + currentPiece.pos.x + currentPiece.pos.y);
                //FALL & PLACE PIECE
                if (piecefallTimer >= 400)
				{

                    if (currentPiece.shouldPlace()){
                        if (placeTimer >= 1000){

                            currentPiece.placePiece();
							state = gameState.scoreStep;

                        }}

                    else{
                        currentPiece.moveFallingPiece(0, 1);
						piecefallTimer = 0;
						placeTimer = 0;
                    }
                }

				//increment timers
				piecefallTimer += gameTime.ElapsedGameTime.Milliseconds;
				placeTimer += gameTime.ElapsedGameTime.Milliseconds;
				break;

            // ================ SCORE STEP ================
            case gameState.scoreStep:
				//check for scorable lines, tally up score, increase level & multiplier

				foreach(tile tile in currentPiece.tiles) //grab every tile's Y level
				{
					updatedRows.Add(tile.boardPos.y);
				}
				
				updatedRows = updatedRows.Distinct().ToList(); //remove duplicate entries

				foreach(int i in updatedRows)
				{
					if (isRowScoreable(i)) { scorableRows.Add(i); } //check rows, add rows that are full to the list
				}


				for(int x = 0; x < board.boardDim.x; x++) //process score of every tile in every scored row
				{
					foreach(int y in scorableRows) //process through rows
					{
						tile tile = board.tiles[x, y];
						turnScore += tile.score();
						turnMultiplier += tile.type.getMultiplier(tile);
					}
				}

                

                state = gameState.scoreAnim;
                break;

			case gameState.scoreAnim:
				//lol
				//add animations later
				state = gameState.endTurn; 
				break;

			case gameState.endTurn:

				foreach(tile tile in board.tiles){ //tick every tile
					if(tile != null){
						tile.tick();}} //going to need to rework ticking tiles a bit to handling tick events that add score



				//final score operation
                if (scorableRows.Count > 0) //get level multiplier and row bonus
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

                board.lowerRows(scorableRows); //lower rows


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

	public void parseInput(GameTime gameTime)
	{
		//rework inputs to add "on key down" kinda stuff
		//movement should work 

		
        if (data.leftKey.keyHeld && inputCooldown > 100)
        {
			if (currentPiece.isMoveValid(-1))
			{
				currentPiece.moveFallingPiece(-1, 0);
				inputCooldown = 0;
			}
        }
		else if (data.rightKey.keyHeld && inputCooldown > 100)
		{
			if (currentPiece.isMoveValid(1))
			{
				currentPiece.moveFallingPiece(1, 0);
                inputCooldown = 0;
            }
        }

		if(data.downKey.keyHeld)
		{
			piecefallTimer += gameTime.ElapsedGameTime.TotalMilliseconds * 4;
		}
		else if (data.upKey.keyHeld)
		{
			piecefallTimer -= gameTime.ElapsedGameTime.TotalMilliseconds * 0.8 ;
		}

		if (data.leftRotateKey.keyDown)
		{
            if (currentPiece.isRotationValid(1))
            {
                currentPiece.rotatePiece(1);
            }
        }
		else if (data.rightRotateKey.keyDown)
		{
            if (currentPiece.isRotationValid(-1))
            {
                currentPiece.rotatePiece(-1);
            }
        }
		inputCooldown += gameTime.ElapsedGameTime.TotalMilliseconds;
    }



	//DEPRECATED - REWRITE all input code
	public void parseInputOLD(double deltaTime) //runs during piecefall, checks input to move piece, determines move validity and executes move
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
            inputCooldown = 0;
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
		for(int x = 0; x < board.boardDim.x; x++)
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

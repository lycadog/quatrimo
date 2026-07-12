using Godot;
using System;
using System.Collections.Generic;

public partial class Board : Control
{
    Cell[,] CellBoard;
    public BoardRow[] BoardRows;

    public Cell this[int x, int y]
    {
        get => CellBoard[x, y];
        private set => CellBoard[x, y] = value;
    }

    public Vector2I VisualDimensions;
    public Vector2I CellDimensions;

    public Enemy Enemy;

    public double CurrentScore;
    public double LevelMult;

    //todo: make these properties

    int Level;


    double MultPerLevel;
    int RowsNeededForLevelup;
    int RowsUntilLevelup;

    int TotalRowsScored = 0;
    int TurnRowsScored = 0;

    #region == Properties ==

    bool _Board_HasUnprocessedUpdates;
    public bool Board_HasUnprocessedUpdates
    {
        get => _Board_HasUnprocessedUpdates;
        set
        {
            _Board_HasUnprocessedUpdates = value;
            if (value) { EmitSignalBoardUpdated(); }
        }
    }

    #endregion
    #region == Export Fields ==
    [Export] AudioStreamPlayer ScoringSfx;
    [Export] AudioStreamPlayer PlacementSfx;

    [Export] public Node2D BlockBox;
    [Export] NinePatchRect border;
    [Export] GradientTexture2D darkGradient;

    [Export] PlayerHand Hand;
    [Export] RowsClearedDial RowsClearedDial;
    [Export] ScoreBox ScoreBox;
    [Export] LevelStatBoxes LevelStatBoxes;
    [Export] EnemyHealthBar EnemyHealthBar;

    [Export] Area2D BottomBorder;

    [Export] int defaultX = 12, defaultY = 20;
    #endregion
    #region == Signals ==
    [Signal] public delegate void BoardUpdatedEventHandler();

    [Signal] public delegate void PlayerTurn_StartedEventHandler();
    [Signal] public delegate void Piece_PlayedEventHandler();
    [Signal] public delegate void Piece_PlacedEventHandler();

    /// <summary>
    /// Ran each time the Board Processing loops again, before we actually process the board.
    /// </summary>
    [Signal] public delegate void BoardProcessingLoop_StartedEventHandler();

    /// <summary>
    /// Use this to tick all the blocks. Blocks are bound to this when placed
    /// </summary>
    /// <param name="isPriorityTick"></param>
    [Signal] public delegate void TickBlocksEventHandler(bool isPriorityTick);
    [Signal] public delegate void TurnEnded_EventHandler();
    #endregion

    static PackedScene ScoreAnimation = ResourceLoader.Load<PackedScene>("uid://joftg3j7lslu");

    #region == Constants ==

    const double LowerAnimLength = .3;
    const double BaseScoreTickdownTime = 0.3;
    const double WaitTimeAfterEnemyDies = 0.6;
    //todo: maybe change this so the wait time is based on the enemy?

    #endregion



    List<Block> PlacedBlocks = [];

    FallingPiece CurrentPiece;


    #region === Board Logic ===

    void StartPlayerTurn()
    {
        Board_HasUnprocessedUpdates = false;

        //draw hand and stuff
        Hand.OnTurnStart();
        EmitSignalPlayerTurn_Started();
        //handle playerhand things
    }

    void PlayerPlaysCard(PieceCard Piece)
    {
        //start piecefall
        EmitSignalPiece_Played();
    }

    void PlaceBlock(Block block)
    {

    }

    void Piecefall_Finished()
    {
        EmitSignalPiece_Placed();
        StartBoardProcessing(TickPriorityBlocks);
        //process score moving onto block ticking after
    }

    /// Break logic here to score ///

    void TickPriorityBlocks()
    {
        EmitSignalTickBlocks(true);

        if (Board_HasUnprocessedUpdates)
        {
            StartBoardProcessing(TickNormalBlocks);
            return;
        }

        TickNormalBlocks();
        //WE MISSED a key part of this state in the diagram! we have to run this logic TWICE, the first time being for destructive ticks
        //WAIT this may need to be a part of score processing
        //actually probably not. if stuff gets placed by an enemy it doesn't need to be ticked immediately
    }

    void TickNormalBlocks()
    {

        //goto score processing after this !!!
    }


    void ResetTurnValues()
    {
        CurrentScore = 0;

        //start reset animations
        //check here if the enemy's attacking! if so, we need to create an animation state, and wait before starting the enemy's turn!
    }

    void StartEnemyTurn()
    {
        //we need to wait for reset animations ONLY IF the enemy is going to attack!!!
    }

    void FinishEnemyTurn()
    {
        //finish the enemy's turn, updating their cooldowns and then moving onto the next turn!
    }

    #endregion
    #region === Board Processing ===

    Action ProcessingReturnMethod;
    List<Cell> ScoredCells = [];
    List<BoardRow> ScorableRows = [];

    //we need to implement animations for rows and numbers and such
    //and superstate animations for these, waiting on them before progressing past board processing

    /// <summary>
    /// Start processing the board for scorable rows. Returns to specified method after processing is done!
    /// </summary>
    /// <param name="ReturnMethod"></param>
    void StartBoardProcessing(Action ReturnMethod)
    {
        if(ProcessingReturnMethod != null)
        {
            GD.PushError("Attempted to start board processing while board processing is already happening!");
            return;
        }

        ProcessingReturnMethod = ReturnMethod;
        ScorableRows.Clear();
        ScoredCells.Clear();

        CheckScorability();
    }

    void CheckScorability()
    {
        ScorableRows.Clear();
        EmitSignalBoardProcessingLoop_Started();

        //scan all rows for scorability, adding them to a list
        foreach (BoardRow row in BoardRows)
        {
            if (row.Scorable)
            {
                ScorableRows.Add(row);
            }
        }

        //if stuff scored: score it!
        if (ScorableRows.Count > 0)
        {
            //scoring happened so we gotta start it and handle it now
            foreach (var row in ScorableRows)
            {
                row.StartScoring();
            }
            RowsClearedDial.AddRows(ScorableRows.Count);

            //rows cleared dial will run events to animate everything related to rows
            //these will create superstate animations as well when they start

            RowsUntilLevelup -= ScorableRows.Count;

            TurnRowsScored += ScorableRows.Count;
            TotalRowsScored += ScorableRows.Count;

            while(RowsUntilLevelup <= 0)
            {
                //level up until we no longer have enough to levelup
                //we use a while loop so we can level up multiple times at once
                LevelUp();

            }

            //TODO PLAY SOUNDS HERE
            PlayScoringSounds(ScorableRows.Count);
            //we gotta start our animation waiting here, then progressing to LoweringBlocks after !!!

            StartStateAnimation(LowerScoredBlocks);
            //we need to update all rows-related values right here!

        }
        //NO SCORING LETS LEAVE!!!!!
        else
        {
            //WE NEED TO WAIT!!! on ALL superstate animations before exiting!!!!
            //no scoring happened! let's skip all this noise

            StartSuperStateAnimation(ExitBoardProcessing);
            //after all animations have fully finished, we can exit board processing!
        }
    }

    void LevelUp()
    {
        RowsUntilLevelup += RowsNeededForLevelup;
        Level++;
        LevelMult += MultPerLevel;
    }

    void PlayScoringSounds(int RowsScored)
    {
        //play sound depending on how many rows scored !!!
    }

    void Block_Scored(Block block)
    {
        //create score number here!!!
        SuperStateAnimationsCount++;

        var newNumber = MiniScoreNumber.GetNew(block.ScoreValue, ScoreBox.ScoreLabel.GetGlobalRect().GetCenter());
        newNumber.NumberReachedScore += AddScoreNumberToScore;
        BlockBox.AddChild(newNumber);
        newNumber.Position = block.Position;

        if(block.MultValue != 0)
        {
            //TODO add mult number!!!!
        }
    }


    void AddScoreNumberToScore(double score)
    {
        CurrentScore += score;
        ScoreBox.SetScore(CurrentScore);

        //score numbers are superstate animations
        SuperStateAnimationsFinished++;
    }

    void Cell_Scored(Cell cell)
    {
        //add scoring animation here!!!

        StateAnimationsCount++;

        ScoreAnimation animation = (ScoreAnimation)ScoreAnimation.Instantiate();
        BlockBox.AddChild(animation);

        animation.Position = cell.RealPosition;
        animation.AnimationFinished += () => StateAnimationsFinished++;

        if (cell.FilledInOnScoring)
        {
            ScoredCells.Add(cell);
        }
    }

    void LowerScoredBlocks()
    {
        if(ScoredCells.Count == 0)
        {
            //nothing to lower! end lowering early
            CheckScorability();
            return;
        }

        int LowestScoredY = 1000; //initialize this to be big

        foreach (var cell in ScoredCells)
        {
            //iterate upwards in the array, setting lower values for every cell
            LowestScoredY = Math.Min(cell.y, LowestScoredY); //get the lowest value of any scored cell

            for (int y = cell.y + 1; y < CellDimensions.Y - 1; y++)
            {
                CellBoard[cell.x, y].LowerDistance++;
            }

            cell.DeleteBlock();
        }
        ScoredCells.Clear();

        if(PlacedBlocks.Count == 0)
        {
            //abort lowering since there's nothing to lower!
            CheckScorability();
            return;
        }

        Tween tween = GetTree().CreateTween();
        foreach (var block in PlacedBlocks)
        {
            StateAnimationsCount++;
            LowerBlock(block, LowestScoredY, tween);
        }

        //loop back to checking scorability after lowering
        StartStateAnimation(CheckScorability);
    }

    void LowerBlock(Block block, int lowestScoredY, Tween tween)
    {
        block.BoardPos = new(block.BoardPos.X, block.BoardPos.Y - block.LowerDistance);

        //we don't want the delay to be too long when we have a really tall board, so if it's too tall we decrease it by a bit
        double decreaseDelayPerBlockAtHighValues = (block.BoardPos.Y - lowestScoredY) * .0002;
        double loweringDelay = (block.BoardPos.Y - lowestScoredY) * (.03 - decreaseDelayPerBlockAtHighValues); //* AnimationTimescale;

        StateAnimationsCount++;
        //tween lowering to our new position, and we actually move when its done with the method
        tween.TweenProperty(block, "position",
            new Vector2(0, block.LowerDistance * 10), LowerAnimLength).AsRelative()
            .SetTrans(Tween.TransitionType.Bounce)
            .SetDelay(loweringDelay).SetEase(Tween.EaseType.Out)
            .Finished += () => Block_FinishedLowering(block);

    }

    void Block_FinishedLowering(Block block)
    {
        CellBoard[block.BoardPos.X, block.BoardPos.Y].HeldBlock = block;
        block.LowerDistance = 0;

        StateAnimationsFinished++;
    }

    void ExitBoardProcessing()
    {
        Board_HasUnprocessedUpdates = false;

        ProcessingReturnMethod.Invoke();
        ProcessingReturnMethod = null;
    }





    #endregion
    #region === Score Processing ===

    void TallyUpScore()
    {
        if (CurrentScore == 0)
        {
            //end early
            //WE MAY not need this!!!1
            ResetTurnValues();
            return;
        }

        bool IsMultiplied = false;

        if (TurnRowsScored >= Run.Current.RowsNeededForMultiplier)
        {
            //we multiply!
            IsMultiplied = true;
            CurrentScore *= LevelMult;
        }
        //fully process score and multiply it and stuff and then subtract it from enemy health

        //score box will start tallying up everything.
        //after a bit it will fire back to us, so we can handle damaging the enemy and resetting the score value!
        ScoreBox.ProcessScore(CurrentScore, IsMultiplied);
    }

    void SubtractScoreFromEnemyHP()
    {
        //subtract health, then animate it!
        Enemy.Health -= CurrentScore;

        //we want to take slightly longer to animate based on score, but this shouldn't be mind-numbingly slow at high values
        //so we make it reverse exponential growth
        double ExponentialTimeFactorFromScore = Math.Max(Math.Pow(CurrentScore, 0.2) * 0.05, 0);
        double TickdownAnimationTime = BaseScoreTickdownTime + ExponentialTimeFactorFromScore;

        ScoreBox.TickDownScore(TickdownAnimationTime);
        EnemyHealthBar.DealDamage(CurrentScore, TickdownAnimationTime);

        //start animations, then wait until a bit after them to check enemy HP!

        GetTree().CreateTween()
            .TweenCallback(Callable.From(CheckEnemyHP))
            .SetDelay(TickdownAnimationTime + WaitTimeAfterEnemyDies);
    }

    void CheckEnemyHP()
    {
        if (Enemy.Health <= 0)
        {
            //if enemy is below hp, END encounter!!!
            return;
        }

        //enemy alive, let's continue!
        ResetTurnValues();
    }




    #endregion
    #region === Animations ===

    #region State Animations

    /// <summary>
    /// Start a state animation, specifying the method to call after it ends
    /// </summary>
    /// <param name="action"></param>
    void StartStateAnimation(Action action)
    {
        StateAnimationReturnMethod = action;
        StateAnimationsInProgress = true;
        CheckStateAnimations();
    }

    //Where to return to after all StateAnimations have finished
    Action StateAnimationReturnMethod;
    bool StateAnimationsInProgress;

    //Brief animations for a certain state in board logic, like scoring or lowering
    int _StateAnimationsFinished;
    int StateAnimationsFinished
    {
        get => _StateAnimationsFinished;
        set 
        { 
            _StateAnimationsFinished = value;
            CheckStateAnimations();
        }
    }

    int StateAnimationsCount;

    void CheckStateAnimations()
    {
        //if we're animating AND have finished all animations: progress!
        if(StateAnimationsInProgress && StateAnimationsFinished == StateAnimationsCount)
        {
            StateAnimationsCount = 0;
            StateAnimationsFinished = 0;

            StateAnimationReturnMethod.Invoke();
            StateAnimationsInProgress = false;
        }
        else if(StateAnimationsFinished > StateAnimationsCount)
        {
            GD.PushError($"Too many state animations finished! Completed {StateAnimationsFinished} out of {StateAnimationsCount} animations!");
        }
    }
    #endregion
    #region Super State Animations

    /// <summary>
    /// Start a super state animation, specifying the method to call after it ends
    /// </summary>
    /// <param name="action"></param>
    void StartSuperStateAnimation(Action action)
    {
        SuperStateAnimationReturnMethod = action;
        SuperStateAnimationsInProgress = true;
        CheckSuperStateAnimations();
    }

    //Where to return to after all SuperStateAnimations have finished
    Action SuperStateAnimationReturnMethod;
    bool SuperStateAnimationsInProgress;

    //Animations that persist in-between states, often at the same time as State Animations, so they need seperate handling
    int _SuperStateAnimationsFinished;
    int SuperStateAnimationsFinished
    {
        get => _SuperStateAnimationsFinished;
        set
        {
            _SuperStateAnimationsFinished = value;
            CheckSuperStateAnimations();
        }
    }

    int SuperStateAnimationsCount;

    void CheckSuperStateAnimations()
    {
        //if we're animating AND have finished all animations: progress!
        if (SuperStateAnimationsInProgress && SuperStateAnimationsFinished == SuperStateAnimationsCount)
        {
            SuperStateAnimationsCount = 0;
            SuperStateAnimationsFinished = 0;

            SuperStateAnimationReturnMethod.Invoke();
            SuperStateAnimationsInProgress = false;
        }
        else if (SuperStateAnimationsFinished > SuperStateAnimationsCount)
        {
            GD.PushError($"Too many superstate animations finished! Completed {SuperStateAnimationsFinished} out of {SuperStateAnimationsCount} animations!");
        }
    }
    #endregion

    #endregion
    #region === Initialization Logic ===
    //setup border and graphics and here and stuff
    //like rows too

    public override void _Ready()
    {
        //initialize fields here to the defaults. TODO remove this later with real encounter initialization logic!!!!

        //TODO IMPORTANT
        //we need a method to properly set fields and instantly set related UI elements to the correct number,
        //WITH NO animations/visual logic. we can run this when encounter starts!

        MultPerLevel = Run.Current.MultPerLevel;
        RowsNeededForLevelup = Run.Current.RowsNeededForLevelup;
        Level = Run.Current.BaseLevel;

        LevelMult = Run.Current.BaseMult + Level * MultPerLevel;
        
    }

    public void SetupBoard(int width, int height)
    {
        //Add 2 because our logic below includes the border as 2 additional units.
        //We do this because if we want a board with a playable space of 12x20, we shouldn't have to input 14x22.
        VisualDimensions = new(width + 2, height + 2);
        CellDimensions = new(width, height + 8);
        //Cell Dimensions is different because of the buffer

        //setup sizes here

        CellBoard = new Cell[CellDimensions.X, CellDimensions.Y];
        BoardRows = new BoardRow[CellDimensions.Y];

        for(int y = 0; y < CellDimensions.Y; y++)
        {
            CreateRow(y);
        }

        //initialize visuals
        BlockBox.Position = new(-(width * 5 - 5), height * 5 - 5);

        border.CustomMinimumSize = VisualDimensions * 10;
        darkGradient.Height = height;
        //todo: add collider? idk if we need colliders still
    }

    void CreateRow(int y)
    {
        Cell[] cells = new Cell[CellDimensions.X];

        for (int x = 0; x < CellDimensions.X; x++)
        {
            var NewCell = CreateCell(x, y);

            cells[x] = NewCell;
            CellBoard[x,y] = NewCell;

        }

        BoardRow newRow = new(y, CellDimensions.X, cells);
        AddChild(newRow);
        BoardRows[y] = newRow;

        //todo: bind row events
    }

    Cell CreateCell(int x, int y)
    {
        Cell NewCell = new(x, y);

        NewCell.Scored += Cell_Scored;
        NewCell.UpdatedBoard += () => Board_HasUnprocessedUpdates = true;

        PlayerTurn_Started += NewCell.FullyResetScoringFlag;
        BoardProcessingLoop_Started += NewCell.PartiallyResetScoringFlag;
        
        //todo

        return NewCell;
    }



    #endregion

}

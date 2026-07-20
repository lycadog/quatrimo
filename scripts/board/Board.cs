using Godot;
using System;
using System.Collections.Generic;

[GlobalClass, Icon("res://texture/icon/boardicon.png")]
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

    [Export] public AnimationWaiter StateAnimator;
    [Export] public AnimationWaiter SuperStateAnimator;

    public Enemy Enemy;

    public double CurrentScore;

    int RowsNeededEveryLevelup;

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
            if (value) { EmitSignalBoardUpdated();
                GD.Print("Board updated signal fired!");
            }
        }
    }

    double _LevelMult;
    public double LevelMult
    {
        get => _LevelMult;
        set { _LevelMult = value; ScoreBox.MultiplierLabel.Text = value.ToString(); }
    }

    //these properties auto-update their corresponding UI element
    int _Level;
    int Level
    {
        get => _Level;
        set
        {
            _Level = value;
            LevelStatBoxes.LevelLabel.Text = _Level.ToString(); ;
        }
    }

    double _MultPerLevel;
    double MultPerLevel
    {
        get => _MultPerLevel;
        set
        {
            _MultPerLevel = value;
            LevelStatBoxes.UpdateMultPerLevelLabel(value);
        }
    }

    int _RowsUntilLevelUp;
    int RowsUntilLevelUp
    {
        get => _RowsUntilLevelUp;
        set
        {
            _RowsUntilLevelUp = value;
            LevelStatBoxes.UpdateRowsUntilLevelUpLabel(value);
        }
    }

    #endregion
    #region == Export Fields ==

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
    [Signal] public delegate void Piecefall_StartedEventHandler();
    [Signal] public delegate void Piecefall_EndedEventHandler();

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
    const double BaseScoreTickdownTime = 0.4;
    const double WaitTimeAfterEnemyDies = 0.8;
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

    void PlayerPlaysCard(PieceCard Card)
    {
        //create piece and bind signals
        CurrentPiece = Card.LinkedPiece.CreatePiece();

        CurrentPiece.PiecePlaced += Piecefall_Finished;
        Connect(SignalName.BoardUpdated, new(CurrentPiece, FallingPiece.MethodName.UpdateCollision));
        

        BlockBox.AddChild(CurrentPiece);

        foreach (var block in CurrentPiece.Blocks)
        {
            ConnectNewBlock(block);
        }
        //this stuff should? just work?

        Vector2I startingPosition = new(CellDimensions.X / 2, CellDimensions.Y - 8);

        CurrentPiece.StartFall(startingPosition);


        //start piecefall
        EmitSignalPiecefall_Started();
    }

    void PlaceBlock(Block block)
    {
        block.Reparent(BlockBox);
        PlacedBlocks.Add(block);

        Connect(SignalName.PlayerTurn_Started, new(block, Block.MethodName.Turn_Started));
        Connect(SignalName.TickBlocks, new(block, Block.MethodName.Tick));

        CellBoard[block.BoardPos.X, block.BoardPos.Y].PlaceBlock(block);
        //add block to CellBoard
    }

    void Piecefall_Finished()
    {
        PlacementSfx.Play();
        CurrentPiece = null;
        EmitSignalPiecefall_Ended();
        StartBoardProcessing(TickPriorityBlocks);
        //process board moving onto block ticking after
    }

    /// Logic stops here to go to board processing!!! ///

    void TickPriorityBlocks()
    {
        EmitSignalTickBlocks(true);

        if (Board_HasUnprocessedUpdates)
        {
            StartBoardProcessing(TickNormalBlocks);
            return;
        }

        TickNormalBlocks();
    }

    void TickNormalBlocks()
    {
        EmitSignalTickBlocks(false);

        if (Board_HasUnprocessedUpdates)
        {
            StartBoardProcessing(TallyUpScore);
            return;
        }

        //wait. we can add animation support super easily here i think. if we just start waiting 
        //goto score processing after this !!!

        TallyUpScore();
        //we end up at ResetTurnValues after processing score
    }

    // Logic stops here for score processing!!

    /// <summary>
    /// Reset values and either wait if the enemy is attacking, or progress immediately if not
    /// </summary>
    void ResetTurnValues()
    {
        CurrentScore = Run.Current.BaseScore;
        ScoreBox.ResetNumberHitSFX();

        TurnRowsScored = 0;

        //it is a bit awkward always creating these animations, even when we're not waiting on them,
        //but it prevents excess animations finished error.
        //this may cause a brief pause if a very short animation is started immediately after this, though it shouldn't really matter
        //0.5 second or less pause
        StateAnimator.AddAnimation();
        StateAnimator.AddAnimation();

        //start reset animations, these will finish the animations created above even if we aren't waiting on them (which is fine)
        ScoreBox.ResetValues(LevelMult);
        RowsClearedDial.Reset();

        //todo: change!!!
        if (Enemy.AttackingThisTurn)
        {
            //if the enemy is attacking: wait on all the animations first
            StateAnimator.StartAnimation(StartEnemyTurn);
        }
        else
        {
            //if they're not attacking, progress immediately
            StartEnemyTurn();
        }
    }

    void StartEnemyTurn()
    {
        //start attacking!

        Enemy.PlayTurn();
        //when enemy is done, it will run StartPlayerTurn()
    }

    void FinishTurn()
    {
        if (Board_HasUnprocessedUpdates)
        {
            StartBoardProcessing(StartPlayerTurn);
            return;
        }

        StartPlayerTurn();

    }

    #endregion

    #region === Board Processing ===

    Action ProcessingReturnMethod;
    List<Cell> ScoredCells = [];
    List<BoardRow> ScorableRows = [];

    bool BoardProcessingActive = false;

    //we need to implement animations for rows and numbers and such
    //and superstate animations for these, waiting on them before progressing past board processing

    /// <summary>
    /// Start processing the board for scorable rows. Returns to specified method after processing is done!
    /// </summary>
    /// <param name="ReturnMethod"></param>
    void StartBoardProcessing(Action ReturnMethod)
    {
        if(BoardProcessingActive)
        {
            GD.PushError("Attempted to start board processing while board processing is already happening!");
            return;
        }

        ProcessingReturnMethod = ReturnMethod;
        BoardProcessingActive = true;

        CheckScorability();
    }

    void CheckScorability()
    {
        ScorableRows.Clear();
        ScoredCells.Clear();

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

            //TODO PLAY SOUNDS HERE
            PlayScoringSounds(ScorableRows.Count);
            //we gotta start our animation waiting here, then progressing to LoweringBlocks after !!!

            StateAnimator.StartAnimation(LowerScoredBlocks);
            //we need to update all rows-related values right here!

        }
        //NO SCORING LETS LEAVE!!!!!
        else
        {
            //WE NEED TO WAIT!!! on ALL superstate animations before exiting!!!!
            //no scoring happened! let's skip all this noise


            if(SuperStateAnimator.AnimationsCount == 0)
            {
                ExitBoardProcessing();
                return;
            }

            SuperStateAnimator.StartAnimation(ExitBoardProcessing);
            //after all animations have fully finished, we can exit board processing!
        }
    }

    void Row_CreatedIterator(ScoreIterator iterator)
    {
        StateAnimator.AddAnimation();
        iterator.IteratorCompleted += StateAnimator.FinishAnimation;
    }

    /// <summary>
    /// Updates all cleared row-dependent things, including UI. ClearedRowDial runs this every time it scrolls to a new number!
    /// </summary>
    void AddClearedRowToCount(int rowCount)
    {
        TurnRowsScored++;
        TotalRowsScored++;

        RowsUntilLevelUp--;

        if (RowsUntilLevelUp <= 0)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        Level++;
        LevelStatBoxes.LevelUp();

        RowsUntilLevelUp += RowsNeededEveryLevelup;
        LevelMult += MultPerLevel;
    }

    void Block_Scored(Block block)
    {
        //create score number here!!!
        SuperStateAnimator.AddAnimation();

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
        ScoreBox.PlayNumberHitSFX();

        //score numbers are superstate animations
        SuperStateAnimator.FinishAnimation();
    }

    void Cell_Scored(Cell cell)
    {
        //we start scoring animation!!!

        StateAnimator.AddAnimation();

        ScoreAnimation animation = (ScoreAnimation)ScoreAnimation.Instantiate();
        BlockBox.AddChild(animation);

        animation.Position = cell.RealPosition;
        animation.AnimationFinished += () => StateAnimator.FinishAnimation();

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
            GD.Print("aborting lowering");
            CheckScorability();
            return;
        }

        Tween tween = GetTree().CreateTween().SetParallel();
        foreach (var block in PlacedBlocks)
        {
            StateAnimator.AddAnimation();
            LowerBlock(block, LowestScoredY, tween);
        }

        //loop back to checking scorability after lowering
        StateAnimator.StartAnimation(CheckScorability);
    }

    void LowerBlock(Block block, int lowestScoredY, Tween tween)
    {
        block.BoardPos = new(block.BoardPos.X, block.BoardPos.Y - block.LowerDistance);

        //we don't want the delay to be too long when we have a really tall board, so if it's too tall we decrease it by a bit
        double decreaseDelayPerBlockAtHighValues = (block.BoardPos.Y - lowestScoredY) * .0002;
        double loweringDelay = (block.BoardPos.Y - lowestScoredY) * (.03 - decreaseDelayPerBlockAtHighValues); //* AnimationTimescale;

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

        StateAnimator.FinishAnimation();

    }

    void ExitBoardProcessing()
    {

        Board_HasUnprocessedUpdates = false;
        BoardProcessingActive = false;

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
        //why the fuck were we waiting unnecessarily long on this before? it makes no sense to wait when they don't die. what?

        GetTree().CreateTween()
            .TweenCallback(Callable.From(CheckEnemyHP))
            .SetDelay(TickdownAnimationTime); //+ WaitTimeAfterEnemyDies);
    }

    void CheckEnemyHP()
    {
        if (Enemy.Health <= 0)
        {
            //if enemy is below hp, END encounter!!!

            GetTree().Quit();
            return;
        }

        //enemy alive, let's continue!
        ResetTurnValues();
    }

    #endregion

    #region === Signal Binding & Board Events ===

    /// <summary>
    /// Connect a new block to the necessary signals
    /// </summary>
    /// <param name="block"></param>
    public void ConnectNewBlock(Block block)
    {
        block.Placed += PlaceBlock;
        block.Scored += Block_Scored;
        block.Deleted += () => Block_Deleted(block);
    }

    public void Block_Deleted(Block block)
    {
        //sometimes just barely placed blocks need to be removed, and their IsPlaced is not updated yet!
        if (block.IsPlaced || PlacedBlocks.Contains(block))
        {
            PlacedBlocks.Remove(block);
            Board_HasUnprocessedUpdates = true;
        }
    }

    /// <summary>
    /// Place block directly on board at its board position
    /// </summary>
    /// <param name="block"></param>
    public void PlaceBlockDirectlyOnBoard(Block block, bool doWhiteFlash = true)
    {
        ConnectNewBlock(block);
        BlockBox.AddChild(block);
        //for some reason this was previously adding block as child directly TO us.
        //shouldnt this result in incorrect positioning?
        block.Place(doWhiteFlash);
    }

    #endregion

    #region === Sound Effects ===

    AudioStreamPlayer ScoringSfx;
    AudioStreamPlayer ScoringSfxHarmonics1;
    AudioStreamPlayer ScoringSfxHarmonics2;
    AudioStreamPlayer ScoringSfxHarmonics3;
    AudioStreamPlayer ScoringSfxHarmonics4;
    AudioStreamPlayer ScoringSfxHarmonics5;
    AudioStreamPlayer ScoringSfxHarmonics6;

    AudioStreamPlayer[] ScoringSFXTable;


    [Export] AudioStreamPlayer PlacementSfx;

    void PlayScoringSounds(int RowsScored)
    {
        //play sound depending on how many rows scored !!!

        RowsScored--;

        if(RowsScored < 11)
        {
            ScoringSFXTable[RowsScored].Play();
        }
        else
        {
            //if really high: play final sound
            ScoringSfxHarmonics6.Play();
        }
    }



    #endregion

    #region === Initialization Logic ===
    //setup border and graphics and here and stuff
    //like rows too

    public void StartEncounter(Vector2I boardDimensions, Enemy enemy)
    {
        Enemy = enemy;

        AddChild(enemy);

        enemy.TurnCompleted += FinishTurn;

        SetupBoard(boardDimensions.X, boardDimensions.Y);

        Level = Run.Current.BaseLevel;
        MultPerLevel = Run.Current.MultPerLevel;
        RowsNeededEveryLevelup = Run.Current.RowsNeededForLevelup;
        RowsUntilLevelUp = RowsNeededEveryLevelup;

        LevelMult = Run.Current.BaseMult + Level * MultPerLevel;

        EnemyHealthBar.SetupBar(Enemy.Health, Enemy.MaxHealth);
        BoardAccessor.Board = this;

        ScoringSfx = (AudioStreamPlayer)GetNode("ScoringSfxBase");
        ScoringSfxHarmonics1 = (AudioStreamPlayer)GetNode("ScoringSfxHarmonics1");
        ScoringSfxHarmonics2 = (AudioStreamPlayer)GetNode("ScoringSfxHarmonics2");
        ScoringSfxHarmonics3 = (AudioStreamPlayer)GetNode("ScoringSfxHarmonics3");
        ScoringSfxHarmonics4 = (AudioStreamPlayer)GetNode("ScoringSfxHarmonics4");
        ScoringSfxHarmonics5 = (AudioStreamPlayer)GetNode("ScoringSfxHarmonics5");
        ScoringSfxHarmonics6 = (AudioStreamPlayer)GetNode("ScoringSfxHarmonics6");

        ScoringSFXTable =
            [ScoringSfx,
            ScoringSfxHarmonics1, ScoringSfxHarmonics1, //2, 3
            ScoringSfxHarmonics2, ScoringSfxHarmonics2, //4, 5
            ScoringSfxHarmonics3, ScoringSfxHarmonics3, //6, 7
            ScoringSfxHarmonics4, ScoringSfxHarmonics4, //8, 9
            ScoringSfxHarmonics5, ScoringSfxHarmonics5];//10, 11


        StartPlayerTurn();
    }




    public void SetupBoard(int width, int height)
    {
        //Add 2 because our logic below includes the border as 2 additional units.
        //We do this because if we want a board with a playable space of 12x20, we shouldn't have to input 14x22.
        VisualDimensions = new(width + 2, height + 2);
        CellDimensions = new(width, height + 8);
        //Cell Dimensions is different because of the buffer

        //setup sizes here

        GD.Print("cell dimensions: " + CellDimensions);

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

        newRow.CreatedIterator += Row_CreatedIterator;

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

    #region === Debug Features ===

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Debug2"))
        {
        }

        if (Input.IsActionJustPressed("Debug3"))
        {

        }

        if (Input.IsActionJustPressed("Debug6"))
        {

        }

        if (Input.IsActionJustPressed("Fullscreen"))
        {
            if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen)
            {
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                return;
            }

            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }

        if (Input.IsActionJustPressed("Restart"))
        {
            GetTree().ReloadCurrentScene();
        }

    }


    #endregion

}

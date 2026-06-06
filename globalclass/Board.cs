using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

[GlobalClass, Icon("res://texture/icon/boardicon.png")]
public partial class Board : Control
{
    Cell[,] CellBoard;

    public Cell this[int x, int y]
    {
        get => CellBoard[x,y];
        set => CellBoard[x,y] = value;
    }

    public Vector2I Dimensions;
    public Vector2I CellDimensions;

    [Export] BoardAnimationManager AnimationManager;

    [Export] NinePatchRect border;
	[Export] GradientTexture2D darkGradient;
    [Export] Label ScoreLabel;

    [Export] public Node2D BlockBox;

    [Export] RowsClearedDial RowsClearedDial;

    [Export] Area2D BottomBorder;
    [Export] Area2D LeftBorder;
    [Export] Area2D RightBorder;

	[Export] int defaultX = 12, defaultY = 20;

    [Signal] public delegate void TurnStartedEventHandler();
	[Signal] public delegate void PiecePlayedEventHandler();
    [Signal] public delegate void PiecePlacedEventHandler();
    [Signal] public delegate void ScoreStepStartedEventHandler(bool isInitialStep);
    [Signal] public delegate void TickStepEventHandler(bool isPriorityTick);
    [Signal] public delegate void TurnEndedEventHandler();

    [Signal] public delegate void BoardChangedEventHandler();

    public double totalScore = 0;

    int RowsScored = 0;
    int Level = 1;
    int TurnCount = 1;

    bool _BoardUpdated = false;
    public bool BoardUpdated
    {
        get => _BoardUpdated;
        set
        {
            _BoardUpdated = value;
            if (_BoardUpdated)
            {
                EmitSignalBoardChanged();
            }

        }
    }

    List<Cell> ScoredCells = [];
    List<Block> PlacedBlocks = [];
    public BoardRow[] Rows;

    FallingPiece CurrentPiece;

    static PackedScene ScoreAnimation = ResourceLoader.Load<PackedScene>("uid://joftg3j7lslu");

    /// <summary>
    /// Time scale for our animations, goes down every time blocks are lowered
    /// </summary>
    public static double AnimationTimescale = 1.0;
    const double LowerAnimLength = .3;

    #region === Board Logic ===

    public void StartTurn()
    {
        GD.Print("Turn started!");
        AnimationTimescale = 1.0;
        BoardUpdated = false;
        AnimationManager.ClearAnimations();
        RowsClearedDial.Reset();
        EmitSignalTurnStarted();
    }

    void ScoreBoard(bool initialStep)
    {
        if(!BoardUpdated && !initialStep)
        {
            //progress forwards!
            //this is one of 2 ways we should tick blocks. nothing outside of this method should be ticking blocks
            TickBlocks();
            return;
        }

        ScoredCells.Clear();
        EmitSignalScoreStepStarted(initialStep);

        //set this to false. if we score a row, we will set it to true
        BoardUpdated = false;

        //this will create scoring animations, which will handle repeating this state if applicable.
        foreach (var row in Rows)
        {
            row.AttemptScoring(); //this will update the board if scoring succeeds
        }

        if(!BoardUpdated) //if the board hasn't changed after scoring we can progress
        {
            GD.Print("Skipping score state because board is not updated");
            TickBlocks(); //this is the only other allowed end spot of this state
            return;
        }

        GD.Print("Scoring now");
    }

    void TickBlocks()
    {
        //this may be too simple. reinspect it sooner todo

        //rewrite this 

        EmitSignalTickStep(true);

        if (BoardUpdated) //if the board has updated we gotta check it again
        {
            
            ScoreBoard(false);
            return;
        }

        EmitSignalTickStep(false);

        if (BoardUpdated) //if the board has updated we gotta check it again
        {
            ScoreBoard(false);
            return;
        }

        BeginTurnEnd(); //nothing to score, let's progress
    }

    void BeginTurnEnd()
    {
        //setup stuff here
        AnimationManager.StartEndOfTurnWaiting();
    }

    void EndTurn()
    {
        EmitSignalTurnEnded();
        StartTurn();
    }

    void LowerToFillScoredSpaces()
    {
        int lowestScoredY = 100;
        foreach(var cell in ScoredCells)
        {
            //iterate upwards in the array, setting lower values for every cell

            lowestScoredY = Math.Min(cell.y, lowestScoredY); //get the lowest value of any scored cell

            for (int y = cell.y+1; y < CellDimensions.Y - 1; y++)
            {
                CellBoard[cell.x, y].LowerDistance++;
            }

            cell.DeleteBlock();
        }

        ScoredCells.Clear();

        if(PlacedBlocks.Count == 0)
        {
            //this is somewhat buggy. we had to change cell.DeleteBlock() to immediately remove the block to fix
            //duplicated scoring from happening right here
            //if we go through with lowering block, the animation takes enough time that
            //the block is freed and removed from the cell, fixing the issue
            ScoreBoard(false);
            return;
        }

        GD.Print("Lowering blocks");

        Tween tween = GetTree().CreateTween().SetParallel();

        foreach(var block in PlacedBlocks)
        {
            AnimationManager.AnimationCreated();
            LowerBlock(block, lowestScoredY, tween);
        }

        //shrink timescale each time this is ran so stuff goes by faster
        AnimationTimescale = Math.Max(AnimationTimescale - .1, .04);
        GD.Print(AnimationTimescale);

        AnimationManager.StartAnimating(() => ScoreBoard(false));
    }

    void LowerBlock(Block block, int lowestScoredY, Tween tween)
    {
        block.BoardPos = new(block.BoardPos.X, block.BoardPos.Y - block.LowerDistance);

        //we don't want the delay to be too long when we have a really tall board, so if it's too tall we decrease it by a bit
        double decreaseDelayPerBlockAtHighValues = (block.BoardPos.Y - lowestScoredY) * .0002;
        double loweringDelay = (block.BoardPos.Y - lowestScoredY) * (.03 - decreaseDelayPerBlockAtHighValues) * AnimationTimescale;

        //todo: change delay to be more exponential falloff. higher Y values give less delay

        //tween lowering to our new position, and we actually move when its done with the method
        tween.TweenProperty(block, "position",
            new Vector2(0, block.LowerDistance * 10), LowerAnimLength * AnimationTimescale).AsRelative()
            .SetTrans(Tween.TransitionType.Bounce)
            .SetDelay(loweringDelay).SetEase(Tween.EaseType.Out)
            .Finished += () => Block_FinishedLowering(block);
    }

    public void Block_FinishedLowering(Block block)
    {
        CellBoard[block.BoardPos.X, block.BoardPos.Y].HeldBlock = block;
        block.LowerDistance = 0;

        AnimationManager.AnimationCompleted();
    }

    /// <summary>
    /// Initialize the cells and rows required for board behavior to function
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    void CreateBlockBoard(int width, int height)
    {
        height += 8; //Add an extra 8 height so we have a buffer above the board

        CellDimensions = new(width, height);

        CellBoard = new Cell[width, height];

        Rows = new BoardRow[height];

        for(int y = 0; y < height; y++)
        {
            BoardRow row = new(y, width);
            AddChild(row);
            Rows[y] = row;

            //bind row events here *********                                                <*************
            row.ScoringStarted += Row_StartedScoring;
            row.CreatedIterator += Row_CreatedIterator;

            Cell[] cellsInRow = new Cell[width];

            for(int x = 0; x < width; x++)
            {
                Cell newCell = new(x, y);

                //bind cell events here ****************                                    <*************

                newCell.UpdatedBoard += () => BoardUpdated = true;
                newCell.Scored += OnCellScored;

                CellBoard[x, y] = newCell;

                ScoreStepStarted += newCell.OnScoreStepBegin;

                cellsInRow[x] = newCell;
                row.BindCell(CellBoard[x, y]);
            }

            row.cellsInRow = cellsInRow;
        }

    }

    /// <summary>
    /// Place block directly on board at its board position
    /// </summary>
    /// <param name="block"></param>
    public void PlaceBlockDirectlyOnBoard(Block block, bool doWhiteFlash = true)
    {
        ConnectNewBlock(block);
        AddChild(block);
        block.Place(doWhiteFlash);
    }

    #endregion
    #region === Signal & Event Binding ===

    void ConnectNewPiece(FallingPiece piece)
    {
        piece.Connect(FallingPiece.SignalName.PiecePlaced, new(this, MethodName.OnPiecePlaced), (uint)ConnectFlags.OneShot);

        Connect(SignalName.BoardChanged, new(piece, FallingPiece.MethodName.UpdateCollision));
    }

    /// <summary>
    /// Connect a new block to all the necessary signals
    /// </summary>
    /// <param name="block"></param>
    public void ConnectNewBlock(Block block)
    {
        block.Connect(Block.SignalName.Placed, new(this, MethodName.OnBlockPlaced), (uint)ConnectFlags.OneShot);
        block.Connect(Block.SignalName.Scored, new(this, MethodName.OnBlockScored));
        block.Connect(Block.SignalName.Deleted, new(this, MethodName.OnBlockDeleted), (uint)ConnectFlags.AppendSourceObject);

        Connect(SignalName.TurnStarted, new(block, Block.MethodName.TurnStarted));
        Connect(SignalName.TickStep, new(block, Block.MethodName.Tick));
    }

    #endregion
    #region === Event Methods ===

    void OnScoreNumberReachesScore(double number)
    {
        totalScore += number;
        ScoreLabel.Text = totalScore.ToString();
        AnimationManager.TurnEndAnimationCompleted();
    }

    public void Row_StartedScoring()
    {
        BoardUpdated = true;
        AnimationManager.StartAnimating(LowerToFillScoredSpaces);
        RowsClearedDial.AddRow();
        
        //set up our animationmanager to score again when we conclude!
    }

    public void Row_CreatedIterator(ScoreIterator iterator)
    {
        AnimationManager.AnimationCreated();
        iterator.IteratorCompleted += AnimationManager.AnimationCompleted;
    }

    public void OnBlockScored(Block block)
    {
        AnimationManager.AddTurnEndAnimation();

        var newNumber = MiniScoreNumber.GetNew(block.ScoreValue, ScoreLabel.GetGlobalRect().GetCenter());
        newNumber.NumberReachedScore += OnScoreNumberReachesScore;
        BlockBox.AddChild(newNumber);
        newNumber.Position = block.Position;
    }

    public void OnCellScored(Cell cell)
    {
        ScoreAnimation animation = (ScoreAnimation)ScoreAnimation.Instantiate();

        BlockBox.AddChild(animation);

        AnimationManager.AnimationCreated();

        animation.Position = cell.RealPosition;
        animation.AnimationFinished += AnimationManager.AnimationCompleted;

        if (cell.FilledInOnScoring)
        {
            ScoredCells.Add(cell);
        }
    }



    public void OnBlockDeleted(Block block)
    {
        //sometimes just barely placed blocks need to be removed, and their IsPlaced is not updated yet!
        if (block.IsPlaced || PlacedBlocks.Contains(block))
        {
            PlacedBlocks.Remove(block);
            BoardUpdated = true; 
            //why wasn't this updating the board?
        }
    }

    public void OnBlockPlaced(Block block)
    {
        block.Reparent(BlockBox);
        PlacedBlocks.Add(block); //this could be the problem but it should be fixed now

        CellBoard[block.BoardPos.X, block.BoardPos.Y].PlaceBlock(block);
        //add block to CellBoard
    }

    public void OnPiecePlaced()
    {
        EmitSignalPiecePlaced();
        CurrentPiece = null;
        ScoreBoard(true);
        //move onto scoring now!
    }

    public void BlockCreatesBlock(Block block)
    {
        ConnectNewBlock(block);
    }

    /// <summary>
    /// When a card is played we create its piece and its blocks, binding signals and dropping the piece
    /// </summary>
    /// <param name="card"></param>
    public void PlayCard(PieceCard card)
    {
        CurrentPiece = card.LinkedPiece.CreatePiece();

        BlockBox.AddChild(CurrentPiece);

        ConnectNewPiece(CurrentPiece);
        foreach (var block in CurrentPiece.Blocks)
        {
            ConnectNewBlock(block);
        }

        Vector2I startingPosition = new((Dimensions.X - 2) / 2, Dimensions.Y - 2);

        CurrentPiece.StartFall(startingPosition);

        EmitSignalPiecePlayed();
    }

    #endregion
    #region === Godot Methods ===

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetDimensions(defaultX, defaultY);
        BoardAccessor.CurrentBoard = this;
        StartTurn();
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Debug2"))
        {

        }

        if (Input.IsActionJustPressed("Debug3"))
        {
            foreach (var row in Rows)
            {
                GD.Print($"row {row.y} scorability: {row.totalScorability}, can score: {row.Scorable}");
            }
        }

        

        if (Input.IsActionJustPressed("Fullscreen"))
        {
            if(DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen)
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
    #region === Visuals and Initialization ===

    void SetDimensions(int width, int height)
    {
        CreateBlockBoard(width, height);

        BlockBox.Position = new(-(width * 5 - 5), height * 5 - 5);
        //this was * 10 before, which shouldnt work. im not sure why it was * 10. maybe for a reason? check later
        //set the position of this to the bottom left, all blocks will go in here

        width += 2; height += 2;
        //Add 2 because our logic below includes the border as 2 additional units.
        //We do this because if we want a board with a playable space of 12x20, we shouldn't have to input 14x22.

        Dimensions = new Vector2I(width, height);
        border.CustomMinimumSize = Dimensions * 10;

        GD.Print($"Board resized, new dimensions: {Dimensions}");


        darkGradient.Height = height;

        BottomBorder.Position = new Vector2(width * 5, height * 10 - 5);
        BottomBorder.Scale = new Vector2(width, 1);

        LeftBorder.Position = new Vector2(5, height * 5);
        RightBorder.Position = new Vector2(width * 10 - 5, height * 5);

        LeftBorder.Scale = new Vector2(1, height);
        RightBorder.Scale = new Vector2(1, height);
    }

    void SetDimensions(Vector2I dimensions)
    {
        SetDimensions(dimensions.X, dimensions.Y);
    }

    #endregion

}

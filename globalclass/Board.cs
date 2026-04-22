using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

[GlobalClass, Icon("res://texture/icon/boardicon.png")]
public partial class Board : Control
{
	Vector2I Dimensions;
    Vector2I CellDimensions;

    [Export] BoardAnimationManager AnimationManager;

    [Export] NinePatchRect border;
	[Export] GradientTexture2D darkGradient;
    [Export] Label ScoreLabel;

    [Export] Area2D BoardArea;
    [Export] Area2D BottomBorder;
    [Export] Area2D LeftBorder;
    [Export] Area2D RightBorder;

	[Export] int defaultX = 12, defaultY = 20;

    [Signal] public delegate void TurnStartedEventHandler();
	[Signal] public delegate void PiecePlayedEventHandler();
    [Signal] public delegate void ScoreStepStartedEventHandler(bool isInitialStep);
    [Signal] public delegate void TickStepStartedEventHandler();
    [Signal] public delegate void TurnEndedEventHandler();

    [Signal] public delegate void AnimationCreatedEventHandler();
    [Signal] public delegate void AnimationEndedEventHandler();

    public double totalScore = 0;

    public bool BoardUpdated = false;

    List<Cell> ScoredCells = [];
    List<Block> PlacedBlocks = [];
    BoardRow[] Rows;
    Cell[,] CellBoard;

    FallingPiece CurrentPiece;

    static PackedScene ScoreAnimation = ResourceLoader.Load<PackedScene>("uid://joftg3j7lslu");
    const double LowerAnimLength = .12;

    #region === Board Logic ===

    public void StartTurn()
    {
        BoardUpdated = false;
        AnimationManager.ClearAnimations();
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
        BoardUpdated = false;

        //this will create scoring animations, which will handle repeating this state if applicable.
        foreach (var row in Rows)
        {
            row.AttemptScoring(); //this will update the board if scoring succeeds
        }

        if(!BoardUpdated) //if the board hasn't changed after scoring we can progress
        {
            TickBlocks(); //this is the only other allowed end spot of this state
        }
    }

    void TickBlocks()
    {
        //this may be too simple. reinspect it sooner todo

        EmitSignalTickStepStarted();
        //signal our blocks that we're ticking

        if (BoardUpdated) //if the board has updated we gotta check it again
        {
            ScoreBoard(false);
            return;
        }

        EndTurn(); //nothing to score, let's progress
    }

    void EndTurn()
    {
        GD.Print("Turn ended!");
        EmitSignalTurnEnded();
        StartTurn();
    }

    void LowerToFillScoredSpaces()
    {
        foreach(var cell in ScoredCells)
        {
            //iterate upwards in the array, setting lower values for every cell

            for (int y = cell.y+1; y < CellDimensions.Y - 1; y++)
            {
                CellBoard[cell.x, y].LowerDistance++;
            }

            cell.DeleteBlock();
        }

        ScoredCells.Clear();

        if(PlacedBlocks.Count == 0)
        {
            //why are we returning? what are we returning to? doesn't this just hang forever???
            return;
        }

        AnimationManager.StartAnimating(() => ScoreBoard(false));

        Tween tween = GetTree().CreateTween().SetParallel();

        foreach(var block in PlacedBlocks)
        {
            LowerBlock(block, tween);
        }


    }

    void LowerBlock(Block block, Tween tween)
    {
        block.boardY -= block.LowerDistance;

        EmitSignalAnimationCreated();

        //todo: change delay to be more exponential falloff. higher Y values give less delay

        //tween lowering to our new position, and we actually move when its done with the method
        tween.TweenProperty(block, "position",
            new Vector2(0, block.LowerDistance * 10), LowerAnimLength).AsRelative()
            .SetTrans(Tween.TransitionType.Quad)
            .SetDelay(block.boardY * .03)
            .Finished += () => OnBlockFinishedLowering(block);
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
            row.ScoringStarted += OnRowStartedScoring;

            Cell[] cellsInRow = new Cell[width];

            for(int x = 0; x < width; x++)
            {
                Cell newCell = new Cell(x, y, GetRealPosition(x, y));

                //bind cell events here ****************                                    <*************
                newCell.UpdatedBoard += () => BoardUpdated = true;
                newCell.DeletedBlock += OnBlockExitingTree;

                CellBoard[x, y] = newCell;

                ScoreStepStarted += newCell.OnScoreStepBegin;

                cellsInRow[x] = newCell;
                row.BindCell(CellBoard[x, y]);
            }

            row.cells = cellsInRow;
        }

    }

    #endregion
    #region === Event Methods ===

    public void OnRowStartedScoring()
    {
        BoardUpdated = true;
        AnimationManager.StartAnimating(LowerToFillScoredSpaces);
        //set up our animationmanager to score again when we conclude!
    }
    

    public void OnBlockScored(Block block)
    {
        ScoreAnimation animation = (ScoreAnimation)ScoreAnimation.Instantiate();

        AddChild(animation);

        totalScore += block.ScoreValue;
        ScoreLabel.Text = totalScore.ToString();

        animation.GlobalPosition = block.GlobalPosition;
        animation.AnimationFinished += EmitSignalAnimationEnded;

        EmitSignalAnimationCreated();

        if (block.RemovedOnScoring)
        {
            GD.Print($"Flagging cell {block.boardX}, {block.boardY} for deletion");
            ScoredCells.Add(CellBoard[block.boardX, block.boardY]);
        }
    }

    public void OnBlockFinishedLowering(Block block)
    {
        CellBoard[block.boardX, block.boardY].HeldBlock = block;
        block.LowerDistance = 0;

        EmitSignalAnimationEnded();
    }

    public void OnBlockExitingTree(Block block)
    {
        if (block.isPlaced)
        {
            PlacedBlocks.Remove(block);
        }
    }

    public void OnBlockPlaced(Block block)
    {
        block.Reparent(this);
        PlacedBlocks.Add(block);

        Vector2I blockpos = GetBlockPosition(block.Position);

        CellBoard[blockpos.X, blockpos.Y].PlaceBlock(block);
        //add block to CellBoard
    }

    public void OnPiecePlaced(FallingPiece piece)
    {
        CurrentPiece = null;
        ScoreBoard(true);
        //move onto scoring now!
    }

    public void OnPiecePlayed(FallingPiece piece)
    {

    }

    /// <summary>
    /// When a card is played we create its piece and its blocks, binding signals
    /// </summary>
    /// <param name="card"></param>
    public void OnCardPlayed(PieceCard card)
    {
        CurrentPiece = card.LinkedPiece.CreatePiece();

        AddChild(CurrentPiece);

        //BIND SIGNALS HERE **********************************************************************************************

        CurrentPiece.Connect(FallingPiece.SignalName.OnPiecePlacement, new(this, MethodName.OnPiecePlaced), (uint)ConnectFlags.OneShot);
        foreach (var block in CurrentPiece.Blocks)
        {
            block.Connect(Block.SignalName.Placed, new(this, MethodName.OnBlockPlaced), (uint)ConnectFlags.OneShot);
            block.Connect(Block.SignalName.Scored, new(this, MethodName.OnBlockScored));
            block.Connect(Node.SignalName.TreeExiting, new(this, MethodName.OnBlockExitingTree), (uint)ConnectFlags.AppendSourceObject);

            Connect(SignalName.TurnStarted, new(block, Block.MethodName.OnTurnStart));
            Connect(SignalName.TickStepStarted, new(block, Block.MethodName.OnTickStep));


            //block signals here
        }

        CurrentPiece.Position = new Vector2(5, -Dimensions.Y * 5 + 5);
        CurrentPiece.Play();

        OnPiecePlayed(CurrentPiece);
        EmitSignalPiecePlayed();
    }

    public void OnAreaEntered(Area2D area)
    {
        if (area is Block)
        {
            Block block = area as Block;
            block.OnEnterBoard();
        }
    }

    public void OnAreaExited(Area2D area)
    {
        if (area is Block)
        {
            Block block = area as Block;
            block.OnExitBoard();
        }
    }

    #endregion
    #region === Godot Methods ===

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetDimensions(defaultX, defaultY);
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

    void SetDimensions(int x, int y)
    {
        BoardArea.Scale = new(x - .5f, y - .5f);

        CreateBlockBoard(x, y);

        x += 2; y += 2;
        //Add 2 because our logic below includes the border as 2 additional units.
        //We do this because if we want a board with a playable space of 12x20, we shouldn't have to input 14x22.

        Dimensions = new Vector2I(x, y);
        border.CustomMinimumSize = Dimensions * 10;

        GD.Print($"Board resized, new dimensions: {Dimensions}");


        darkGradient.Height = y;

        BottomBorder.Position = new Vector2(x * 5, y * 10 - 5);
        BottomBorder.Scale = new Vector2(x, 1);

        LeftBorder.Position = new Vector2(5, y * 5);
        RightBorder.Position = new Vector2(x * 10 - 5, y * 5);

        LeftBorder.Scale = new Vector2(1, y);
        RightBorder.Scale = new Vector2(1, y);
    }

    void SetDimensions(Vector2I dimensions)
    {
        SetDimensions(dimensions.X, dimensions.Y);
    }

    #endregion
    #region === Board Utils ===

    public Vector2I GetBlockPosition(Vector2 realPosition)
    {
        int boardX = ((int)realPosition.X + (CellDimensions.X * 5 - 5)) / 10;
        int boardY = ((int)realPosition.Y + (CellDimensions.Y * 5 - 5)) / 10 + 5; //5 fixes everything magic number. idk

        boardY = CellDimensions.Y - boardY; //invert the value so we can flip Y (0,0 is bottom left)

        return new(boardX, boardY);
    }

    public Vector2 GetRealPosition(int x, int y)
    {
        float realX = x * 10 - (CellDimensions.X * 5 - 5);

        int invertedY = CellDimensions.Y - y;

        float realY = (invertedY - 5) * 10 - (CellDimensions.Y * 5 - 5);

        return new(realX, realY);
    }

    public Vector2 GetRealPosition(Vector2I blockpos)
    {
        return GetRealPosition(blockpos.X, blockpos.Y);
    }

    #endregion

}

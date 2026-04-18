using Godot;
using System;

[GlobalClass, Icon("res://texture/icon/boardicon.png")]
public partial class Board : Control
{
	Vector2I Dimensions;
    Vector2I CellDimensions;

	[Export] NinePatchRect border;
	[Export] GradientTexture2D darkGradient;

    [Export] Area2D BoardArea;
    [Export] Area2D BottomBorder;
    [Export] Area2D LeftBorder;
    [Export] Area2D RightBorder;

	[Export] int defaultX, defaultY;

    [Signal] public delegate void TurnStartedEventHandler();
	[Signal] public delegate void PiecePlayedEventHandler();
    [Signal] public delegate void TickStepStartedEventHandler();
    [Signal] public delegate void ScoreStepStartedEventHandler(bool isInitialStep);

    public bool BoardUpdated = false;

    BoardRow[] Rows;
    Cell[,] CellBoard;

    FallingPiece CurrentPiece;

    #region === Board Logic ===

    public void StartTurn()
    {
        BoardUpdated = false;
        EmitSignalTurnStarted();
    }

    void ScoreBoard(bool initialStep)
    {
        if(!BoardUpdated && !initialStep)
        {
            TickBlocks();
            return;
        }

        EmitSignalScoreStepStarted(initialStep);
        BoardUpdated = false;

        foreach (var row in Rows)
        {
            row.AttemptScoring();
        }

        ScoreBoard(false);
    }

    void TickBlocks()
    {

        StartTurn();



    }

    void EndTurn()
    {

    }

    void LowerCollumn(int x, int startingY)
    {
        //todo: tween this?

        //Go up and bring stuff down!
        for (int y = startingY; y < CellDimensions.Y; y++)
        {
            if (y == CellDimensions.Y - 1)
            {
                //if we're at the top we don't need to lower anything since there's nothing above us
                //this line below should throw an error. be concerned if it doesn't!
                CellBoard[x, y].RemoveBlock();
                return;
            }
            CellBoard[x, y].HeldBlock = CellBoard[x, y + 1].HeldBlock;
            //replace our block with the above block
            //SHOULD remove extra references automatically. double check!
        }
    }

    void CreateBlockBoard(int width, int height)
    {
        height += 8; //Add an extra 8 height so we have a buffer above the board

        CellDimensions = new(width, height);

        GD.Print(CellDimensions);

        CellBoard = new Cell[width, height];

        Rows = new BoardRow[height];

        for(int y = 0; y < height; y++)
        {
            BoardRow row = new(y, width);

            AddChild(row);

            Rows[y] = row;

            Cell[] cellsInRow = new Cell[width];

            for(int x = 0; x < width; x++)
            {
                Cell newCell = new Cell(x, y, GetRealPosition(x, y));

                newCell.UpdatedBoard += () => { BoardUpdated = true; }; //bind events

                CellBoard[x, y] = newCell;

                ScoreStepStarted += newCell.OnScoreStepBegin;

                cellsInRow[x] = newCell;
                row.BindCell(CellBoard[x, y]);
            }

            row.cells = cellsInRow;
        }

    }

    #endregion
    #region === Godot Methods ===

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetDimensions(defaultX, defaultY);
        StartTurn();

        GD.Print(GetRealPosition(new(0, 0)));
        GD.Print(GetRealPosition(new(2, 2)));
        GD.Print(GetRealPosition(new(4, 4)));
        GD.Print(GetRealPosition(new(11, 20)));
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

        if (Input.IsActionJustPressed("Debug1"))
        {
            StartTurn();
        }

        if (Input.IsActionJustPressed("Debug2"))
        {
            GD.Print("board updated? " + BoardUpdated);
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
    #region === Event Methods ===

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

        //BIND SIGNALS HERE ******************************************

        CurrentPiece.Connect(FallingPiece.SignalName.OnPiecePlacement, new(this, MethodName.OnPiecePlaced), (uint)ConnectFlags.OneShot);
        foreach(var block in CurrentPiece.blocks)
        {
            block.Connect(Block.SignalName.Placed, new(this, MethodName.OnBlockPlaced), (uint)ConnectFlags.OneShot);

            Connect(SignalName.TurnStarted, new(block, Block.MethodName.OnTurnStart));

            //block signals here
        }

        CurrentPiece.Position = new Vector2(5, -Dimensions.Y * 5 + 5);
        CurrentPiece.Play();

        OnPiecePlayed(CurrentPiece);
        EmitSignalPiecePlayed();
    }

    public void OnBlockPlaced(Block block)
    {
        block.Reparent(this);

        Vector2I blockpos = GetBlockPosition(block.Position);

        GD.Print($"block placed at {blockpos.X}, {blockpos.Y}");

        CellBoard[blockpos.X, blockpos.Y].PlaceBlock(block);
        //add block to CellBoard
    }

    public void OnPiecePlaced(FallingPiece piece)
    {
        CurrentPiece = null;
        GD.Print("Piece placed!");

        ScoreBoard(true);
        //move onto scoring now!
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

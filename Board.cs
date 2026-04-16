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

    Cell[,] CellBoard;

    FallingPiece CurrentPiece;

    public void StartTurn()
    {
        EmitSignalTurnStarted();

    }

    public void OnCardPlayed(PieceCard card)
    {
        CurrentPiece = card.LinkedPiece.CreatePiece();

        AddChild(CurrentPiece);

        //fucking c# godot man


        //bind signals here

        CurrentPiece.Connect(FallingPiece.SignalName.OnPiecePlacement, new(this, MethodName.OnPiecePlaced), (uint)ConnectFlags.OneShot);

        foreach(var block in CurrentPiece.blocks)
        {
            block.Connect(Block.SignalName.Placed, new(this, MethodName.OnBlockPlaced), (uint)ConnectFlags.OneShot);
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

        //convert godot position into board position
        int boardX = (int)((block.Position.X + (CellDimensions.X * 5 - 5)) / 10);
        int boardY = (int)((block.Position.Y + (CellDimensions.Y * 5 - 5)) / 10) + 5;
        //5 fixes the centering issue with the board buffer. no fucking clue why
        //i think its bc of the offset

        boardY = CellDimensions.Y - boardY; //invert the value so we can flip Y (0,0 is bottom left)

        GD.Print($"block placed at {boardX}, {boardY}");

        CellBoard[boardX, boardY].PlaceBlock(block);
        //add block to CellBoard
    }

    public void OnPiecePlaced(FallingPiece piece)
    {
        GD.Print("Piece placed!");

        //move onto scoring now!
        //next up: here!
    }

    public void OnAreaEntered(Area2D area)
    {
        if(area is Block)
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

    public void OnPiecePlayed(FallingPiece piece)
    {
        
    }



    void LowerCollumn(int x, int startingY)
    {
        //Go up and bring stuff down!



        for(int y = startingY; y < CellDimensions.Y; y++)
        {
            if (y == CellDimensions.Y - 1) 
            {
                //if we're at the top we don't need to lower anything
                return;
            }


            

        }


    }



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		SetDimensions(defaultX, defaultY);
        StartTurn();
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        
        if (Input.IsActionJustPressed("Debug1"))
        {
            if(CurrentPiece != null) //bad
            {
                CurrentPiece.Free();
            }
            
            StartTurn();
        }

    }

    void CreateBlockBoard(int width, int height)
    {
        height += 8; //Add an extra 8 height so we have a buffer above the board

        CellDimensions = new(width, height);

        GD.Print(CellDimensions);

        CellBoard = new Cell[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                CellBoard[x, y] = new Cell(x, y);
            }
        }

    }

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
}

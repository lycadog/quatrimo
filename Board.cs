using Godot;
using System;

[GlobalClass, Icon("res://texture/icon/boardicon.png")]
public partial class Board : Control
{
	Vector2I Dimensions;

	[Export] NinePatchRect border;
	[Export] GradientTexture2D darkGradient;

    [Export] Area2D BottomBorder;
    [Export] Area2D LeftBorder;
    [Export] Area2D RightBorder;

	[Export] int defaultX, defaultY;

    [Signal] public delegate void TurnStartedEventHandler();
	[Signal] public delegate void PiecePlayedEventHandler();

    FallingPiece CurrentPiece;

    public void StartTurn()
    {
        EmitSignalTurnStarted();

    }

    public void OnCardPlayed(PieceCard card)
    {
        CurrentPiece = card.LinkedPiece.CreatePiece();

        AddChild(CurrentPiece);


        //TODO: bind piece signals HERE. we've got our hands on these unruly pieces here so we need to bind them before they wiggle away

        CurrentPiece.Position = new Vector2(5, -Dimensions.Y * 5 + 5);
        CurrentPiece.Play();

        EmitSignalPiecePlayed();

    }

    public void OnBlockPlaced(Block block)
	{

	}

    public void OnPiecePlaced(FallingPiece piece)
    {

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


    void SetDimensions(int x, int y)
    {
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

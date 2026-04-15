using Godot;
using System;

[GlobalClass, Icon("res://texture/icon/boardicon.png")]
public partial class Board : Control
{
	Vector2I Dimensions;

	[Export] NinePatchRect border;
	[Export] GradientTexture2D darkGradient;

	[Export] Area2D TopBorder;
    [Export] Area2D BottomBorder;
    [Export] Area2D LeftBorder;
    [Export] Area2D RightBorder;

    [Signal] public delegate void OnTurnStartEventHandler();
	[Signal] public delegate void OnPiecePlayedEventHandler();


	public void OnBlockPlaced(Block block)
	{

	}

	

	void SetDimensions(int x, int y)
	{
		x += 2; y += 2; 
		//Add 2 because our logic below includes the border as 2 additional units.
		//We do this because if we want a board with a playable space of 12x20, we shouldn't have to input 14x22.

		Dimensions = new Vector2I(x, y);
		border.Size = Dimensions * 10;

        GD.Print($"Board resized, new dimensions: {Dimensions}");


        darkGradient.Height = y;

        TopBorder.Position = new Vector2(x * 5, 5);
		BottomBorder.Position = new Vector2(x * 5, y * 10 - 5);

		TopBorder.Scale = new Vector2(x, 1);
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

    public void OnBorderResize()
	{
		Dimensions = (Vector2I)(border.Size * .10f);

		

    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetDimensions(8, 12);
		//darkGradient.Height = (int)(border.Size.Y * .10f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

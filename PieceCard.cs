using Godot;
using System;

[GlobalClass, Icon("res://texture/icon/cardicon.png")]
public partial class PieceCard : TextureRect
{
	[Export] TextureRect HighlightBars;

	bool IsSelected = false;
	public float YOffset;

	public void LinkToPiece()
	{

	}


	public void UpdatePosition(float spacing, int index)
	{
		float y = (index * 34) + (spacing * index) + YOffset; 
		
		Position = new(0, y);
	}

	public void Select()
	{
		HighlightBars.Visible = true;
		IsSelected = true;
	}

	public void Deselect()
	{
		HighlightBars.Visible = false;
		IsSelected = false;
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

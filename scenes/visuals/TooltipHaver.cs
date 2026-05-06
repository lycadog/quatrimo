using Godot;
using System;

[GlobalClass]
public partial class TooltipHaver : Control
{
	[Export] PackedScene TooltipScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


    public override GodotObject _MakeCustomTooltip(string forText)
    {
		
        return TooltipScene.Instantiate();
    }

	public enum TooltipType
	{
		Block,		//0
		Piece,		//1
		PieceTag	//2
	}
}

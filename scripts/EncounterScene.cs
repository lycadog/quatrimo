using Godot;
using System;

public partial class EncounterScene : Control
{
	ColorRect CrtRect;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CrtRect = GetNode<ColorRect>("/root/CrtRect");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Input.IsActionJustPressed("Debug5"))
        {
			CrtRect.Visible = !CrtRect.Visible;
        }
    }
}

using Godot;
using System;

public partial class EncounterScene : Control
{
	ColorRect CRTRect;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CRTRect = (ColorRect)GetNode("CRTRect");
		CRTRect.Visible = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Input.IsActionJustPressed("Debug5"))
        {
			CRTRect.Visible = !CRTRect.Visible;
        }
    }
}

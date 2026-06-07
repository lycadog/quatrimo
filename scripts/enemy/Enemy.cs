using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : Node
{
	public int Level = 1;

    protected Attack[] AllAttacks;
    protected List<Attack> AvailableAttacks = [];

	[Export] protected int StartingLevel = 1;

	[Export] bool ScaleLevel = true;
	[Export] int LevelupEveryXTurns = 30;
	
	
	public void PlayTurn()
	{



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

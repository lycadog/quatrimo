using Godot;
using System;

public partial class EncounterScene : Control
{
	ColorRect CrtRect;
	[Export] Board Board;
	[Export] EnemyAttackPanel AttackPanel;

	Vector2I BoardDimensions;
	Enemy Enemy;

	PackedScene DefaultEnemy = ResourceLoader.Load<PackedScene>("uid://1d3m7ucqbcrk");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CrtRect = GetNode<ColorRect>("/root/CrtRect");

		if (Enemy == null)
		{
			//if nothing specified: auto-initialize with basic instructions
			Enemy = (Enemy)DefaultEnemy.Instantiate();
			BoardDimensions = new(12, 20);
		}

		Enemy.AttackPanel = AttackPanel;

		Board.StartEncounter(BoardDimensions, Enemy);

		//pass bag to playerhand!

		//initialize board from values!
		//hookup enemy!
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

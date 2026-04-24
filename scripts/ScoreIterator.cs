using Godot;
using System;

public partial class ScoreIterator(int x, int direction, Cell[] row) : Node
{

	const double TimeBetweenIterating = 0.03;
    double timer = 0;

    [Signal] public delegate void IteratorCompletedEventHandler();

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timer += delta;

		if( timer > TimeBetweenIterating)
		{
			timer = 0;
			Iterate();
		}
	}

	void Iterate()
	{
		x += direction;
		if(x >= row.Length || x < 0) //if we have gone beyond the board, we are done
		{
			EmitSignalIteratorCompleted();
			QueueFree();
			return;

		}

		Cell currentCell = row[x];

		if(currentCell.ScoreFlag == Cell.ScoringFlags.CanScore || currentCell.ScoreFlag == Cell.ScoringFlags.CanScoreButFullyRestrictAfterScoring)
		{
			currentCell.ScoreBlock();
		}

	}
}

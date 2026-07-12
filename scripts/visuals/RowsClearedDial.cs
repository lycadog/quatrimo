using Godot;
using System;

public partial class RowsClearedDial : Control
{
	const double ScrollingTime = 0.2;

	[Signal] public delegate void StartedScrollingEventHandler();
    [Signal] public delegate void StoppedScrollingEventHandler();

	[Signal] public delegate void ReachedNumberEventHandler(int number);

	[Signal] public delegate void ResetFinishedEventHandler();

    [Export] VBoxContainer Container;

    int RowsCleared = 0;
    int queuedRows = 0;

    bool Scrolling = false;

	public void AddRows(int rows)
	{
        queuedRows += rows;

		int TotalRows = RowsCleared + queuedRows;

		//clamp values within 11 rows!!!
		if(TotalRows > 11)
		{
			queuedRows = TotalRows - 11;

			if(queuedRows == 0)
			{
				return;
			}
		}

        if (!Scrolling)
        {
            EmitSignalStartedScrolling();
            StartScrolling();
            return;
        }
    }

	void StartScrolling()
	{
        RowsCleared++;
        Scrolling = true;
        GetTree().CreateTween().TweenProperty(Container, "position", new Vector2(0, 16), ScrollingTime)
			.AsRelative().SetTrans(Tween.TransitionType.Cubic).Finished += DoneScrolling;
    }

	void DoneScrolling()
	{
		EmitSignalReachedNumber(RowsCleared);

		//if we have more to scroll, go and do it!
		if(queuedRows > 0)
		{
			queuedRows--;
			StartScrolling();
			return;
		}

		//if we're here we're done scrolling!

		Scrolling = false;
		EmitSignalStoppedScrolling();
	}

	public void Reset()
	{
		if(RowsCleared == 0 && !Scrolling)
		{
			EmitSignalResetFinished();
			return;
		}

		RowsCleared = 0;
		queuedRows = 0;

        Tween tween = GetTree().CreateTween();
		tween.TweenProperty(Container, "position", new Vector2(0, -175), 1)
			.SetTrans(Tween.TransitionType.Bounce).SetEase(Tween.EaseType.Out).Finished += EmitSignalResetFinished;
    }
}

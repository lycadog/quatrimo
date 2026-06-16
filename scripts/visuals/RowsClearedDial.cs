using Godot;
using System;

public partial class RowsClearedDial : Control
{
	const double ScrollingTime = 0.2;

	[Signal] public delegate void StartedScrollingEventHandler();
    [Signal] public delegate void StoppedScrollingEventHandler();
	[Signal] public delegate void ReachedNumberEventHandler(short number);

    [Export] VBoxContainer Container;

    short RowsCleared = 0;
	short queuedRows = 0;

    bool Scrolling = false;

	public void AddRow()
	{
		if(RowsCleared >= 11)
		{
			return;
		}

		if (!Scrolling)
		{
			EmitSignalStartedScrolling();
			StartScrolling();
			return;
		}
		queuedRows++;
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
			return;
		}

		RowsCleared = 0;
		queuedRows = 0;
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(Container, "position", new Vector2(0, -175), 1)
			.SetTrans(Tween.TransitionType.Bounce).SetEase(Tween.EaseType.Out);
    }
}

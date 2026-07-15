using Godot;
using System;

public partial class RowsClearedDial : Control
{
	const double ScrollingTime = 0.35;
	const double ResetTime = 1;

	[Signal] public delegate void StartedScrollingEventHandler();
    [Signal] public delegate void StoppedScrollingEventHandler();

	[Signal] public delegate void ReachedNumberEventHandler(int number);

	[Signal] public delegate void ResetFinishedEventHandler();

    [Export] VBoxContainer Container;
	[Export] AudioStreamPlayer ChimeSFX;
    [Export] AudioStreamPlayer SpecialChimeSFX;

    int RowsCleared = 0;
    int queuedRows = 0;

    bool Scrolling = false;

	public void AddRows(int rows)
	{
        queuedRows += rows;

		int TotalRows = RowsCleared + queuedRows;

		//clamp values within 11 rows!!!

		//TODO THIS NEEDS TO BE UNCAPPED!!!!
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
        queuedRows--;
        RowsCleared++;

        Scrolling = true;
        GetTree().CreateTween().TweenProperty(Container, "position", new Vector2(0, 16), ScrollingTime)
			.AsRelative().SetTrans(Tween.TransitionType.Cubic).Finished += DoneScrolling;
    }

	void DoneScrolling()
	{
		EmitSignalReachedNumber(RowsCleared);

		//change pitch here
		if (RowsCleared < 4)
		{
			ChimeSFX.PitchScale += 0.1f;
		}
		else if (RowsCleared == 4)
		{
			ChimeSFX.PitchScale = 1;
			SpecialChimeSFX.Play();
		}

		ChimeSFX.Play();

		//if we have more to scroll, go and do it!
		if(queuedRows > 0)
		{
			StartScrolling();
			return;
		}

		//if we're here we're done scrolling!

		Scrolling = false;
		EmitSignalStoppedScrolling();
	}

	public void Reset()
	{
		ChimeSFX.PitchScale = 0.4f;
		if(RowsCleared == 0 && !Scrolling)
		{
			EmitSignalResetFinished();
			return;
		}

		RowsCleared = 0;
		queuedRows = 0;

        Tween tween = GetTree().CreateTween();
		tween.TweenProperty(Container, "position", new Vector2(0, -175), ResetTime)
			.SetTrans(Tween.TransitionType.Bounce).SetEase(Tween.EaseType.Out).Finished += EmitSignalResetFinished;
    }

}

using Godot;
using System;
using System.Collections.Generic;

public partial class BoardAnimationManager : Node
{
	int CompletedAnimations;
	int TotalAnimations;

	int CompletedTurnEndAnimations;
	int TotalTurnEndAnimations;

	bool AlreadyAnimating = false;

	[Signal] public delegate void BoardAnimationCompletedEventHandler();
	[Signal] public delegate void TurnEndAnimationsCompletedEventHandler();


	public void AddTurnEndAnimation()
	{
		TotalTurnEndAnimations++;
	}

	public void TurnEndAnimationCompleted()
	{
		CompletedTurnEndAnimations++;
		if(CompletedTurnEndAnimations >= TotalTurnEndAnimations)
		{
			EmitSignalTurnEndAnimationsCompleted();
		}
	}

    public void StartEndOfTurnWaiting()
    {
        if (TotalTurnEndAnimations == 0)
		{
			EmitSignal(SignalName.TurnEndAnimationsCompleted);
		}
    }

    public void ClearAnimations()
	{
		CompletedAnimations = 0;
		TotalAnimations = 0;

		CompletedTurnEndAnimations = 0;
		TotalTurnEndAnimations = 0;
	}

	public void AnimationCreated()
	{
		TotalAnimations++;
	}

	public void AnimationCompleted()
	{
		//GD.Print($"Completed animation {CompletedAnimations}/{TotalAnimations}");
		CompletedAnimations++;

		if(CompletedAnimations == TotalAnimations)
		{
			AlreadyAnimating = false;
			EmitSignalBoardAnimationCompleted();
		}

		if(CompletedAnimations > TotalAnimations)
		{
			GD.PushError($"Completed more animations than registered animations! Completed: {CompletedAnimations}, registered: {TotalAnimations}");
		}
	}

	/// <summary>
	/// Start an animation phase, calling the provided method on completion. Can be safely ran multiple times!
	/// </summary>
	/// <param name="MethodCalledOnCompletion"></param>
	public void StartAnimating(Action MethodCalledOnCompletion)
	{
		if (AlreadyAnimating) { return; } //already animating! dont animate twice!

		AlreadyAnimating = true;
		Connect(SignalName.BoardAnimationCompleted, Callable.From(MethodCalledOnCompletion), (uint)ConnectFlags.OneShot);
	}


}

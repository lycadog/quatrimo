using Godot;
using System;
using System.Collections.Generic;

public partial class BoardAnimationManager : Node
{
	int CompletedAnimations;
	int TotalAnimations;

	bool AlreadyAnimating = false;

	[Signal] public delegate void BoardAnimationCompletedEventHandler();

	public void ClearAnimations()
	{
		CompletedAnimations = 0;
		TotalAnimations = 0;
	}

	public void OnAnimationCreated()
	{
		TotalAnimations++;
	}

	public void OnAnimationCompleted()
	{
		CompletedAnimations++;

		GD.Print($"animation {CompletedAnimations}/{TotalAnimations} completed");
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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

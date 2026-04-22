using Godot;
using System;

public partial class ScoreAnimation : Node2D
{
	[Signal] public delegate void AllVisualsFinishedEventHandler();
	[Signal] public delegate void AnimationFinishedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		StartAnimation();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	protected virtual void StartAnimation()
	{
        GpuParticles2D particles = (GpuParticles2D)GetNode("ScoreParticles");

		PointLight2D light = (PointLight2D)GetNode("PointLight2D");

		GetTree().CreateTween().TweenProperty(light, "energy", 0, 0.6);
        particles.Restart();
    }

	public void OnFinalVisualFinished()
	{
		EmitSignalAllVisualsFinished();
        QueueFree();
	}

	public void OnAnimationFinished()
	{
		EmitSignalAnimationFinished();
	}

	
}

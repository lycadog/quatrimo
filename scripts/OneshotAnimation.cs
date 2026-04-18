using Godot;
using System;

public partial class OneshotAnimation : AnimatedSprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

        GpuParticles2D particles = (GpuParticles2D)GetNode("ScoreParticles");
		particles.Restart();
		Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnParticlesFinished()
	{
		QueueFree();
	}
}

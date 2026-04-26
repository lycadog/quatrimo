using Godot;
using System;

public partial class MiniScoreNumber : Node2D
{

	[Export] Label label;
    internal double ScoreNumber = 1;
    internal Vector2 finalPosition;
	Vector2 velocity;

	//number .. - 0
	static LabelSettings BadSettings = ResourceLoader.Load<LabelSettings>("uid://dmamg3mk440hl");
	//number 1 - 2
	static LabelSettings LowSettings = ResourceLoader.Load<LabelSettings>("uid://dpwq1x5um0pgw");
    //number 3 - 7
    static LabelSettings MediumSettings = ResourceLoader.Load<LabelSettings>("uid://lbauet8ivo45");
    //number 8 - 19
    static LabelSettings HighSettings = ResourceLoader.Load<LabelSettings>("uid://bd6e47e5shbpb");
    //number 20 - 49
    static LabelSettings ReallyHighSettings = ResourceLoader.Load<LabelSettings>("uid://clmx5swa303r3");
    //number 50 - ...
    static LabelSettings CrazySettings = ResourceLoader.Load<LabelSettings>("uid://dqhxueja2hvqy");



    float TimeAlive = 0;

	[Signal] public delegate void NumberReachedScoreEventHandler(double number);

	static PackedScene NumberScene = ResourceLoader.Load<PackedScene>("uid://cfpvh6h2homdf");


	public static MiniScoreNumber GetNew(double number, Vector2 endPosition)
	{
		MiniScoreNumber newNumber = (MiniScoreNumber)NumberScene.Instantiate();

		newNumber.ScoreNumber = number;
        newNumber.finalPosition = endPosition;

        return newNumber;
	}

	 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetRandomStartingVelocity();

        SetNumber();
        label.Visible = true;
		
		//setup children
	}

    void SetNumber()
    {
        label.Text = ScoreNumber.ToString();

		label.LabelSettings = LowSettings;

		

		if(ScoreNumber < 1)
		{
			label.LabelSettings = BadSettings;
		}

        if (ScoreNumber < 3)
        {
            return;
        }

        //this is shit lol TODO
        if (ScoreNumber > 19)
		{
			Scale = new(1, 1);
			label.LabelSettings = ReallyHighSettings;
		}
		else if(ScoreNumber > 7)
		{
            Scale = new(0.9f, 0.9f);
            label.LabelSettings = HighSettings;
		}
		else if(ScoreNumber > 2)
		{
            Scale = new(0.8f, 0.8f);
            label.LabelSettings = MediumSettings;
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
		Position += velocity;
		

		if (GlobalPosition.DistanceTo(finalPosition) < 20)
		{

			Tween tween = GetTree().CreateTween();
			tween.TweenProperty(this, "scale", new Vector2(0, 0), .05).Finished += () => { QueueFree(); };

			EmitSignalNumberReachedScore(ScoreNumber);
			return;
		}

		float deltaF = (float)delta;
		TimeAlive += deltaF;

        velocity += GlobalPosition.DirectionTo(finalPosition) * deltaF * 4 * (TimeAlive * 8); //accelerate towards final position
		
		velocity = new(velocity.X - velocity.X * deltaF * 2, velocity.Y - velocity.Y * deltaF * 2); //small amount of drag
    }

	void SetRandomStartingVelocity()
	{
		velocity = new(GD.Randf() * 8 - 4, GD.Randf() * -5);
	}
}

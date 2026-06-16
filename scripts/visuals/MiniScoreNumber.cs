using Godot;
using System;

public partial class MiniScoreNumber : Node2D
{

	[Export] Label label;
    internal double Value = 1;
    internal Vector2 finalPosition;
	Vector2 velocity;

	bool isFinished = false;

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

	static LabelSettings MultSettings = ResourceLoader.Load<LabelSettings>("uid://c11xh8necm285");


    float TimeAlive = 0;

	[Signal] public delegate void NumberReachedScoreEventHandler(double number);

	static PackedScene NumberScene = ResourceLoader.Load<PackedScene>("uid://cfpvh6h2homdf");


	public static MiniScoreNumber GetNew(double number, Vector2 endPosition)
	{
		MiniScoreNumber newNumber = (MiniScoreNumber)NumberScene.Instantiate();

		newNumber.Value = number;
        newNumber.finalPosition = endPosition;

        newNumber.ChooseScoreSettings();
        return newNumber;
	}

	public static MiniScoreNumber GetNewMult(double number, Vector2 endPosition)
	{
        MiniScoreNumber newNumber = (MiniScoreNumber)NumberScene.Instantiate();

        newNumber.Value = number;
        newNumber.finalPosition = endPosition;

		newNumber.ChooseMultSettings();
        return newNumber;
    }
	 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetRandomStartingVelocity();
        label.Visible = true;
	}

    void ChooseScoreSettings()
    {
        label.Text = Value.ToString();

        if (Value < 1)
		{
			label.LabelSettings = BadSettings;
		}

        else if (Value < 4)
        {
            label.LabelSettings = LowSettings;
        }
		else if (Value < 8)
		{
            Scale = new(0.8f, 0.8f);
            label.LabelSettings = MediumSettings;
        }
		else if( Value < 20)
		{
            Scale = new(0.9f, 0.9f);
            label.LabelSettings = HighSettings;
        }
		else if (Value < 50)
		{
            Scale = new(1, 1);
            label.LabelSettings = ReallyHighSettings;
		}
		else
		{
            Scale = new(1, 1);
            label.LabelSettings = CrazySettings;
        }
    }

	void ChooseMultSettings()
	{
        //×
        label.Text = "x" + Value.ToString();

		if(Value > 0)
		{
			label.LabelSettings = BadSettings;
		}
		else if(Value > 1.5)
		{
			label.LabelSettings = MultSettings;
		}
		else if(Value > 3)
		{
			label.LabelSettings = ReallyHighSettings;
		}
		else
		{
			label.LabelSettings = CrazySettings;
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
		Position += velocity;

		if (isFinished) { return; }

		if (GlobalPosition.DistanceTo(finalPosition) < 20)
		{
			isFinished = true;

			Tween tween = GetTree().CreateTween();
			tween.TweenProperty(this, "scale", new Vector2(0, 0), .05).Finished += () => { QueueFree(); };

			EmitSignalNumberReachedScore(Value);
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

using Godot;
using System;
using System.Collections.Generic;

[GlobalClass, Icon("res://texture/icon/pieceicon.png")]
public partial class FallingPiece : Node2D
{
	double fallingCounter = -0.2;
	double placementCounter = -0.2;
	double DownHeldTime = 0;
	double LeftCooldown = 0;
	double RightCooldown = 0;
	double PostRotationLockout = 0;

	const double TimeBeforeFalling = 0.4;
	const double TimeBeforePlacement = 0.8;
	const int FastfallSpeed = 80;

	bool Falling = false;

	public Vector2I Dimensions;

	public List<Block> Blocks = [];

	public float TotalSlamOffset = 10000;

    public FallingPiece(Block[] blocks, Vector2I dimensions)
    {
        LinkBlocks(blocks);
        Dimensions = dimensions;
    }

    [Signal] public delegate void OnPiecePlacementEventHandler(FallingPiece piece);

   
	/// <summary>
	/// Links the blocks to the newly-created piece
	/// </summary>
	/// <param name="blocks"></param>
	void LinkBlocks(Block[] blocks)
	{
		foreach(var block in blocks)
		{
			Blocks.Add(block);
			AddChild(block);

			block.TreeExiting += () => OnBlockExitingTree(block);
		}
		//do other stuff maybe
	}

    public virtual void Play()
    {
        ForceUpdateTransform();
        Falling = true;
		foreach(var block in Blocks)
		{
			block.Play();
		}
        SetNewSlamPosition();
    }

    public virtual void Place()
    {
		Falling = false;
		foreach(var block in Blocks)
		{
			block.Place();
		}

        QueueFree();
        EmitSignalOnPiecePlacement(this);
    }

	protected void OnBlockExitingTree(Block block)
	{
		if (Falling)
		{
            Blocks.Remove(block);
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{

		if(fallingCounter >= TimeBeforeFalling)
		{
			if (CanMoveDown())
			{
				//fall!

				Move(0, 10); // move down!
				fallingCounter = 0;
				placementCounter = 0;

                SetNewSlamPosition();
                return;
			}
			else
			{
				//if we can't fall, then let's try to place!
				if(placementCounter >= TimeBeforePlacement)
				{
					Place();

					return;
				}

			}
		}

        ProcessInput(delta); //this is after the rest because sometimes this needs to end the state
		//we don't want to run the above code if the state is ended, so we run after!

        fallingCounter += delta;
		placementCounter += delta;

		SetNewSlamPosition();
    }

    public void ProcessInput(double delta)
	{

        if (Input.IsActionJustPressed("SlamPiece"))
		{
			SlamAndPlace();
			return;
		}

        if (Input.IsActionPressed("Down")) 
        {
            if (Input.IsActionJustPressed("Down")) //When down is pressed immediately move down, then 
            {
                fallingCounter += 1;
                DownHeldTime = 0;
            }

            DownHeldTime += delta;

            if (DownHeldTime >= 0.14) //After it's held for a bit move down some more
            {
                fallingCounter += delta * FastfallSpeed;
            }
        }

        else if (Input.IsActionJustReleased("Down")) //Set the timer to negative when we release so we have more precision
        {
            fallingCounter = -.2;
			placementCounter = -.2;
        }

		else if (Input.IsActionPressed("Up"))
		{
			fallingCounter = 0;
		}

        if (Input.IsActionPressed("Left"))
		{
			LeftCooldown -= delta;

			if (Input.IsActionJustPressed("Left"))
			{
				AttemptMoveLeft();
				LeftCooldown = .1;
				return;
			}

			else if (LeftCooldown <= 0)
			{
				AttemptMoveLeft();
				LeftCooldown = .034;
				return;
			}
		}
		
		else if (Input.IsActionPressed("Right"))
		{
			RightCooldown -= delta;
			if (Input.IsActionJustPressed("Right"))
			{
				AttemptMoveRight();
				RightCooldown = .1;
				return;
			}

			else if (RightCooldown <= 0)
			{
				AttemptMoveRight();
				RightCooldown = .034;
				return;
			}
		}

        if (Input.IsActionJustPressed("RotateLeft"))
        {
            AttemptLeftRotation();
        }

        else if (Input.IsActionJustPressed("RotateRight"))
        {
            AttemptRightRotation();
        }

    }

	public void SlamAndPlace()
	{
		Position = new(Position.X, Position.Y + TotalSlamOffset);
		Place();
	}

    public void SetNewSlamPosition()
    {
        TotalSlamOffset = 10000;

		//a bit inefficient to run this constantly
		//ultimately doesn't matter much
		//TODO maybe figure out a better way

        foreach (var block in Blocks)
        {
			block.UpdateSlamPosition();
            TotalSlamOffset = Math.Min(block.SlamOffset, TotalSlamOffset);
            
        }

        foreach (var block in Blocks)
        {
            block.UpdateSlamSprite(TotalSlamOffset);
        }
    }

    public void Move(float xOffset, float yOffset)
	{
		Position = new Vector2(Position.X + xOffset, Position.Y + yOffset);
	}

	public void AttemptMoveLeft()
	{
		if (CanMoveLeft()) { Move(-10, 0); }
	}

    public void AttemptMoveRight()
    {
        if (CanMoveRight()) { Move(10, 0); }
    }

	public void AttemptLeftRotation()
	{
		if (CanRotateNegative()) { Rotate(-1); }
	}

    public void AttemptRightRotation()
    {
        if (CanRotatePositive()) { Rotate(1); }
    }

    void Rotate(int direction)
	{
		foreach(var block in Blocks)
		{
			block.Rotate(direction);
		}
	}

	

    protected bool CanMoveLeft()
    {
        foreach (var block in Blocks)
        {
            if (!block.CanMoveLeft) { return false; }
        }

        return true;
    }
    protected bool CanMoveDown()
	{
		foreach(var block in Blocks)
		{
			if (!block.CanMoveDown) { return false; }
		}

		return true;
	}
    protected bool CanMoveRight()
    {
        foreach (var block in Blocks)
        {
            if (!block.CanMoveRight) { return false; }
        }

        return true;
    }
    protected bool CanRotateNegative()
    {
        foreach (var block in Blocks)
        {
            if (!block.CanRotateNegative) { return false; }
        }

        return true;
    }
    protected bool CanRotatePositive()
    {
        foreach (var block in Blocks)
        {
            if (!block.CanRotatePositive) { return false; }
        }

        return true;
    }

    void BindBlockEvents(Block block)
	{

	}

	void UnbindBlockEvents(Block block)
	{

	}


}

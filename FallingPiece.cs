using Godot;
using System;
using System.Collections.Generic;

[GlobalClass, Icon("res://texture/icon/pieceicon.png")]
public partial class FallingPiece : Node2D
{
	double fallingCounter = -0.4;
	double placementCounter = -0.4;
	double DownHeldTime = 0;
	double LeftCooldown = 0;
	double RightCooldown = 0;

	[Export] public Block[] blocks;

	public float TotalSlamOffset = 10000;

	[Signal] public delegate void OnPiecePlacementEventHandler();

   
	/// <summary>
	/// Links the blocks to the newly-created piece
	/// </summary>
	/// <param name="blocks"></param>
	public void LinkBlocks(Block[] blocks)
	{
		this.blocks = blocks;

		foreach(var block in blocks)
		{
			AddChild(block);
		}
		//do other stuff maybe
	}

    public virtual void Play()
    {

    }

    public virtual void Place()
    {
        //unlink all the blocks, run their events etc, then delete ourself!
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if(fallingCounter >= .6)
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
				if(placementCounter >= 1)
				{
					Place();

					fallingCounter = 0;
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

		if (Input.IsActionPressed("LeftMove"))
		{
			LeftCooldown -= delta;

			if(Input.IsActionJustPressed("LeftMove"))
			{
				AttemptMoveLeft();
				LeftCooldown = .1;
			}

			if(LeftCooldown <= 0)
			{
				AttemptMoveLeft();
				LeftCooldown = .03;
			}

		}

		else if (Input.IsActionPressed("RightMove"))
		{
            RightCooldown -= delta;
            if (Input.IsActionJustPressed("RightMove"))
            {
                AttemptMoveRight();
                RightCooldown = .1;
            }

			if(RightCooldown <= 0)
			{
				AttemptMoveRight();
				RightCooldown = .03;
			}
        }

		if (Input.IsActionPressed("DownMove"))
		{
			if (Input.IsActionJustPressed("DownMove"))
			{
				fallingCounter += 1;
				DownHeldTime = 0;
			}

			DownHeldTime += delta;

			if (DownHeldTime >= 0.14)
			{
				fallingCounter += delta * 20;
			}

		}
		else if (Input.IsActionJustReleased("DownMove"))
		{
			fallingCounter = -.2;
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

        foreach (var block in blocks)
        {
			block.UpdateSlamPosition();
            TotalSlamOffset = Math.Min(block.SlamOffset, TotalSlamOffset);
            
        }

        foreach (var block in blocks)
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
		foreach(var block in blocks)
		{
			block.Rotate(direction);
		}
	}

	

    protected bool CanMoveLeft()
    {
        foreach (var block in blocks)
        {
            if (!block.CanMoveLeft) { return false; }
        }

        return true;
    }
    protected bool CanMoveDown()
	{
		foreach(var block in blocks)
		{
			if (!block.CanMoveDown) { return false; }
		}

		return true;
	}
    protected bool CanMoveRight()
    {
        foreach (var block in blocks)
        {
            if (!block.CanMoveRight) { return false; }
        }

        return true;
    }
    protected bool CanRotateNegative()
    {
        foreach (var block in blocks)
        {
            if (!block.CanRotateNegative) { return false; }
        }

        return true;
    }
    protected bool CanRotatePositive()
    {
        foreach (var block in blocks)
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

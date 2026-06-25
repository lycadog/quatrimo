using Godot;
using System;
using System.Collections.Generic;

public partial class FallingPiece : Node2D
{
	public Vector2I BoardPos
	{
		get => CollisionData.BoardPos;
        set
        {
            CollisionData.BoardPos = value;
            Position = new Vector2(BoardPos.X, -BoardPos.Y) * 10 + Offset;
        }
        //set method that auto updates our position
    }
    public Vector2I Dimensions;

    public FallingPiece(Block[] newBlocks, Vector2I dimensions)
    {
        foreach(var block in newBlocks)
        {
            AddBlock(block);

            //check if we should offset! we offset if any piece has a non-integer boardposition (5 instead of 10)
            if(block.Position.X % 10 != 0)
            {
                Offset = new(5, 5);
            }
        }
        Dimensions = dimensions;

        SfxMove = new() { Stream = MoveBlip, VolumeDb = -16.0f };
        AddChild(SfxMove);
    }

    static AudioStreamWav MoveBlip = ResourceLoader.Load<AudioStreamWav>("uid://bklrddjdj5m80");

    Vector2 Offset = new(0, 0);

	public List<Block> Blocks = [];
    protected BoardCollisionData CollisionData = new();

    AudioStreamPlayer SfxMove;

    bool Falling = false;
    double fallingCounter = -0.2;
    double placementCounter = -0.2;
    double DownHeldTime = 0;
    double LeftCooldown = 0;
    double RightCooldown = 0;

    double SlamLockout = 0.1;

    const double TimeBeforeFalling = 0.6;
    const double TimeBeforePlacement = 1.2;
    const int FastfallSpeed = 80;

    [Signal] public delegate void PiecePlacedEventHandler();

    public void AddBlock(Block block)
    {
        AddChild(block);
        Blocks.Add(block);
        block.Connect(Block.SignalName.Deleted, new(this, MethodName.RemoveBlock), (uint)ConnectFlags.AppendSourceObject);
    }

    public void RemoveBlock(Block block)
    {
        //todo: consider recalculating dimensions when we add or remove blocks
        if (Falling)
        {
            Blocks.Remove(block);

            if(Blocks.Count == 0)
            {
                Place();
            }
            UpdateCollision();
        }
    }

    public void StartFall(Vector2I StartingPosition)
	{
        if(Offset.X != 0)
        {
            //if we're weird and are using an offset: we are +1 +1 from our intended position, so we need to change it!
            StartingPosition += new Vector2I(-1, 0);
        }
        BoardPos = StartingPosition;
        Falling = true;
        UpdateCollision();
	}

    public void Place()
    {
        Falling = false;
        QueueFree();

        foreach (var block in Blocks)
        {
            block.Disconnect(Block.SignalName.Deleted, new(this, MethodName.RemoveBlock));
            block.Place();
        }

        EmitSignalPiecePlaced();
    }

    public void UpdateCollision()
    {
        if (!Falling || IsQueuedForDeletion())
        {
            return;
        }

        //the force update transforms may not be needed, but they most likely are
        ForceUpdateTransform();

        foreach (var block in Blocks)
        {
            block.ForceUpdateTransform();
            block.UpdateFallingCollision(Position);
        }

        CollisionData.UpdatePiece(Blocks, Position);

        foreach (var block in Blocks)
        {
            block.UpdateSlamSprite();
        }
    }

    void MoveTo(int x, int y)
    {
        BoardPos = new(x, y);
        UpdateCollision();
    }

    void Move(int x, int y)
    {
        BoardPos += new Vector2I(x, y);

        UpdateCollision();
    }

    void AttemptMoveLeft()
    {
        if (CollisionData.LeftMoveValid)
        {
            Move(-1, 0);
        }
    }
    void AttemptMoveRight()
    {
        if (CollisionData.RightMoveValid)
        {
            Move(1, 0);
        }
    }

    void AttemptRotation(int direction)
    {
        if (direction != -1 && direction != 1)
        {
            GD.PushError($"Rotation error on falling piece! Attempted to use invalid direction of {direction}");
            return;
        }

        int offsetIndex = -1;
        bool[] ValidRotations;

        if (direction == -1) //this kinda sucks lol
        {
            ValidRotations = CollisionData.ValidLeftRotations;
        }
        else
        {
            ValidRotations = CollisionData.ValidRightRotations;
        }

        //Find rotation offset to use
        for (int i = 0; i < BoardCollisionData.RotationOffsets.Length; i++)
        {
            if (ValidRotations[i])
            {
                //we've found our proper offset so let's stop looking
                offsetIndex = i;
                break;
            }
        }

        //no valid rotations, so we return
        if (offsetIndex == -1) { return; }

        //rotate everything
        foreach (var block in Blocks)
        {
            block.Rotate(direction);
        }

        Vector2I offset = new(
            BoardCollisionData.RotationOffsets[offsetIndex].X * direction,
            BoardCollisionData.RotationOffsets[offsetIndex].Y);

        BoardPos += offset;
        //now we actually use the offset

        UpdateCollision();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
        if (fallingCounter >= TimeBeforeFalling)
        {
            if (CollisionData.DownMoveValid)
            {
                //fall!

                Move(0, -1); // move down!
                fallingCounter = 0;
                placementCounter = 0;

                return;
            }
            else
            {
                //if we can't fall, then let's try to place!
                if (placementCounter >= TimeBeforePlacement)
                {
                    Place();

                    return;
                }

            }
        }


        ProcessInput(delta); //this is after the rest because sometimes this needs to end the state
                             //we don't want to run the above code if the state is ended, so we run after!

        SlamLockout -= delta;
        fallingCounter += delta;
        placementCounter += delta;
    }

    void ProcessInput(double delta)
    {

        if (Input.IsActionJustPressed("SlamPiece") && SlamLockout < 0)
        {
            Move(0, -CollisionData.SlamYOffset);
            Place();
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
            placementCounter = 0;
        }

        if (Input.IsActionPressed("Left"))
        {
            LeftCooldown -= delta;

            if (Input.IsActionJustPressed("Left"))
            {
                AttemptMoveLeft();
                LeftCooldown = .11;
                return;
            }

            else if (LeftCooldown <= 0)
            {
                AttemptMoveLeft();
                LeftCooldown = .036;
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
                RightCooldown = .036;
                return;
            }
        }

        if (Input.IsActionJustPressed("RotateLeft"))
        {
            AttemptRotation(-1);
        }

        else if (Input.IsActionJustPressed("RotateRight"))
        {
            AttemptRotation(1);
        }
    }

}

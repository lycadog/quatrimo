using Godot;
using System;

public partial class FallingEnemyBlock : Node2D
{
    public int BoardX;

	Block block;
	bool Falling = false;

	//Use this offset when a solid falling block is below us! We should place above them so lift up the preview
	int AttackSlamOffset = 0;
    float velocity = 0;
    const float acceleration = 8;

    public event Action FallingStarted;
    public event Action FallingFinished;

    public FallingEnemyBlock(Block block, int boardX, int attackSlamOffset)
    {
        this.block = block;
        AddChild(block);
        BoardX = boardX;
        AttackSlamOffset = attackSlamOffset;

        block.BoardPos = new(boardX, BoardAccessor.CellDimensions.Y - 8 + attackSlamOffset);

        //disable the block to prevent collision until we fall
        block.ProcessMode = ProcessModeEnum.Disabled;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{

		if (Falling)
		{
			Fall(delta);
            return;
		}

		UpdateSlamPreview();
	}

    public void StartFall()
    {
        if (Falling)
        {
            return;
        }

        //we use visible here and invoke the falling event as certain falling blocks have sprites attached to them
        //like the +3 when some are hidden, and the arrows below the base block
        //this event triggers those to delete themselves
        //blocks above y2 are hidden so we need to make sure those are revealed
        Visible = true;
        FallingStarted?.Invoke();

        //reenable the block
        block.ProcessMode = ProcessModeEnum.Inherit;

        Falling = true;
        Modulate = new(1, 1, 1, 1);
        block.HideSlamSprite();
    }

    public void SetNormalVisibility()
    {
        Modulate = new(1, 1, 1, 0.8f);
    }

    public void SetLowVisibility()
    {
        Modulate = new(1, 1, 1, 0.4f);
    }

    public void SetPreviewSprite()
    {
        block.SetEnemySlamSprite();
    }

    void UpdateSlamPreview()
	{
		block.CollisionData.UpdateSlamOffset();
		block.CollisionData.SlamYOffset -= AttackSlamOffset;
        block.UpdateSlamSprite();
    }

	void Fall(double delta)
	{
        velocity += (float)(delta * acceleration);

        //accellerate downwards
        Position = new Vector2(Position.X, Position.Y + velocity);

        block.CollisionData.UpdateFallingEnemyBlock(Position);

        //if we can't move down: place!
        if (!block.CollisionData.DownMoveValid)
        {
            block.Place();
            FallingFinished.Invoke();

            Falling = false;
            QueueFree();
        }
    }

}

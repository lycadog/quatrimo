using Godot;
using System;

public partial class FallingEnemyBlock : Node2D
{
    public int BoardX;

	public Block block;
	bool Falling = false;

	
	int AttackSlamOffset = 0;
    float velocity = 0;
    const float acceleration = 8;

    public event Action FallingStarted;
    public event Action<bool> FallingFinished;

    public FallingEnemyBlock(Block block, int boardX, int blocksBeneath, int solidBlocksBeneath)
    {
        this.block = block;
        AddChild(block);

        block.Deleted += BlockDeleted;

        BoardX = boardX;

        //Use this offset when a solid falling block is below us! We should place above them so lift up the preview
        AttackSlamOffset = solidBlocksBeneath;

        block.BoardPos = new(boardX, BoardAccessor.CellDimensions.Y - 8 + blocksBeneath);

        //disable the block to prevent collision until we fall
        block.ToggleCollision(false);
    }

    public void SetTexture(Rect2 region)
    {
        block.SetTexture(region);
    }

    public void SetColor(Color color)
    {
        block.SetColor(color);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
        if (IsQueuedForDeletion())
        {
            return;
        }

		if (Falling)
		{
			Fall(delta);
            return;
		}

		UpdateSlamPreview();
	}

    void BlockDeleted()
    {
        FallingFinished.Invoke(false);
        QueueFree();
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
        block.ToggleCollision(true);

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
            block.Deleted -= BlockDeleted;

            block.Place();
            FallingFinished.Invoke(true);

            Falling = false;
            QueueFree();
        }
    }

}

using Godot;
using System;

public partial class FallingEnemyBlock : Node2D
{
	Block block;
	bool isFalling = false;

	//Use this offset when a solid falling block is below us! We should place above them so lift up the preview
	int AttackSlamOffset = 0;
    float velocity = 0;

    public FallingEnemyBlock(Block block, int attackSlamOffset)
    {
        this.block = block;
        AddChild(block);
        AttackSlamOffset = attackSlamOffset;

        //disable the block to prevent collision until we fall
        block.ProcessMode = ProcessModeEnum.Disabled;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{

		if (isFalling)
		{
			Fall(delta);
		}

		UpdateSlamPreview();
		
	}

    public void StartFall()
    {
        if (isFalling)
        {
            return;
        }

        //reenable the block
        block.ProcessMode = ProcessModeEnum.Inherit;

        isFalling = true;
        Modulate = new(1, 1, 1, 1);
        block.HideSlamSprite();
    }

    public void ToggleLowVisibility(bool LowerVisiblity)
    {
        if (LowerVisiblity)
        {
            Modulate = new(1, 1, 1, 0.4f);
        }
        else
        {
            Modulate = new(1, 1, 1, 0.8f);
        }
    }

	void UpdateSlamPreview()
	{
		block.CollisionData.UpdateSlamOffset();
		block.CollisionData.SlamYOffset -= AttackSlamOffset;
    }

	void Fall(double delta)
	{
        velocity += (float)(delta * 4);

        //accellerate downwards
        Position = new Vector2(Position.X, Position.Y - velocity);

        block.CollisionData.UpdateFallingEnemyBlock(Position);

        //if we can't move down: place!
        if (!block.CollisionData.DownMoveValid)
        {
            block.Place();
            QueueFree();
        }
    }

}

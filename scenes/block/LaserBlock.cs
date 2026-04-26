using Godot;
using System;

public partial class LaserBlock : Block
{
    static readonly PackedScene BurnAnimation = ResourceLoader.Load<PackedScene>("uid://by12ay4yqa5lo");

    int burnsLeft = 4;

    public override void AttemptedToPlaceIntoBlock(Block placedBlock)
    {
        DeleteBlock(placedBlock);
    }

    public override void CollidedWithBlockWhileFalling(Block placedBlock)
    {
        DeleteBlock(placedBlock);
    }

    public override void CollidedWithBlockWhilePlaced(Block fallingBlock)
    {
        DeleteBlock(fallingBlock);
    }

    public override void FallingBlockAttemptingPlacementOnUs(Block fallingBlock)
    {
        DeleteBlock(fallingBlock);
    }

    void DeleteBlock(Block block)
    {
        
        block.QueueFree();
        EmitSignalCreateAnimation(block.GlobalPosition, (ScoreAnimation)BurnAnimation.Instantiate());

        if (burnsLeft == 0)
        {
            QueueFree();
            EmitSignalCreateAnimation(GlobalPosition, (ScoreAnimation)BurnAnimation.Instantiate());
            return;
        }

        burnsLeft--;
    }
}

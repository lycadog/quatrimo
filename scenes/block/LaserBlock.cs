using Godot;
using System;

public partial class LaserBlock : Block
{
    public override void AttemptedToPlaceIntoBlock(Block placedBlock)
    {
        placedBlock.QueueFree();
    }

    public override void CollidedWithBlockWhileFalling(Block placedBlock)
    {
        placedBlock.QueueFree();
    }

    public override void CollidedWithBlockWhilePlaced(Block fallingBlock)
    {
        fallingBlock.QueueFree();
    }

    public override void FallingBlockAttemptingPlacementOnUs(Block fallingBlock)
    {
        fallingBlock.QueueFree();
    }
}

using Godot;
using System;

public partial class VoidBlock : Block
{
    static readonly PackedScene VoidCutAnimation = ResourceLoader.Load<PackedScene>("uid://dimdmpusy70a1");

    public override void CollidedWithBlock(Block otherBlock, bool attemptingPlacement = false)
    {
        BoardAccessor.DestroyBlock(otherBlock);
        BoardAccessor.AddNodeToBoard((Node2D)VoidCutAnimation.Instantiate(), otherBlock.BoardPos);
    }

    public override void Place(bool DoPlacementFlash = true)
    {
        BoardAccessor.AddNodeToBoard((Node2D)VoidCutAnimation.Instantiate(), BoardPos);
        BoardAccessor.DestroyBlock(BoardPos);
        QueueFree();
    }

}

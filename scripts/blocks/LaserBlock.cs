using Godot;

public partial class LaserBlock : Block
{
    static readonly PackedScene BurnAnimation = ResourceLoader.Load<PackedScene>("uid://by12ay4yqa5lo");

    int burnsLeft = 4;

    public override void CollidedWithBlock(Block otherBlock, bool attemptingPlacement = false)
    {
        DeleteBlock(otherBlock);
    }

    void DeleteBlock(Block block)
    {
        BoardAccessor.DestroyBlock(block);
        BoardAccessor.AddSprite((Node2D)BurnAnimation.Instantiate(), block.BoardPos);

        if (burnsLeft == 0)
        {
            Delete();
            return;
        }

        burnsLeft--;
    }
}

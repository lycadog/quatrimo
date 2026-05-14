using Godot;
using System;

public partial class FireBlock : Block
{
    [Export] Area2D burnArea;

    static readonly PackedScene BurnAnimation = ResourceLoader.Load<PackedScene>("uid://by12ay4yqa5lo");

    int turnsLeft;
    public override void CollidedWithBlock(Block otherBlock, bool attemptingPlacement = false)
    {
        if(IsPlaced && attemptingPlacement)
        {
            Delete();
        }
    }

    protected override void RunPlacementBehavior()
    {
        turnsLeft = GD.RandRange(2, 12);
    }

    protected override void RunTickBehavior()
    {
        if(turnsLeft == 0)
        {

            BoardAccessor.AddSprite((Node2D)BurnAnimation.Instantiate(), Position);
            Delete();
            return;
        }

        turnsLeft--;
        if(GD.Randf() > .5f)
        {

            var areas = burnArea.GetOverlappingAreas();
            float multiSpreadChance = .8f;

            foreach (var area in areas)
            {
                if (area is Block block && area is not FireBlock)
                {

                    FireBlock newBlock = (FireBlock)BagBlock.GetNewBlock(BlockType.Fire);
                    newBlock.BoardPos = block.BoardPos;
                    block.Delete(); //overwrite a nearby block

                    BoardAccessor.PlaceBlockDirectlyToBoard(newBlock, false);

                    BoardAccessor.AddSprite((Node2D)BurnAnimation.Instantiate(), block.BoardPos);

                    if(GD.Randf() > multiSpreadChance)
                    {
                        break;
                    }
                    multiSpreadChance -= .2f;
                }
            }

        }

    }

}

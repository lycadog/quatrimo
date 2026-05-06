using Godot;
using System;

public partial class FireBlock : Block
{
    [Export] Area2D burnArea;

    static PackedScene fireBlockScene = ResourceLoader.Load<PackedScene>("uid://vtx0og4n4df0");
    static readonly PackedScene BurnAnimation = ResourceLoader.Load<PackedScene>("uid://by12ay4yqa5lo");

    int turnsLeft;
    public override void FallingBlockAttemptingPlacementOnUs(Block fallingBlock)
    {
        QueueFree();
    }

    protected override void OnPlaced()
    {
        turnsLeft = GD.RandRange(2, 12);
    }

    protected override void DestructiveTickBlock()
    {
        if(turnsLeft == 0)
        {

            BlockCommandHandler.AddAnimationToBoard((Node2D)BurnAnimation.Instantiate(), GlobalPosition);
            QueueFree();
            return;
        }

        turnsLeft--;
        if(GD.Randf() > .5f)
        {

            var areas = burnArea.GetOverlappingAreas();
            float mutliSpreadChance = .8f;

            foreach (var area in areas)
            {
                
                if (area is Block block && area is not FireBlock)
                {

                    FireBlock newBlock = (FireBlock)fireBlockScene.Instantiate();

                    newBlock.boardPos = block.boardPos;

                    block.QueueFree(); //overwrite a nearby block
                    BlockCommandHandler.AddBlockToBoard(newBlock);

                    newBlock.WhiteFlashSprite.Visible = false;

                    BlockCommandHandler.AddAnimationToBoard((Node2D)BurnAnimation.Instantiate(), block.GlobalPosition);

                    if(GD.Randf() > mutliSpreadChance)
                    {
                        break;
                    }
                    mutliSpreadChance -= .2f;
                }
            }

        }

    }

}

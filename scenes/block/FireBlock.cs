using Godot;
using System;

public partial class FireBlock : Block
{
    [Export] Area2D burnArea;

    static PackedScene fireBlockScene = ResourceLoader.Load<PackedScene>("uid://vtx0og4n4df0");
    static readonly PackedScene BurnAnimation = ResourceLoader.Load<PackedScene>("uid://by12ay4yqa5lo");

    int turnsLeft;
    int burnCooldown = 0;

    public override void FallingBlockAttemptingPlacementOnUs(Block fallingBlock)
    {
        QueueFree();
    }

    protected override void OnPlaced()
    {
        turnsLeft = GD.RandRange(3, 12);
    }

    protected override void TickBlock()
    {
        if(turnsLeft == 0)
        {
            EmitSignalCreateAnimation(GlobalPosition, (ScoreAnimation)BurnAnimation.Instantiate());
            QueueFree();
            return;
        }

        turnsLeft--;
        burnCooldown--;
        if(GD.Randf() > .8f && burnCooldown < 0)
        {

            var areas = burnArea.GetOverlappingAreas();

            foreach(var area in areas)
            {
                if (area is Block block && area is not FireBlock)
                {

                    FireBlock newBlock = (FireBlock)fireBlockScene.Instantiate();
                    AddChild(newBlock);
                    newBlock.GlobalPosition = block.GlobalPosition;
                    newBlock.boardX = block.boardX; newBlock.boardY = block.boardY;
                    newBlock.ForceUpdateTransform();

                    GD.Print($"newblock pos: {newBlock.boardX}, {newBlock.boardY}");

                    block.QueueFree(); //overwrite a nearby block
                    EmitSignalCreatedBlock(newBlock);
                    newBlock.Place();

                    newBlock.WhiteFlashSprite.Visible = false;

                    EmitSignalCreateAnimation(block.GlobalPosition, (ScoreAnimation)BurnAnimation.Instantiate());

                    burnCooldown = GD.RandRange(2, 6);
                }
            }
            //spread!
            //add code here to overwrite a random adjacent block
        }

    }

}

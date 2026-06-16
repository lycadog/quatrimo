using Godot;
using System;

public partial class FallingBlocksAttack : Attack
{

    FallingEnemyBlock[] Blocks;

    const double BlockDropDelay = 0.05;

    protected override void SetupAttack()
    {

    }

    public override void ExecuteAttack()
    {
        Tween tween = GetTree().CreateTween().SetParallel(true);

        //drop all the blocks, incorporating a delay based on X position
        foreach(var block in Blocks)
        {
            tween.TweenCallback(Callable.From(block.StartFall)).SetDelay( block.BoardX * BlockDropDelay);
        }
    }
}

using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FallingBlocksAttack : Attack
{
    [Export] int MinBlocksDropped = 4, MaxBlocksDropped = 16;

    [Export] Array<BlockType> BlockTypes;

    FallingEnemyBlock[] Blocks;

    int BlocksDoneFalling;

    const double BlockDropDelay = 0.05;

    protected override void SetupAttack()
    {
        int DroppedBlocksCount = GD.RandRange(MinBlocksDropped, MaxBlocksDropped-1);

        int[] randomXValues = new int[DroppedBlocksCount];
        int[] RecurringXValueCount = new int[BoardAccessor.CellDimensions.X];

        Blocks = new FallingEnemyBlock[DroppedBlocksCount];

        for(int i = 0; i < randomXValues.Length; i++)
        {
            int x = GD.RandRange(0, BoardAccessor.CellDimensions.X - 1);

            BlockType type = BlockTypes[GD.RandRange(0, BlockTypes.Count - 1)];

            int y = RecurringXValueCount[x];

            //todo: we need a SEPERATE VALUE AND ARRAY for our y position, and for solid enemy blocks below us.
            //THESE ARE DIFFERENT THINGS!!!!

            var fallingBlock = BoardAccessor.CreateFallingEnemyBlock(x, y, type);

            fallingBlock.FallingFinished += BlockFinishedFalling;

            Blocks[i] = fallingBlock;

            randomXValues[i] = x;
            RecurringXValueCount[x]++;

        }

    }


    public override void ExecuteAttack()
    {
        Tween tween = GetTree().CreateTween().SetParallel(true);

        //drop all the blocks, incorporating a delay based on X position
        foreach(var block in Blocks)
        {
            tween.TweenCallback(Callable.From(block.StartFall)).SetDelay(block.BoardX * BlockDropDelay);
        }
    }

    void BlockFinishedFalling()
    {
        BlocksDoneFalling++;

        GD.Print($"enemy blocks {BlocksDoneFalling}/{Blocks.Length} placed");

        if(BlocksDoneFalling == Blocks.Length)
        {
            BlocksDoneFalling = 0;
            EmitSignalExecutionFinished();
        }
    }
}

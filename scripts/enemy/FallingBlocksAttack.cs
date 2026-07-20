using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FallingBlocksAttack : Attack
{
    [Export] int MinBlocksDropped = 4, MaxBlocksDropped = 16;
    [Export] Array<BlockType> BlockTypes;

    [Export] Rect2 TextureRegion = new(0, 30, 10, 10);
    [Export] Color BlockColor = Colors.White;

    FallingEnemyBlock[] Blocks;
    int BlocksDoneFalling;

    const double BlockDropDelay = 0.08;
    static readonly CompressedTexture2D Atlas = ResourceLoader.Load<CompressedTexture2D>("uid://dib0bjbdg8no8");
    static readonly AudioStreamWav PlaceBlip = ResourceLoader.Load<AudioStreamWav>("uid://tj2a8wxpeqq4");
    static Rect2 BlankSquareRect = new(70, 0, 10, 10);
    static Rect2 DropIconRect = new(50, 10, 10, 10);

    AudioStreamPlayer PlaceSFX;

    protected override void SetupAttack()
    {
        PlaceSFX.PitchScale = 1;

        int DroppedBlocksCount = GD.RandRange(MinBlocksDropped, MaxBlocksDropped-1);

        //int[] randomXValues = new int[DroppedBlocksCount]; unused right now, use for more complicated settings later TODO

        int[] BlocksPerXValue = new int[BoardAccessor.CellDimensions.X];
        int[] SolidBlocksPerXValue = new int[BoardAccessor.CellDimensions.X];

        Blocks = new FallingEnemyBlock[DroppedBlocksCount];

        for(int i = 0; i < DroppedBlocksCount; i++)
        {
            int x = GD.RandRange(0, BoardAccessor.CellDimensions.X - 1);

            BlockType type = BlockTypes[GD.RandRange(0, BlockTypes.Count - 1)];

            int blocksBeneath = BlocksPerXValue[x];
            int solidBlocksBeneath = SolidBlocksPerXValue[x];

            var fallingBlock = BoardAccessor.CreateFallingEnemyBlock(x, blocksBeneath, solidBlocksBeneath, type);

            fallingBlock.FallingFinished += BlockFinishedFalling;

            fallingBlock.SetTexture(TextureRegion);
            fallingBlock.SetColor(BlockColor);
            //if first block
            if (blocksBeneath == 0)
            {
                MakeFallingSprites(fallingBlock);
            }

            Blocks[i] = fallingBlock;

            BlocksPerXValue[x]++;
            if (fallingBlock.block.SolidWhenFalling)
            {
                SolidBlocksPerXValue[x]++;
            }
        }

    }

    void MakeFallingSprites(FallingEnemyBlock FallingBlock)
    {
        var BlankSquareSprite = new Sprite2D
        {
            Texture = Atlas,
            RegionRect = BlankSquareRect,
            RegionEnabled = true,
            SelfModulate = new(0, 0, 0),
            ShowBehindParent = true
        };

        var DropIconSprite = new Sprite2D
        {
            Texture = Atlas,
            RegionRect = DropIconRect,
            RegionEnabled = true,
            Position = new(0, 10)
        };

        FallingBlock.AddChild(BlankSquareSprite);
        FallingBlock.AddChild(DropIconSprite);

        FallingBlock.FallingStarted += BlankSquareSprite.Free;
        FallingBlock.FallingStarted += DropIconSprite.Free;
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

    void BlockFinishedFalling(bool PlaySound)
    {
        if (PlaySound)
        {
            PlaceSFX.Play();
            PlaceSFX.PitchScale += 0.05f;
        }

        BlocksDoneFalling++;

        GD.Print($"enemy blocks {BlocksDoneFalling}/{Blocks.Length} placed");

        if(BlocksDoneFalling == Blocks.Length)
        {
            BlocksDoneFalling = 0;
            EmitSignalExecutionFinished();
        }
    }

    public override void _EnterTree()
    {
        PlaceSFX = new()
        {
            Stream = PlaceBlip,
        };

        PlaceSFX.VolumeDb = -10;

        AddChild(PlaceSFX);
    }
}

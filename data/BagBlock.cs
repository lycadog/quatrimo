

using Godot;
using System;

public class BagBlock(BlockType Type, Vector2I Position)
{
    public CardBlockSprite GetCardPreviewSprite(float h, float s, float v)
    {
        var sprite = (CardBlockSprite) CardBlocks [(int)Type] .Instantiate();

        sprite.Position = Position * 7;

        sprite.SetColor(h, s, v);

        return sprite;
    }

    public Block GetNewBlock(float h, float s, float v, Rect2 rect)
    {
        Block block = GetNewBlock(Type);
        block.Position = Position * 10;
        block.SetColor(h, s, v);
        block.SetTexture(rect);
        //todo: this may need a local pos, but i dont think so

        return block;
    }

    public static Block GetNewBlock(BlockType type)
    {
        int index = (int)type;
        Block newBlock = (Block)Blocks[index].Instantiate();
        newBlock.AddBlockSprite((Node2D)BlockVisuals[index].Instantiate());

        return newBlock;
    }

    static PackedScene BasicBlock = ResourceLoader.Load<PackedScene>("uid://dm7g0yeneuekf");
    static PackedScene CursedBlock = ResourceLoader.Load<PackedScene>("uid://dggdcwjwy2bfe");
    static PackedScene ReinforcedBlock = ResourceLoader.Load<PackedScene>("uid://kjbhut3hnmrt");
    static PackedScene BombBlock = ResourceLoader.Load<PackedScene>("uid://cmriivs12gm1y"); //TODO
    static PackedScene HologramBlock = ResourceLoader.Load<PackedScene>("uid://omknt2dnim28");
    static PackedScene LaserBlock = ResourceLoader.Load<PackedScene>("uid://6jlkgm20ess5");
    static PackedScene VoidBlock = ResourceLoader.Load<PackedScene>("uid://cmriivs12gm1y"); //TODO
    static PackedScene DiamondBlock = ResourceLoader.Load<PackedScene>("uid://cjd2g4islgdk7");
    static PackedScene GoldBlock = ResourceLoader.Load<PackedScene>("uid://cmriivs12gm1y"); //TODO
    static PackedScene FireBlock = ResourceLoader.Load<PackedScene>("uid://dugo4rcb8mngf");

    static PackedScene[] Blocks = [
        BasicBlock,
        CursedBlock,
        ReinforcedBlock,
        BombBlock,
        HologramBlock,
        LaserBlock,
        VoidBlock,
        DiamondBlock,
        GoldBlock,
        FireBlock
        ];


    static PackedScene BasicBlockSprite = ResourceLoader.Load<PackedScene>("uid://c7abv48quik18");
    static PackedScene CursedBlockSprite = ResourceLoader.Load<PackedScene>("uid://che6me8eo42p7");
    static PackedScene ReinforcedBlockSprite = ResourceLoader.Load<PackedScene>("uid://cmriivs12gm1y");
    static PackedScene BombBlockSprite = ResourceLoader.Load<PackedScene>("uid://cmriivs12gm1y"); //TODO
    static PackedScene HologramBlockSprite = ResourceLoader.Load<PackedScene>("uid://cxto0rlrr4pxj");
    static PackedScene LaserBlockSprite = ResourceLoader.Load<PackedScene>("uid://dyb4r3k7drr66");
    static PackedScene VoidBlockSprite = ResourceLoader.Load<PackedScene>("uid://cmriivs12gm1y"); //TODO
    static PackedScene DiamondBlockSprite = ResourceLoader.Load<PackedScene>("uid://dd55ic6xm5rxr");
    static PackedScene GoldBlockSprite = ResourceLoader.Load<PackedScene>("uid://cmriivs12gm1y"); //TODO
    static PackedScene FireBlockSprite = ResourceLoader.Load<PackedScene>("uid://nqia601hryxr");

    static PackedScene[] BlockVisuals = [
        BasicBlockSprite,
        CursedBlockSprite,
        ReinforcedBlockSprite,
        BombBlockSprite,
        HologramBlockSprite,
        LaserBlockSprite,
        VoidBlockSprite,
        DiamondBlockSprite,
        GoldBlockSprite,
        FireBlockSprite
        ];

    static PackedScene BasicCardBlock = ResourceLoader.Load<PackedScene>("uid://bxjbghjidd7oe");
    static PackedScene CursedCardBlock = ResourceLoader.Load<PackedScene>("uid://dvlylqcyj5p2l");
    static PackedScene ReinforcedCardBlock = ResourceLoader.Load<PackedScene>("uid://bpt452pea0ewe");
    static PackedScene BombCardBlock = ResourceLoader.Load<PackedScene>("uid://dttbse6kfd33n");
    static PackedScene HologramCardBlock = ResourceLoader.Load<PackedScene>("uid://cdeof6fwi5y2q");
    static PackedScene LaserCardBlock = ResourceLoader.Load<PackedScene>("uid://clg0cir271nly");
    static PackedScene VoidCardBlock = ResourceLoader.Load<PackedScene>("uid://ds6nvclvlh6lo");
    static PackedScene DiamondCardBlock = ResourceLoader.Load<PackedScene>("uid://b4dhni0uy4obd");
    static PackedScene GoldCardBlock = ResourceLoader.Load<PackedScene>("uid://cmriivs12gm1y"); //TODO
    static PackedScene FireCardBlock = ResourceLoader.Load<PackedScene>("uid://c8pgbevjjhh4q");

    static PackedScene[] CardBlocks = [
        BasicCardBlock,     //0
        CursedCardBlock,    //1
        ReinforcedCardBlock,//2
        BombCardBlock,      //3
        HologramCardBlock,  //4
        LaserCardBlock,     //5
        VoidCardBlock,      //6
        DiamondCardBlock,   //7
        GoldCardBlock,      //8
        FireCardBlock       //9
        ];

    

    
}
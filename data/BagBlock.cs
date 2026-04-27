

using Godot;
using System;

public class BagBlock(BlockType type, Vector2I position)
{
    BlockType Type = type;
    Vector2I Position = position;

    public CardBlockSprite GetCardPreviewSprite(float h, float s, float v)
    {
        var sprite = (CardBlockSprite) CardBlocks [(int)Type] .Instantiate();

        sprite.Position = Position * 7;

        sprite.SetColor(h, s, v);

        return sprite;
    }

    public Block GetNewBlock(float h, float s, float v, Rect2 rect)
    {
        Block block = (Block)Blocks[(int)Type].Instantiate();
        block.Position = Position * 10;
        block.SetColor(h, s, v);
        block.SetTexture(rect);

        return block;
    }

    public static Block GetNewBlock(BlockType type)
    {
        return (Block)Blocks[(int)type].Instantiate();
    }

    static PackedScene BasicBlock = ResourceLoader.Load<PackedScene>("uid://blmpsbuqvqptb");
    static PackedScene CursedBlock = ResourceLoader.Load<PackedScene>("uid://ctw2ssdea4ogt");
    static PackedScene ReinforcedBlock = ResourceLoader.Load<PackedScene>("uid://4uffm5qf4t0i");

    static PackedScene HologramBlock = ResourceLoader.Load<PackedScene>("uid://b5cakofogsq20");
    static PackedScene LaserBlock = ResourceLoader.Load<PackedScene>("uid://ce3gfft4hauva");
    static PackedScene DiamondBlock = ResourceLoader.Load<PackedScene>("uid://bd4fndwtna7f5");
    static PackedScene FireBlock = ResourceLoader.Load<PackedScene>("uid://vtx0og4n4df0");

    static PackedScene[] Blocks = [
        BasicBlock,
        CursedBlock,
        ReinforcedBlock,
        null,
        HologramBlock,
        LaserBlock,
        null,
        DiamondBlock,
        null, //gold,
        FireBlock
        ];

    static PackedScene BasicCardBlock = ResourceLoader.Load<PackedScene>("uid://bxjbghjidd7oe");
    static PackedScene CursedCardBlock = ResourceLoader.Load<PackedScene>("uid://dvlylqcyj5p2l");
    static PackedScene ReinforcedCardBlock = ResourceLoader.Load<PackedScene>("uid://bpt452pea0ewe");
    static PackedScene BombCardBlock = ResourceLoader.Load<PackedScene>("uid://dttbse6kfd33n");
    static PackedScene HologramCardBlock = ResourceLoader.Load<PackedScene>("uid://cdeof6fwi5y2q");
    static PackedScene LaserCardBlock = ResourceLoader.Load<PackedScene>("uid://clg0cir271nly");
    static PackedScene VoidCardBlock = ResourceLoader.Load<PackedScene>("uid://ds6nvclvlh6lo");
    static PackedScene DiamondCardBlock = ResourceLoader.Load<PackedScene>("uid://b4dhni0uy4obd");
    static PackedScene GoldCardBlock = ResourceLoader.Load<PackedScene>("uid://b4dhni0uy4obd"); //todo
    static PackedScene FireCardBlock = ResourceLoader.Load<PackedScene>("uid://c8pgbevjjhh4q");

    static PackedScene[] CardBlocks = [
        BasicCardBlock,     //0
        CursedCardBlock,    //1
        ReinforcedCardBlock,//2
        BombCardBlock,      //3
        HologramCardBlock,  //4
        LaserCardBlock,     //5
        VoidCardBlock,
        DiamondCardBlock,
        GoldCardBlock,
        FireCardBlock
        ];

    

    
}
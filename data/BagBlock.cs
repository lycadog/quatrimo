

using Godot;
using System;

public class BagBlock(BlockType type, Vector2I position, Vector2I origin)
{
    BlockType Type = type;
    Vector2I Position = position;
    Vector2I PieceOrigin = origin;

    public CardBlockSprite GetCardPreviewSprite(float h, float s, float v)
    {
        var sprite = (CardBlockSprite) CardBlocks [(int)Type] .Instantiate();

        sprite.Position = (Position + PieceOrigin) * 7;

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

    static PackedScene[] Blocks = [
        BasicBlock
        ];

    static PackedScene BasicCardBlock = ResourceLoader.Load<PackedScene>("uid://bxjbghjidd7oe");
    static PackedScene CursedCardBlock = ResourceLoader.Load<PackedScene>("uid://dvlylqcyj5p2l");
    static PackedScene ReinforcedCardBlock = ResourceLoader.Load<PackedScene>("uid://bpt452pea0ewe");
    static PackedScene BombCardBlock = ResourceLoader.Load<PackedScene>("uid://dttbse6kfd33n");
    static PackedScene HologramCardBlock = ResourceLoader.Load<PackedScene>("uid://cdeof6fwi5y2q");
    static PackedScene LaserCardBlock = ResourceLoader.Load<PackedScene>("uid://clg0cir271nly");

    static PackedScene[] CardBlocks = [
        BasicCardBlock,     //0
        CursedCardBlock,    //1
        ReinforcedCardBlock,//2
        BombCardBlock,      //3
        HologramCardBlock,  //4
        LaserCardBlock      //5
        ];

    

    
}
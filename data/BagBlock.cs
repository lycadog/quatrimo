

using Godot;
using System;

public class BagBlock(BlockType type, Vector2I position, Rect2 textureRect, float h, float s, float v)
{
    BlockType Type = type;
    Vector2I Position = position;
    Rect2 TextureRect = textureRect;
    float h = h, s = s, v = v;


    public CardBlockSprite GetCardPreviewSprite()
    {
        var sprite = (CardBlockSprite)CardBlocks[(int)Type].Instantiate();

        sprite.Position = Position * 7;
        sprite.SetColor(h, s, v);

        return sprite;
    }

    public Block GetNewBlock()
    {
        Block block = (Block)Blocks[(int)Type].Instantiate();

        block.Position = Position * 10;
        block.SetColor(h, s, v);
        block.SetTexture(TextureRect);

        return block;
    }

    static PackedScene[] Blocks = [
        BasicBlock
        ];

    static PackedScene BasicBlock = ResourceLoader.Load<PackedScene>("uid://blmpsbuqvqptb");


    static PackedScene[] CardBlocks = [
        BasicCardBlock
        ];

    static PackedScene BasicCardBlock = ResourceLoader.Load<PackedScene>("uid://bxjbghjidd7oe");

    
}
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class BagPiece(PieceType type, BagBlock[] blocks, Vector2I dimensions, Rect2 textureRegion, float h, float s, float v, string name)
{
    public PieceType Type = type;
    public BagBlock[] Blocks = blocks;
    public Vector2I Dimensions = dimensions;

    public Rect2 TextureRegion = textureRegion;
    public float h = h, s = s, v = v;
    public string Name = name;

    //Weight for use in the weighted bag system (each weight = 1 extra entry in the pool)
    //Entry weight is reduced every time this is drawn, then reset after some amount of draws
    public int BaseWeight = 6;


    static Func<FallingPiece>[] Pieces = [
        () => { return new FallingPiece(); }
        ];

    public FallingPiece CreatePiece()
    {
        Block[] newBlocks = new Block[Blocks.Length];

        for(int i = 0; i < Blocks.Length; i++)
        {
            newBlocks[i] = Blocks[i].GetNewBlock(h, s, v, TextureRegion);
        }

        var piece = Pieces[(int)Type]();

        piece.LinkBlocks(newBlocks);

        return piece;
    }

}


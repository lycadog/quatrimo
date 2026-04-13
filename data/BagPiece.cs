using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class BagPiece(PieceType type, BagBlock[] blocks, Rect2 textureRegion, float h, float s, float v, string name)
{
    public PieceType Type = type;
    public BagBlock[] Blocks = blocks;
    public Vector2I Dimensions = new(blocks.GetLength(0), blocks.GetLength(1));

    Rect2 TextureRegion = textureRegion;
    public float h = h, s = s, v = v;
    public string Name = name;


    static Func<FallingPiece>[] Pieces = [
        () => { return new FallingPiece(); }
        ];

    public FallingPiece CreatePiece()
    {
        Block[] newBlocks = new Block[Blocks.Length];

        for(int i = 0; i < Blocks.Length; i++)
        {
            newBlocks[i] = Blocks[i].GetNewBlock();
        }

        var piece = Pieces[(int)Type]();

        piece.LinkBlocks(newBlocks);

        return piece;
    }


    

}


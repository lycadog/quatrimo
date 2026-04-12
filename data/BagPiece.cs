using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class BagPiece
{

    BagBlock[] Blocks;

    PieceType Type;


    static Func<FallingPiece>[] Pieces = [
        () => { return new FallingPiece(); }
        ];




    public FallingPiece CreatePiece()
    {
        var piece = Pieces[(int)Type]();

        foreach(var block in Blocks)
        {
            
        }

        return piece;
    }


    

}




using System.Collections.Generic;

public class PlayerBag
{

    public List<BagPiece> Pieces = [];
    ObjectPool<BagPiece> PiecePool;

    int drawCounter;

    public PlayerBag(List<BagPiece> pieces)
    {
        Pieces = pieces;

        int[] weights = new int[pieces.Count];
        
        for(int i = 0; i < pieces.Count; i++)
        {
            weights[i] = pieces[i].BaseWeight;
        }

        PiecePool = new ObjectPool<BagPiece>(pieces, weights);

    }


    //methods to add/remove stuff from bag

    public PieceCard DrawRandomCard()
    {
        OnDraw();

        var entry = PiecePool.GetRandomEntry();
        entry.SubtractWeight(2);

        return PieceCard.CreateNewCard(entry.heldObject);
    }

    /// <summary>
    /// Whenever we draw we need to run this so we can increase piece weight
    /// </summary>
    public void OnDraw()
    {
        if(drawCounter >= Pieces.Count * 2)
        {
            ResetWeight();
            drawCounter = 0;
        }

        drawCounter++;
    }

    void ResetWeight()
    {
        foreach(var entry in PiecePool.entries)
        {
            entry.SetWeight(entry.heldObject.BaseWeight);
        }
    }

}
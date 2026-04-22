using Godot;
using System.Collections.Generic;

public class StarterBag
{
    List<PieceDefinition> pieces;
    List<(int, float, float)> colors;
    bool useSetColors;
    bool useBagTexture;

    Rect2 blockTextureRect;
    public string name;

    public StarterBag(List<PieceDefinition> pieces, List<(int, float, float)> colors, Rect2 blockTextureRect, string name)
    {
        this.pieces = pieces;
        this.colors = colors;
        this.blockTextureRect = blockTextureRect;
        this.name = name;

        useSetColors = true;
        useBagTexture = true;
    }

    public StarterBag(List<PieceDefinition> pieces, List<(int, float, float)> colors, string name)
    {
        this.pieces = pieces;
        this.colors = colors;
        this.name = name;

        useSetColors = true;
        useBagTexture = false;
    }

    public StarterBag(List<PieceDefinition> pieces, Rect2 blockTextureRect, string name)
    {
        this.pieces = pieces;
        this.blockTextureRect = blockTextureRect;
        this.name = name;

        useSetColors = false;
        useBagTexture = true;
    }

    public StarterBag(List<PieceDefinition> pieces, string name)
    {
        this.pieces = pieces;
        this.name = name;

        useSetColors = false;
        useBagTexture = false;
    }

    public PlayerBag CreateBag()
    {
        return new(GetPieces());
    }

    public List<BagPiece> GetPieces()
    {
        List<BagPiece> bagPieces = [];

        for(int i = 0; i < pieces.Count; i++)
        {
            BagPiece newPiece = pieces[i].GetPiece();

            //if we override color or texture, do so!
            if (useSetColors)
            {
                if (i >= colors.Count)
                {
                    GD.PushWarning($"Too little preset colors specified in bag! Falling back to random color. Expected {pieces.Count}, got {colors.Count}");
                    (float, float, float) coloure = Utils.GetRandomPieceHSV();

                    newPiece.h = coloure.Item1;
                    newPiece.s = coloure.Item2;
                    newPiece.v = coloure.Item3;
                }

                else
                {
                    float h = ((float)colors[i].Item1) / 360f; //we store this in 0-360 format for ease of entering

                    newPiece.h = h;
                    newPiece.s = colors[i].Item2;
                    newPiece.v = colors[i].Item3;
                }
            }

            if (useBagTexture)
            {
                newPiece.TextureRegion = blockTextureRect;
            }

            bagPieces.Add(newPiece);
        }

        return bagPieces;
    }


}
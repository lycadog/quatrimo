

using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

public class pieceType
{
    public pieceType(int xSize, int ySize, wSet<blockType>[,] blocks, wSet<pieceMod> mod, Vector2I origin, int blockCount, string name, colorSet color, Texture2D tex) 
    {
        dimensions = new Vector2I(xSize, ySize);
        blockSet = blocks;
        this.mod = mod;
        this.origin = origin;
        this.blockCount = blockCount;
        this.name = name;
        this.color = color;
        this.tex = tex;
        Debug.WriteLine("RAAAH PIECETYPE TEX NULL: " + tex == null);
    }
    public wSet<blockType>[,] blockSet { get; set; }
    public wSet<pieceMod> mod { get; set; }
    public Vector2I dimensions { get; set; }
    public Vector2I origin {  get; set; }
    public int blockCount {  get; set; }
    public string name { get; set; }
    public rarity rarity { get; set; }
    public colorSet color { get; set; } //properly define and use colorsets
    public Texture2D tex { get; set; }

    public void addToBag(bag bag) //add new piece to player's bag 
    {
        blockType[,] blocks = new blockType[dimensions.x,dimensions.y];
        
        for (int x = 0; x < dimensions.x; x++){
            for (int y = 0; y < dimensions.y; y++){

                if (blockSet[x, y] != null){ //get the blocktype from each blockset
                    blocks[x, y] = blockSet[x, y].getRandom();
                }
            }}
        bagPiece piece = new bagPiece(blocks, mod.getRandom(), dimensions, origin, blockCount, name, rarity, color.getRandomColor(), tex); //create new piece

        bag.pieces.Add(piece); //add new piece to player's bag
        Debug.WriteLine("piece " + piece.name + " added!");
    }
    public void addToStarterBag(bag bag)
    {
        bag.pieces = new System.Collections.Generic.List<bagPiece>();
        
    }
}

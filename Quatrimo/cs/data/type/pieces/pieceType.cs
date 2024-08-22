

using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

public class pieceType
{
    public pieceType(int xSize, int ySize, tileSet[,] tiles, Vector2I origin, int tileCount, string name, colorSet color, Texture2D tex) 
    {
        dimensions = new Vector2I(xSize, ySize);
        tileSet = tiles;
        this.origin = origin;
        this.tileCount = tileCount;
        this.name = name;
        this.color = color;
        this.tex = tex;
        Debug.WriteLine("RAAAH PIECETYPE TEX NULL: " + tex == null);
    }
    public tileSet[,] tileSet { get; set; }
    public Vector2I dimensions { get; set; }
    public Vector2I origin {  get; set; }
    public int tileCount {  get; set; }
    public string name { get; set; }
    public rarity rarity { get; set; }
    public colorSet color { get; set; } //properly define and use colorsets
    public Texture2D tex { get; set; }

    public void addToBag(bag bag) //add new piece to player's bag 
    {
        tileType[,] tiles = new tileType[dimensions.x,dimensions.y];
        
        for (int x = 0; x < dimensions.x; x++){
            for (int y = 0; y < dimensions.y; y++){
                Debug.WriteLine(x + "," + y);
                if (tileSet[x, y] != null){ //get the tiletype from each tileset
                    tiles[x, y] = tileSet[x, y].getRandomType();
                }
            }}
        bagPiece piece = new bagPiece(tiles, dimensions, origin, tileCount, name, rarity, color.getRandomColor(), tex); //create new piece

        bag.pieces.Add(piece); //add new piece to player's bag
        Debug.WriteLine("piece " + piece.name + " added!");
    }
    public void addToStarterBag(bag bag)
    {
        bag.pieces = new System.Collections.Generic.List<bagPiece>();
        
    }
}

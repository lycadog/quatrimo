using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class bagPiece //used for pieces held in the bag
{ //nest this in board piece maybe?
    public bagPiece(baseBlockType[,] blocks, Vector2I dimensions, Vector2I origin, int tileCount, string name, rarity rarity, Color color, Texture2D tex)
    {
        this.blocks = blocks;
        this.dimensions = dimensions;
        this.origin = origin;
        this.tileCount = tileCount;
        this.name = name;
        this.rarity = rarity;
        this.color = color;
        this.tex = tex;
        weight = 1;
    }

    public boardPieceold getBoardPiece(board board)
    {
        
        tile[] tiles = new tile[tileCount];
        boardPieceold piece = new boardPieceold(tiles, dimensions, origin, name, rarity, color, tex, board);
        int index = 0;
        for (int x = 0; x < dimensions.x; x++){
            for(int y = 0; y < dimensions.y; y++){ //process through each tile of the bagPiece and create a real tile for the boardPiece
                if (this.blocks[x, y] != null) //only process solid tiles!
                {
                    
                    tile tile = new tile(piece, new Vector2I(x - origin.x, y - origin.y), board, this.blocks[x, y].getNewInstance(board));
                    tiles[index] = tile;
                    index++;
                }}}
        return piece;

    }

    public baseBlockType[,] blocks { get; set; }
    public Vector2I dimensions { get; set; }
    public Vector2I origin { get; set; }
    public int tileCount {  get; set; }
    public float weight { get; set; } //used to decide chance of drawing this from the bag. may or may not remain a float, decrease this temporarily after drawing the piece maybe?
    public string name { get; set; }
    public Color color { get; set; }
    public Texture2D tex {  get; set; }
    public rarity rarity { get; set; }
}

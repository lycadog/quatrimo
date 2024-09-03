using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class bagPiece //used for pieces held in the bag
{ //nest this in board piece maybe?
    public bagPiece(baseBlockType[,] blocks, Vector2I dimensions, Vector2I origin, int blockCount, string name, rarity rarity, Color color, Texture2D tex)
    {
        this.blocks = blocks;
        this.dimensions = dimensions;
        this.origin = origin;
        this.blockCount = blockCount;
        this.name = name;
        this.rarity = rarity;
        this.color = color;
        this.tex = tex;
        weight = 1;
    }

    /// <summary>
    /// Serializes the bag piece into a playable board piece
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    public boardPiece getBoardPiece(board board)
    {
        
        block[] blocks = new block[blockCount];
        boardPiece piece = new boardPiece(board, blocks, dimensions, origin, name, tex, color);
        int index = 0;
        for (int x = 0; x < dimensions.x; x++){
            for(int y = 0; y < dimensions.y; y++){ //process through each block of the bagPiece and create a real block for the boardPiece
                if (this.blocks[x, y] != null) //only process solid blocks!
                {
                    blockType type = this.blocks[x, y].getType(board); //create our new type

                    element element = new element(type.getTex(piece), type.getColor(piece), new Vector2I(0, -5), 0.8f); //create new sprite element
                        
                    block block = new block(board, element, type, piece, new Vector2I(x - origin.x , y - origin.y)); //create new block
                    blocks[index] = block;
                    index++;
                }}}
        return piece;

    }

    public baseBlockType[,] blocks { get; set; }
    public Vector2I dimensions { get; set; }
    public Vector2I origin { get; set; }
    public int blockCount {  get; set; }
    public float weight { get; set; } //used to decide chance of drawing this from the bag. may or may not remain a float, decrease this temporarily after drawing the piece maybe?
    public string name { get; set; }
    public Color color { get; set; }
    public Texture2D tex {  get; set; }
    public rarity rarity { get; set; }
}

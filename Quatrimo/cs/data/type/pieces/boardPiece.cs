using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

public class boardPiece
{   //IDEA IDEA IDEA: add method to "undraw" piece from the element board after it is drawn while falling
    public boardPiece(tile[] tiles, Vector2I dimensions, Vector2I origin, string name, rarity rarity, Color color, Texture2D tex ,board board)
    {
        this.tiles = tiles;
        this.dimensions = dimensions;
        this.origin = origin;
        this.name = name;
        this.rarity = rarity;
        this.color = color;
        this.tex = tex;
        this.board = board;
        rotDimensions = dimensions;
        isPlaced = false;
        dropOffset = 0;
    }
    public board board;
    public tile[] tiles { get; set; }
    public Vector2I dimensions { get; set; }
    public Vector2I rotDimensions { get; set; } //piece dimensions but rotated properly after rotation
    public Vector2I pos { get; set; } //this position might be desynced from the piece's tile's positions due to 0 index array shenanigans, look into later //what
    public int dropOffset {  get; set; }
    public Vector2I origin {  get; set; }
    public int rotation { get; set; } //varies from 0-3
    public bool isPlaced {  get; set; }
    public string name { get; set; }
    public Color color { get; set; }
    public Texture2D tex { get; set; }
    public rarity rarity { get; set; }

    public void rotatePiece(int direction) //returns true if the rotation is valid
    {
        rotation += direction;
        if(rotation > 3)
        {
            rotation -= 4;
        }else if (rotation < 0)
        {
            rotation += 4;
        }
        foreach(tile tile in tiles)
        {
            tile.rotate(direction);
        }
        Vector2I swap = new Vector2I(rotDimensions.x, rotDimensions.y);
        rotDimensions = swap; //switch rot dimensions since rotDimensions is rotate-adjusted dimensions
        updateTilePosition();
    }
    
    public bool isRotationValid(int direction)
    {
        Debug.WriteLine("CHECKING ROTATION");
        foreach(tile tile in tiles)
        {
            Vector2I rotateBoardPos = tile.getRotatePos(direction).add(pos);
            if (!board.isPositionValid(rotateBoardPos, tile.type.shouldCollide()))
            {
                return false;
            }

        }
        return true;
    }

    public void playPiece() //runs when a piece is dropped
    { //dimensions.y -origin.y+1
        pos = new Vector2I(5, 0 ); //change to proper position later
        updateTilePosition();
    }

    public void moveFallingPiece(int xOffset, int yOffset) //x and y offset for which direction to move
    {
        Debug.WriteLine("PIECE POSITION PREMOVE: " + pos.x + ", " + pos.y);
        Vector2I offset = new Vector2I(xOffset, yOffset);
        pos = pos.add(offset);
        Debug.WriteLine("PIECE POSITION POSTMOVE: " + pos.x + ", " + pos.y);

        updateTilePosition();
    }

    public void placePiece()
    {
        foreach (tile tile in tiles) //place every tile in the piece
        {
            tile.place();
            tile.isPlaced = true;
            tile.renderPlaced();
        }
        isPlaced = true;
    }

    public bool isMoveValid(int xOffset = 0, int yOffset = 0) //returns whether or not a move is valid
    {
        foreach(tile tile in tiles)
        {            
            if (tile.checkMoveCollision(xOffset, yOffset))    
            {
                return false;
            }
        }
        return true;
    }

    public bool shouldPlace() //checks if a piece should be placed or not
    {
        foreach(tile tile in tiles)
        {
            //process through every solid tile and check the collision
            
            if (tile.checkFallingCollision() == false)    
            {
                continue; //keep checking collision if the tile is not colliding, if one tile collides then the else will return true
            }
                
            else { return true; }
        }
        return false; //if no tiles collide then return false
    }

    public void renderFalling()
    {
        foreach(tile tile in tiles) { tile.renderFalling(); }
    }
    public void updateTilePosition()
    {
        foreach (tile tile in tiles)
        {
            tile.updatePos();
        }
        updateDropPos();
    }

    public void updateDropPos()
    {
        int y = 0;
        
        while (true)
        {
            if(!isMoveValid(0, y))
            {
                dropOffset = System.Math.Max(y - 1, 0); break;
            }
            else { y++; }
        }
    }

    /*public void renderDropShadow() //renders the shadow below pieces and the piece preview
    {
        int y = 0;
        dropPos = pos;
        while(true)
        {
            if (!isMoveValid(0, -y-1)) //i have no idea why but it always goes too low and reaches out of bounds without the -1. do NOT remove it
            {
                dropPos = new Vector2I(pos.x, pos.y - y);
                break;
            }
            else { y += 1; }
            
        }
        foreach(tile tile in tiles)
        {
            Vector2I previewPos = new Vector2I(tile.boardPos.x, tile.boardPos.y - y);
            renderable render = new renderable(previewPos, "[color=999999]▒", 0, true);
            
        }
    }*/

    
}

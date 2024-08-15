

using System.Diagnostics;

public class tile
{ //this class needs a general cleanup
    public tile(boardPiece piece, Vector2I localPos, board board)// IMPORTANT! SPECIFY THE TILE TYPE JUST AFTER CREATION ===
    {
        this.piece = piece;
        this.localPos = localPos; //we need a function for newly created tiles to initialize their local position! perhaps when new pieces are added to bag? //i think i did this already
        isPlaced = false;
        preRotPos = localPos;
        this.board = board;
    }
    public board board;
    public tileType type { get; set; } //rework how tiletype methods work maybe by adding them here
    public boardPiece piece { get; set; }
    public Vector2I boardPos { get; set; } //used for placed tiles on the board
    public Vector2I localPos { get; set; } //used for local position relative to the origin of the piece the tile belongs to
    public Vector2I preRotPos {  get; set; }
    public bool isPlaced { get; set; }

    public bool checkMoveCollision(int xOffset, int yOffset) //returns whether or not the next move will collide
    { //the offsets are for the tile to check
        //GD.Print($"checkMoveCollision event at {boardPos.X}, {boardPos.y} offset at {xOffset}, {yOffset}");
        Vector2I checkPos = new Vector2I(boardPos.x + xOffset, boardPos.y + yOffset);
        
        if(checkPos.x < 0) { return true; }
        if(checkPos.x >= board.dimensions.y) { return true; } //if the tile is outside the board dimensions return true (invalid move)
        if(checkPos.y < 0) { return true; }
        if(checkPos.y >= board.dimensions.y) { return true; } //maybe add game over code here?

        

        return type.checkMoveCollision(boardPos, checkPos);

    }
    public bool checkFallingCollision() //returns whether or not the piece is colliding with something below it
    {
        if (boardPos.y == 0) //check if the colliding tile is outside the array
        {
            boardCollideFalling();
            return true;
        }
        return type.checkFallingCollision(boardPos);
    }
    public bool checkPlacedCollision() //returns whether or not the placed tile should collide
    {
        return type.checkPlacedCollision();
    }
    public void collideFalling() //runs when this falling tile collides with another tile ***WORRY ABOUT THESE 2 METHODS LATER***
    {
        Debug.WriteLine($"collideFalling event at {boardPos.x}, {boardPos.y}");

        type.collideFalling(this);
    }
    public void boardCollideFalling() //runs when this falling tile collides with the bottom of the board 
    {
        Debug.WriteLine($"boardCollideFalling event at {boardPos.x}, {boardPos.y}");

        type.boardCollide(this);
    }
    public void collideGround() //runs when this placed tile collides with a falling tile ***WORRY ABOUT THESE 2 METHODS LATER***
    {
        Debug.WriteLine($"collideGround event at {boardPos.x}, {boardPos.y}");

        type.collideGround(this);
    }

    public void place() //places the tile on the board properly **** SOMETHING STRANGE happening here, the first tile is placed properly but the rest lag (are 1 tile up briefly), may result in issues
    {
        Debug.WriteLine($"place event at {boardPos.x}, {boardPos.y}");

        boardPos = piece.pos.add(localPos); //get proper tile position

        board.tiles[boardPos.x, boardPos.y] = this; //place tile on the board
        type.place(this);
    }
    public void tick() //runs after every turn
    {
        Debug.WriteLine($"tick event at {boardPos.x}, {boardPos.y}");

        type.tick(this);
    }

    public long score(long score) //runs when this tile is scored
    {
        Debug.WriteLine($"score event at {boardPos.x}, {boardPos.y}");

        long tileScore = type.score(this);
        remove(board);
        return tileScore;
    }

    public void destroy() //runs when this tile is removed without scoring it
    {
        type.destroy(this);
        remove(board);
    }


  

    public void rotate(int direction)//DIRECTION SHOULD ONLY EVER BE -1 OR 1, NEVER ANYTHING ELSE
    {
        localPos = getRotatePos(direction);
        updatePos();
        render(board);
    }

    public Vector2I getRotatePos(int direction)
    {
        return new Vector2I(localPos.y * -direction, localPos.x * direction); //swap x and y coordinates
    }
    public void updatePos() //updates tile board position
    {
        boardPos = piece.pos.add(localPos);
    }

    public void render(board board)
    {
        renderable render = new renderable(boardPos, getAscii(), 1, !isPlaced);
        board.boardRenderQueue.Add(render);
    }
    public string getAscii()
    {
        return "[color=" + piece.color + "]" + type.getAscii();
    }

    public void remove(board board) //used to remove a tile
    {
        board.tiles[boardPos.x, boardPos.y] = null;
        //board.boardStaleTiles.Add(board.asciiForeground[boardPos.x,boardPos.y]); //add the tile to be removed next render step
        isPlaced = false;
    }
}
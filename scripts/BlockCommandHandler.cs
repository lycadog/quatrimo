

using Godot;

public static class BlockCommandHandler
{
    static Board board;
    static Cell[,] CellBoard;


    public static void LinkToBoard(Board board, Cell[,] CellBoard)
    {
        BlockCommandHandler.board = board;
        BlockCommandHandler.CellBoard = CellBoard;

    }
    
    public static void AddAnimationToBoard(Node2D animation, Vector2 globalPosition)
    {
        board.AddChild(animation);
        animation.GlobalPosition = globalPosition;
    }

    public static void AddBlockToBoard(Block block)
    {
        board.PlaceBlockDirectlyOnBoard(block);
    }


    public static void ScoreBlock(int x, int y)
    {
        //if out of bounds: cancel the operation
        if (x < 0 || x >= CellBoard.GetLength(0) || y < 0 || y >= CellBoard.GetLength(1))
        {
            return;
        }
        

    }

    public static void ShatterConnectedGlassBlocks(int x, int y)
    {
        //spiral outwards through connected glass blocks and shatter them all
    }

    public static void ScoreConnectedCrystalBlocks(int x, int y)
    {

    }


    



}
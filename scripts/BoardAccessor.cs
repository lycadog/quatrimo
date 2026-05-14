

using Godot;

public static class BoardAccessor
{
    //todo: this sucks. but we need to check in on the board somehow!

    static Board _CurrentBoard;

    public static Board CurrentBoard
    {
        private get => _CurrentBoard; 
        set => _CurrentBoard = value;
    }

    public static bool IsValidMoveTo(Vector2I position, bool isSolid)
    {
        if (IsOutOfBounds(position))
        {
            return false;
        }

        //if we're nonsolid and the position we're checking is not absolutely solid: move is valid!
        if (!isSolid && !CurrentBoard[position.X, position.Y].AbsoluteSolid)
        {
            return true;
        }

        return !CurrentBoard[position.X, position.Y].Solid;
    }
    static bool IsOutOfBounds(Vector2I position)
    {
        if (position.X < 0 || position.X >= CurrentBoard.CellDimensions.X
        || position.Y < 0 || position.Y >= CurrentBoard.CellDimensions.Y)
        {
            return true;
        }

        return false;
    }
    public static bool IsOutsideVisualBoard(Vector2I position)
    {
        if (position.X < 0 || position.X >= CurrentBoard.Dimensions.X - 2
        || position.Y < 0 || position.Y >= CurrentBoard.Dimensions.Y - 2)
        {
            return true;
        }

        return false;
    }

    public static void DestroyBlock(Vector2I boardPos)
    {
        CurrentBoard[boardPos.X, boardPos.Y].DeleteBlock();
    }

    public static void DestroyBlock(Block block)
    {
        block.Delete();
    }

    public static void AddSprite(Node2D node, Vector2 position)
    {
        CurrentBoard.BlockBox.AddChild(node);
        node.Position = position;
    }
    public static void AddSprite(Node2D node, Vector2I boardPos)
    {
        Vector2 position = new(boardPos.X * 10, boardPos.Y * -10);
        AddSprite(node, position);
    }
    public static void PlaceBlockDirectlyToBoard(Block block, bool doWhiteFlash = true)
    {
        CurrentBoard.PlaceBlockDirectlyOnBoard(block, doWhiteFlash);
    }


}
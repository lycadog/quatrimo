

using Godot;

public static class BoardAccessor
{
    //todo: this sucks. but we need to check in on the board somehow!

    static Board _Board;


    public static Vector2I CellDimensions
    {
        get => Board.CellDimensions;
    }

    public static Board Board
    {
        private get => _Board;
        set => _Board = value;
    }

    public static bool IsValidMoveTo(Vector2I position, bool isSolid)
    {
        if (IsOutOfBounds(position))
        {
            return false;
        }

        //if we're nonsolid and the position we're checking is not absolutely solid: move is valid!
        if (!isSolid && !Board[position.X, position.Y].AbsoluteSolid)
        {
            return true;
        }

        return !Board[position.X, position.Y].Solid;
    }
    static bool IsOutOfBounds(Vector2I position)
    {
        if (position.X < 0 || position.X >= Board.CellDimensions.X
        || position.Y < 0 || position.Y >= Board.CellDimensions.Y)
        {
            return true;
        }

        return false;
    }
    public static bool IsOutsideVisualBoard(Vector2I position)
    {
        if (position.X < 0 || position.X >= Board.VisualDimensions.X - 2
        || position.Y < 0 || position.Y >= Board.VisualDimensions.Y - 2)
        {
            return true;
        }

        return false;
    }

    public static void DestroyBlock(Vector2I boardPos)
    {
        Board[boardPos.X, boardPos.Y].DeleteBlock();
    }

    public static void DestroyBlock(Block block)
    {
        block.Delete();
    }

    public static void AddNodeToBoard(Node2D node, Vector2 position)
    {
        Board.BlockBox.AddChild(node);
        node.Position = position;
    }
    public static void AddNodeToBoard(Node2D node, Vector2I boardPos)
    {
        Vector2 position = new(boardPos.X * 10, boardPos.Y * -10);
        AddNodeToBoard(node, position);
    }
    public static void PlaceBlockDirectlyToBoard(Block block, bool doWhiteFlash = true)
    {
        Board.PlaceBlockDirectlyOnBoard(block, doWhiteFlash);
    }

    public static Block CreateFallingBlock(BlockType type)
    {
        Block newBlock = BagBlock.GetNewBlock(type);
        Board.ConnectNewBlock(newBlock);

        return newBlock;
    }

    public static FallingEnemyBlock CreateFallingEnemyBlock(int x, int y, BlockType type)
    {
        Block newBlock = CreateFallingBlock(type);

        FallingEnemyBlock FallingBlock = new(newBlock, x, y);

        Board.BlockBox.AddChild(FallingBlock);
        FallingBlock.SetPreviewSprite();

        Board.Connect(Board.SignalName.Piecefall_Started,
            Callable.From(FallingBlock.SetLowVisibility));

        Board.Connect(Board.SignalName.Piecefall_Ended,
            Callable.From(FallingBlock.SetNormalVisibility));

        FallingBlock.Position = new(x * 10, (y + Board.CellDimensions.Y - 8) * -10);


        return FallingBlock;
    }
    
}

using Godot;

public class Cell(int x, int y)
{
    Block heldBlock;
    public Block HeldBlock { get => heldBlock; set => SetBlock(value); }

    bool Occupied = false;
    public bool Scorable = false;

    public void Score()
    {

    }

    /// <summary>
    /// Place new block inside this cell
    /// </summary>
    /// <param name="block"></param>
    public void PlaceBlock(Block block)
    {
        if (!Occupied)
        {
            SetBlock(block);
            return;
        }

        //TODO: do clipping shenanigans here
    }

    void SetBlock(Block block)
    {
        if (Occupied)
        {
            GD.Print($"Block cell {x}, {y} overwritten");
        }

        Scorable = block.Scorable;
        heldBlock = block;
        Occupied = true;
    }

    public Block RemoveBlock()
    {
        if (!Occupied) { GD.PushError("Attempted to remove block from unoccupied block!"); }

        Scorable = false;
        Occupied = false;
        return heldBlock;
    }

}
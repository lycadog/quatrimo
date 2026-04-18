
using Godot;

public partial class BoardRow(int y, int boardWidth) : Node
{
    public int y = y;
    public int totalScorability = 0;

    public Cell[] cells;

    public bool Scorable
    {
        get
        {
            return
                totalScorability >= boardWidth - RunStats.EmptySpacesAllowedInScoring - RunStats.EmptySpacesRequiredInScoring
                &&
                totalScorability <= boardWidth - RunStats.EmptySpacesRequiredInScoring;
        }
    }

    public void AttemptScoring()
    {
        GD.Print("attempted scoring: " + Scorable);
        if (Scorable)
        {
            StartScoring();
        }
    }

    public void StartScoring()
    {
        bool leftIteratorNext = true;

        for(int x = 0; x < boardWidth; x++)
        {
            //if we find a newly placed block, iterate left from it. then change state to next line
            //if we find a block that isn't newly placed, iterate right from the block to the left of it. then revert state

            if (cells[x].JustPlaced)
            {
                if (cells[x].Scorable) { cells[x].ScoreBlock(); }

                if (leftIteratorNext)
                {
                    CreateIterator(x, -1);
                    leftIteratorNext = false;
                    continue;
                }
            }

            else if(!leftIteratorNext)
            {
                CreateIterator(x - 1, 1);
                leftIteratorNext = true;
            }
        }
    }

    void CreateIterator(int x, int direction)
    {
        GD.Print($"iterator created, x: {x}, direction: {direction}");
        ScoreIterator newIterator = new(x, direction, cells);
        AddChild(newIterator);
        
    }

    public void BindCell(Cell cell)
    {
        cell.BecameScorable += OnCellBecameScorable;
        cell.BecameNonScorable += OnCellBecameNonscorable;
    }

    public void OnCellBecameScorable()
    {
        totalScorability++;
    }

    public void OnCellBecameNonscorable()
    {
        totalScorability--;
    }

    public override void _Ready()
    {
        Name = $"Row #{y}";
    }
}

using Godot;
using System;

public partial class BoardRow(int y, int boardWidth) : Node
{
    public int y = y;
    public int totalScorability = 0;

    int minimumRequiredScorability
    {
        get
        {
            return boardWidth - RunStats.EmptySpacesAllowedInScoring - RunStats.EmptySpacesRequiredInScoring;
        }
    }

    int maximumAllowedScorability
    {
        get
        {
            return boardWidth - RunStats.EmptySpacesRequiredInScoring;
        }
    }

    public Cell[] cells;

    public event Action ScoringStarted;

    public bool Scorable
    {
        get
        {
            return
                totalScorability >= minimumRequiredScorability
                &&
                totalScorability <= maximumAllowedScorability;
        }
    }

    public void AttemptScoring()
    {
        if (Scorable)
        {
            CursedBlockFailsafe();

            ScoringStarted?.Invoke();
            StartScoring();
        }
    }

    public void StartScoring()
    {
        bool leftIteratorNext = true;
        bool createdIterator = false;

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
                    createdIterator = true;
                    continue;
                }
            }

            else if(!leftIteratorNext)
            {
                CreateIterator(x - 1, 1);
                leftIteratorNext = true;
            }
        }

        //if we failed to make an iterator, emergency backup! this starts from the middle and goes out
        if (!createdIterator)
        {
            CreateIterator(boardWidth / 2 + 1, -1);
            CreateIterator(boardWidth / 2 - 1, 1);
        }
    }

    void CreateIterator(int x, int direction)
    {
        GD.Print($"iterator created, x: {x}, direction: {direction}");
        ScoreIterator newIterator = new(x, direction, cells);
        AddChild(newIterator);
        
    }

    void CursedBlockFailsafe()
    {
        int cursedCount = 0;
        foreach(var cell in cells)
        {
            if(cell.BlockType == BlockType.Cursed)
            {
                cursedCount++;
            }
        }

        if(cursedCount >= minimumRequiredScorability && cursedCount <= maximumAllowedScorability)
        {
            foreach(var cell in cells)
            {
                cell.ScoreFlag = Cell.ScoringFlags.CanScoreButFullyRestrictAfterScoring;
            }
        }
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
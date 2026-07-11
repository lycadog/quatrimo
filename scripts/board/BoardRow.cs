
using Godot;
using System;

public partial class BoardRow : Node
{
    public int y;
    int BoardWidth;
    Cell[] Cells;
    public int totalScorability = 0;

    public BoardRow(int y, int BoardWidth, Cell[] Cells)
    {
        this.y = y;
        this.BoardWidth = BoardWidth;
        this.Cells = Cells;

        foreach(var cell in Cells)
        {
            //bind cell events
            BindCell(cell);
        }
    }

    int MinimumRequiredScorability
    {
        get
        {
            return BoardWidth - Run.Current.EmptySpacesAllowedInScoring - Run.Current.EmptySpacesRequiredInScoring;
        }
    }

    int MaximumAllowedScorability
    {
        get
        {
            return BoardWidth - Run.Current.EmptySpacesRequiredInScoring;
        }
    }


    public event Action ScoringStarted;
    public event Action<ScoreIterator> CreatedIterator;

    public bool Scorable
    {
        get
        {
            return
                totalScorability >= MinimumRequiredScorability
                &&
                totalScorability <= MaximumAllowedScorability;
        }
    }

    public void StartScoring()
    {
        CursedBlockFailsafe();

        bool leftIteratorNext = true;
        bool createdIterator = false;

        for(int x = 0; x < BoardWidth; x++)
        {
            //if we find a newly placed block, iterate left from it. then change state to next line
            //if we find a block that isn't newly placed, iterate right from the block to the left of it. then revert state

            if (Cells[x].JustPlaced)
            {
                if (Cells[x].Scorable) { Cells[x].ScoreBlock(); }

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
            CreateIterator(BoardWidth / 2 + 1, -1);
            CreateIterator(BoardWidth / 2 - 1, 1);
        }
    }

    void CreateIterator(int x, int direction)
    {
        ScoreIterator newIterator = new(x, direction, Cells);
        AddChild(newIterator);

        CreatedIterator.Invoke(newIterator);
    }

    /// <summary>
    /// Check if every block is a cursed block. If so, restrict their scoring after they score!
    /// </summary>
    void CursedBlockFailsafe()
    {
        int cursedCount = 0;
        foreach(var cell in Cells)
        {
            if(cell.BlockType == BlockType.Cursed)
            {
                cursedCount++;
            }
        }

        if(cursedCount >= MinimumRequiredScorability && cursedCount <= MaximumAllowedScorability)
        {
            foreach(var cell in Cells)
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
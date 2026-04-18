
using Godot;
using System;

public class Cell(int x, int y, Vector2 cellPosition)
{

    Block _HeldBlock;
    public Block HeldBlock { get => _HeldBlock; set => SetBlock(value); }

    bool _Occupied = false;
    bool Occupied 
    { 
        get => _Occupied;
        set
        {
            UpdatedBoard.Invoke();
            bool staleScorable = Scorable;
            _Occupied = value;
            ScorabilityUpdated(staleScorable); //occupied changes this value so we need to mark this as updated
        }
    }

    ScoringFlags _ScoreFlag;
    public ScoringFlags ScoreFlag
    {
        get => _ScoreFlag;
        set
        {
            bool staleScorable = Scorable;
            _ScoreFlag = value;
            ScorabilityUpdated(staleScorable); //this value may have changed so let's check if we should run our events

        }
    }

    public double ScoreValue
    {
        get
        {
            if (Occupied)
            {
                return _HeldBlock.ScoreValue;
            }
            return 0;
        }

        set
        {
            if (Occupied)
            {
                HeldBlock.ScoreValue = value;
            }
            else
            {
                GD.PushWarning($"Attempted to give score value {value} to empty block!");
            }
        }
    }

    public bool Scorable
    {
        get
        {
            if (Occupied && ScoreFlag == ScoringFlags.CanScore)
            {
                return _HeldBlock.Scorable;
            }
            return false;
        }
    }

    public bool JustPlaced
    {
        get
        {
            if (_Occupied) { return HeldBlock.JustPlaced; }
            return false;
        }
    }

    public event Action BecameScorable;
    public event Action BecameNonScorable;

    public event Action UpdatedBoard;

    void ScorabilityUpdated(bool staleScorable)
    {
        if(staleScorable == Scorable)
        {
            return;
        }

        if (Scorable == false)
        {
            BecameNonScorable?.Invoke();
        }
        else
        {
            BecameScorable?.Invoke();
        }

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
        //TODO: maybe find a way to sync block to cell position here?
        //we can define a global position for the cell if it uses a real node or similar. or even through math
        if (Occupied)
        {
            GD.Print($"Block cell {x}, {y} overwritten");
            RemoveBlock();
        }

        _HeldBlock = block;
        _HeldBlock.EmitSignal(Block.SignalName.MovedCells);
        _HeldBlock.Position = cellPosition; //Update our blocks position to ensure it is placed right
        Occupied = true;

        _HeldBlock.TreeExiting += RemoveBlock;
        _HeldBlock.MovedCells += RemoveBlock;
        //If our block is deleted or *gasp* abandons us for a new cell! We need to discard our connections to it

    }

    public void RemoveBlock()
    {
        if (!Occupied) { GD.PushError("Attempted to remove block from unoccupied block!"); return; }

        _HeldBlock.TreeExiting -= RemoveBlock;
        _HeldBlock.MovedCells -= RemoveBlock;

        Occupied = false;
        _HeldBlock = null;
    }

    public void ScoreBlock()
    {
        //TODO: score stuff here

        if (Occupied)
        {
            HeldBlock.Score();
        }
        
        ScoreFlag = ScoringFlags.CannotScoreThisStep;
    }

    public void OnScoreStepBegin(bool isInitialStep)
    {
        if (isInitialStep)
        {
            ScoreFlag = ScoringFlags.CanScore;
            return;
        }

        if (ScoreFlag == ScoringFlags.CannotScoreThisTurn)
        {
            return;
        }

        ScoreFlag = ScoringFlags.CanScore;
    }

    public enum ScoringFlags
    {
        CanScore,
        CannotScoreThisStep,    //Each scoring step can only process a block once. Set to this when we score
        CannotScoreThisTurn     //We reset this on the initial scoring step!
    }

}

using Godot;
using System;

public class Cell(int x, int y, Vector2 cellRealPosition)
{
    public int x = x, y = y;
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

    public bool IsScored
    {
        get
        {
            if (_Occupied) { return HeldBlock.IsScored; }
            return false;
        }
    }


    public event Action BecameScorable;
    public event Action BecameNonScorable;
    public event Action UpdatedBoard;
    public event Action<Block> DeletedBlock;

    public int LowerDistance
    {
        get
        {
            if (Occupied) { return HeldBlock.LowerDistance; }
            return 0;
        }

        set
        {
            if (Occupied)
            {
                HeldBlock.LowerDistance = value;
            }
        }
    }

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

        GD.Print("Placement clipping behavior ran!");

        //clipping shenanigans below
        //basically, we want to run through every possible method to ensure SOMETHING handles this.
        //we need to run falling methods bc slamming blocks runs before normal falling methods can run

        //this is terrible. we have no way of knowing if things resolved properly so we have to check everything.
        //TODO fix this mess

        HeldBlock.CollidedWithBlockWhilePlaced(block);

        if (block.IsQueuedForDeletion()) { return; }
        else if (HeldBlock.IsQueuedForDeletion()) { SetBlock(block); return; } //if any of our blocks have been deleted, return

        HeldBlock.FallingBlockAttemptingPlacementOnUs(block);

        if(!HeldBlock.IsQueuedForDeletion() && !block.IsQueuedForDeletion())
        {
            //if nothing gets resolved, just delete the falling block and carry on
            block.QueueFree();
            return;
        }

        if (!block.IsQueuedForDeletion())
        {
            SetBlock(block); //if our falling block isn't queued for deletion the other block is. so let's place
        }
    }

    public void DeleteBlock()
    {
        if (Occupied)
        {
            DeletedBlock?.Invoke(HeldBlock);
            _HeldBlock.QueueFree();
        }
        //if not occupied: cool! we can just chill
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
        _HeldBlock.Position = cellRealPosition; //Update our blocks position to ensure it is placed right
        _HeldBlock.boardX = x; _HeldBlock.boardY = y;
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
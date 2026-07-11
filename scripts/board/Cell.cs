
using Godot;
using System;

public class Cell(int x, int y)
{
    public int x = x, y = y;
    public Vector2 RealPosition = new(x * 10, -y * 10);
    Block _HeldBlock;
    public Block HeldBlock { get => _HeldBlock; set => SetBlock(value); }

    public event Action BecameScorable;
    public event Action BecameNonScorable;
    public event Action UpdatedBoard;
    public event Action<Cell> Scored;

    #region Properties
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

    public bool Solid
    {
        get
        {
            if (_Occupied) { return HeldBlock.SolidWhenPlaced; }
            return false;
        }
    }
    public bool AbsoluteSolid
    {
        get
        {
            if (_Occupied) { return HeldBlock.AbsoluteSolid; }
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
                //todo: maybe remove this warning
            }
        }
    }
    public bool Scorable
    {
        get
        {
            if (Occupied && (ScoreFlag == ScoringFlags.CanScore || ScoreFlag == ScoringFlags.CanScoreButFullyRestrictAfterScoring))
            {
                return _HeldBlock.Scorable;
            }
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
    public bool FilledInOnScoring
    {
        get
        {
            if (_Occupied)
            {
                return HeldBlock.RemovedOnScoring;
            }
                return true;
            }
    }

    public BlockType BlockType
    {
        get
        {
            if (_Occupied) { return HeldBlock.type; }

            return BlockType.None;
        }
    }

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
    #endregion

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
    /// <param name="newBlock"></param>
    public void PlaceBlock(Block newBlock)
    {

        //if we're empty, OR our current block is going to be deleted: we can safely place the new block here
        if (!Occupied || HeldBlock.IsQueuedForDeletion())
        {
            SetBlock(newBlock);
            return;
        }

        //GD.Print("Placement clipping behavior ran!");

        //we do this so we can keep a reference to the block if it deletes itself from our HeldBlock reference
        Block oldBlock = HeldBlock;

        oldBlock.CollidedWithBlock(newBlock, true);
        newBlock.CollidedWithBlock(oldBlock, true);

        //run both collision methods
        //if the held block isn't deleted, we will delete the falling block and return

        if (Occupied) //if we're still occupied: delete falling block
        {
            //GD.Print("Deleting falling block on clipping!");
            newBlock.Delete();
            return;
        }
        else if (newBlock.IsQueuedForDeletion())
        {
            return;
        }

        //if the held block was deleted, we check if the falling block was deleted.
        //if the falling block wasn't deleted then we can place it!
    
        SetBlock(newBlock);
    }

    public void DeleteBlock()
    {
        if (Occupied)
        {
            _HeldBlock.Delete();
        }
        //if not occupied: cool! we can just chill
    }

    void SetBlock(Block block)
    {
        if (Occupied)
        {
            RemoveBlock();
        }

        _HeldBlock = block;
        _HeldBlock.EmitSignal(Block.SignalName.MovedCells);
        _HeldBlock.Position = RealPosition; //Update our blocks position to ensure it is placed right
        _HeldBlock.BoardPos = new(x, y);

        _HeldBlock.UpdateOutsideBoardSprite();

        Occupied = true;

        _HeldBlock.Deleted += RemoveBlock;
        _HeldBlock.MovedCells += RemoveBlock;
        //If our block is deleted or *gasp* abandons us for a new cell! We need to discard our connections to it

    }

    void RemoveBlock()
    {
        if (!Occupied) { GD.PushError("Attempted to remove block from unoccupied block!"); return; }

        _HeldBlock.Deleted -= RemoveBlock;
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

        Scored.Invoke(this);
        UpdatedBoard.Invoke(); //i'm not sure why this wasn't being invoked here already but it is now. could cause issues?

        if (ScoreFlag == ScoringFlags.CanScoreButFullyRestrictAfterScoring)
        {
            ScoreFlag = ScoringFlags.CannotScoreThisTurn;
            return;
        }

        //if we're in this method we already know we are flagged to score, so we don't need to check
        ScoreFlag = ScoringFlags.CannotScoreThisStep; 
        
    }

    /// <summary>
    /// Fully reset ScoringFlags on turn start.
    /// </summary>
    public void FullyResetScoringFlag()
    {
        ScoreFlag = ScoringFlags.CanScore;
    }

    /// <summary>
    /// Partially reset ScoringFlags on each scoring step.
    /// </summary>
    public void PartiallyResetScoringFlag()
    {
        if (ScoreFlag == ScoringFlags.CannotScoreThisTurn)
        {
            return;
        }

        ScoreFlag = ScoringFlags.CanScore;
    }

    public enum ScoringFlags
    {
        CanScore,
        CanScoreButFullyRestrictAfterScoring,   //After scoring this is set to CannotScoreThisTurn
        CannotScoreThisStep,    //Each scoring step can only process a block once. Set to this when we score
        CannotScoreThisTurn     //We reset this on the initial scoring step!
    }

}
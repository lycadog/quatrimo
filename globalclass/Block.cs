using Godot;
using System;
using System.Security.Cryptography;

[GlobalClass, Icon("res://texture/icon/blockicon.png")]
public partial class Block : Area2D
{
    public int boardX, boardY;

    public float SlamOffset;
    public bool CanMoveLeft = true;
    public bool CanMoveDown = true;
    public bool CanMoveRight = true;
    public bool CanRotateNegative = true;
    public bool CanRotatePositive = true;
    public int LowerDistance = 0;

    public bool isPlaced = false;

    public bool JustPlaced = false;
    public bool IsScored = false;
    public bool IsTicked = false;

    bool ExitBoardBehaviorEnabled = true;

    [Export] public bool Scorable = true;
    [Export] public bool RemovedOnScoring = true;
    [Export] public double ScoreValue = 1;

    [Export] bool SolidWhenFalling = true;
    [Export] bool SolidWhenPlaced = true;
    [Export] bool OverrideColor = false;
    [Export] bool OverrideTexture = false;

    [Export] protected Node2D SpriteLayer1;
    [Export] protected Node2D SpriteLayer2;
    [Export] protected Sprite2D AboveBoardIndicatorSprite;
    [Export] protected Sprite2D DropPreviewSprite;
    [Export] protected Sprite2D WhiteFlashSprite;
    [Export] protected Area2D LeftArea;
    [Export] protected Area2D DownArea;
    [Export] protected Area2D RightArea;
    [Export] protected Area2D NegativeRotationArea;
    [Export] protected Area2D PositiveRotationArea;
    [Export] protected RayCast2D DropPreviewRaycast;

    [Signal] public delegate void PlacedEventHandler(Block block);
    [Signal] public delegate void ScoredEventHandler(Block block);
    [Signal] public delegate void MovedCellsEventHandler();

    const double PlacementFlashLength = .18;

    #region === Board Interaction/Positional Methods ===

    public virtual void Play()
    {
        ForceUpdateTransform();
        DropPreviewRaycast.ForceRaycastUpdate();
        DropPreviewSprite.Visible = true;
        UpdateRotationDestinations();
        ToggleOffBoardTexture(true);
    }

    public void Score()
    {
        IsScored = true;
        CustomScore();
        EmitSignalScored(this);
    }

    protected virtual void CustomScore()
    {
        HideSprites();
    }

    public void Place()
    {
        ExitBoardBehaviorEnabled = false;
        ToggleOffBoardTexture(false);

        SetCollisionLayerValue(1, SolidWhenPlaced); //we are now solid on the placedblocks layer

        isPlaced = true;
        JustPlaced = true;

        DropPreviewRaycast.Enabled = false;
        DropPreviewSprite.Visible = false;

        LeftArea.ProcessMode = ProcessModeEnum.Disabled;
        RightArea.ProcessMode = ProcessModeEnum.Disabled;
        DownArea.ProcessMode = ProcessModeEnum.Disabled;
        NegativeRotationArea.ProcessMode = ProcessModeEnum.Disabled;
        PositiveRotationArea.ProcessMode = ProcessModeEnum.Disabled;

        //temporarily make the white sprite visible so we get a little flash animation when we place stuff
        WhiteFlashSprite.Visible = true;
        Tween tween = GetTree().CreateTween().SetParallel();
        tween.TweenCallback(Callable.From(() => WhiteFlashSprite.Visible = false)).SetDelay(PlacementFlashLength);
        tween.TweenProperty(GetNode("WhiteFlashBox/PointLight2D"), "energy", 0, PlacementFlashLength);

        EmitSignalPlaced(this);
        //do clipping? idk
    }

    public void Rotate(int direction = 1)
    {
        if(direction == -1)
        {
            GlobalPosition = NegativeRotationArea.GlobalPosition;
        }
        else if(direction == 1)
        {
            GlobalPosition = PositiveRotationArea.GlobalPosition;
        }

        else { throw new ArgumentOutOfRangeException($"Attempted to rotate block with invalid direction value: {direction}"); }

        //Move to new positions
        UpdateRotationDestinations();
    }

    protected void UpdateRotationDestinations()
    {
        NegativeRotationArea.Position = GetAreaRotationPositon(Position, -1);
        PositiveRotationArea.Position = GetAreaRotationPositon(Position, 1);
    }

    Vector2 GetAreaRotationPositon(Vector2 initialPosition, int direction)
    {
        Vector2 swappedPos = new(initialPosition.Y * -direction, initialPosition.X * direction);

        return swappedPos - Position;
    }

    public void UpdateSlamPosition()
    {
        //update our slam pos

        DropPreviewRaycast.ForceRaycastUpdate();
        if (DropPreviewRaycast.GetCollider() is not Node2D nodeHit) 
        { throw new NullReferenceException("FUCK slam raycast detected nothing"); }

        SlamOffset = nodeHit.GlobalPosition.Y - GlobalPosition.Y - 10;

    }

    #endregion
    #region === Godot Methods ===

    public override void _Process(double delta)
    {
        //run process event here
        //we need to make an event (empty by default) that runs every frame here for stuff like attack falling code using this block
        //no we dont lol
        //just make the attack run code every turn
    }

    public override void _Ready()
    {
        if (!SolidWhenFalling)
        {
            LeftArea.SetCollisionMaskValue(1, false);
            DownArea.SetCollisionMaskValue(1, false);
            RightArea.SetCollisionMaskValue(1, false);
            NegativeRotationArea.SetCollisionMaskValue(1, false);
            PositiveRotationArea.SetCollisionMaskValue(1, false);

            DropPreviewRaycast.SetCollisionMaskValue(1, false);
        }

        UpdateRotationDestinations();
    }
    #endregion
    #region === Visual Methods ===

    public void SetTexture(Rect2 rect)
    {
        if (OverrideTexture) { return; }

        if(SpriteLayer1 is Sprite2D)
        {
            (SpriteLayer1 as Sprite2D).RegionRect = rect;
        }

    }

    /// <summary>
    /// Set the color of all sprites adjusting different layers' color
    /// </summary>
    /// <param name="hue"></param>
    /// <param name="sat"></param>
    /// <param name="val"></param>
    public virtual void SetColor(float hue, float sat, float val)
    {
        if (OverrideColor) { return; }

        SpriteLayer1.SelfModulate = Color.FromHsv(hue, sat, val);

        SpriteLayer2.SelfModulate = Utils.GetSecondLayerColor(hue, sat, val);
    }

    public void UpdateSlamSprite(float yOffset)
    {
        DropPreviewSprite.Offset = new(0, yOffset);
    }

    public virtual void ToggleOffBoardTexture(bool toggleOn)
    {
        if (toggleOn)
        {
            HideSprites();
            AboveBoardIndicatorSprite.Visible = true;
        }
        else
        {
            UnhideSprites();
            AboveBoardIndicatorSprite.Visible = false;
        }
    }

    protected virtual void UnhideSprites()
    {
        SpriteLayer1.Visible = true;
    }

    protected virtual void HideSprites()
    {
        SpriteLayer1.Visible = false;
    }
    #endregion
    #region === Event Methods ===

    

    public virtual void OnTurnStart()
    {
        JustPlaced = false;
        IsTicked = false;
    }

    public void OnTickStep()
    {
        if (!IsTicked)
        {
            IsTicked = true;
            TickBlock();
        }
    }

    protected virtual void TickBlock()
    {

    }

    public void OnEnterBoard()
    {
        if (!ExitBoardBehaviorEnabled)
        {
            return;
        }
        HideSprites();
        ToggleOffBoardTexture(false);
    }

    public void OnExitBoard()
    {
        if (!ExitBoardBehaviorEnabled)
        {
            return;
        }
        HideSprites();
        ToggleOffBoardTexture(true);
    }

    public void OnAreaEntered(Area2D area)
    {
        if(area is Block && isPlaced)
        {
            //we want to ensure the placed block calls their method first, so we only call this is we are placed.
            //if the placed block doesn't delete the falling block, then the falling block's collision method will run after
            CollidedWithBlockWhilePlaced((Block)area);
        }
    }

    /// <summary>
    /// Runs first in a collision, calling the falling block's method unless said block gets removed
    /// </summary>
    /// <param name="fallingBlock"></param>
    public virtual void CollidedWithBlockWhilePlaced(Block fallingBlock)
    {
        //we didn't remove the falling block while colliding so we can still call their method
        fallingBlock.CollidedWithBlockWhileFalling(this);
    }

    /// <summary>
    /// Runs AFTER the placed block's method, and ONLY if the placed block does not terminate the falling block.
    /// </summary>
    /// <param name="placedBlock"></param>
    public virtual void CollidedWithBlockWhileFalling(Block placedBlock)
    {
        //nothing to do!
    }

    /// <summary>
    /// A falling block is trying to place itself on us! by default, we don't do anything and let the falling block sort it out
    /// </summary>
    /// <param name="fallingBlock"></param>
    public virtual void FallingBlockAttemptingPlacementOnUs(Block fallingBlock)
    {
        fallingBlock.AttemptedToPlaceIntoBlock(this);
    }

    /// <summary>
    /// We are being placed ontop of an existing block! By default we will delete ourselves to resolve the conflict
    /// </summary>
    /// <param name="placedBlock"></param>
    public virtual void AttemptedToPlaceIntoBlock(Block placedBlock)
    {
        //we are trying to place into a block! we must terminate instead
        QueueFree();
    }


    public void LeftAreaChanged(Area2D area)
    {
        if (LeftArea.HasOverlappingAreas()) { CanMoveLeft = false; } else { CanMoveLeft = true; }
    }

    public void DownAreaChanged(Area2D area)
    {
        if (DownArea.HasOverlappingAreas()) { CanMoveDown = false; } else { CanMoveDown = true; }
    }

    public void RightAreaChanged(Area2D area)
    {
        if (RightArea.HasOverlappingAreas()) { CanMoveRight = false; } else { CanMoveRight = true; }
    }

    public void NegativeRotationAreaChanged(Area2D area)
    {
        if (NegativeRotationArea.HasOverlappingAreas()) { CanRotateNegative = false; } else { CanRotateNegative = true; }
    }

    public void PositiveRotationAreaChanged(Area2D area)
    {
        if (PositiveRotationArea.HasOverlappingAreas()) { CanRotatePositive = false; } else { CanRotatePositive = true; }
    }
    #endregion
}

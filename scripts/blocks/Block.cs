using Godot;
using System;

[GlobalClass, Icon("res://texture/icon/blockicon.png")]
public partial class Block : Area2D
{
	public Vector2I BoardPos
    {
        get => CollisionData.BoardPos;
        set => CollisionData.BoardPos = value;
    }
    public BoardCollisionData CollisionData = new();

    public int LowerDistance = 0;

    [Signal] public delegate void PlacedEventHandler(Block block);
    [Signal] public delegate void ScoredEventHandler(Block block);
    [Signal] public delegate void MovedCellsEventHandler();
    [Signal] public delegate void DeletedEventHandler();

    bool _IsPlaced;
    public bool IsPlaced
    {
        get => _IsPlaced;
        set
        {
            //WHY WAS THIS SETTING TO TRUE????
            _IsPlaced = value;

            //swap our collision values depending on if we're placed or not
            SetCollisionLayerValue(1, !value);
            SetCollisionLayerValue(2, value);

            //if we're placed we want to be checking falling blocks (layer 1) and be solid on layer 2, but not layer 1
            SetCollisionMaskValue(1, value);
            SetCollisionMaskValue(2, !value);
        }
    }


    public bool JustPlaced = false;
    public bool IsScored = false;
    public bool IsTicked = false;
    bool ForceHidden = false;

    [Export] public BlockType type;

    /// <summary>
    /// If the block should collide with other blocks while falling
    /// </summary>
    [Export] public bool SolidWhenFalling = true;
    /// <summary>
    /// If falling blocks should collide with this block
    /// </summary>
    [Export] public bool SolidWhenPlaced = true;
    /// <summary>
    /// If true, non-solid blocks will not be allowed to move into us
    /// </summary>
    [Export] public bool AbsoluteSolid = false;
    /// <summary>
    /// The score value this block gives when scored
    /// </summary>
    [Export] public double ScoreValue = 1;
    /// <summary>
    /// Mult value added to turn level mult upon scoring
    /// </summary>
    [Export] public double MultValue = 0;
    /// <summary>
    /// If the block can fill a row for scoring
    /// </summary>
    [Export] public bool Scorable = true;
    /// <summary>
    /// If the block should be removed after being scored
    /// </summary>
    [Export] public bool RemovedOnScoring = true;
    /// <summary>
    /// If the block should tick before others. ONLY do this for destructive tick events!!!
    /// </summary>
    [Export] public bool PriorityTicker = false;

    /// <summary>
    /// If we should also rotate our block sprite when we rotate
    /// </summary>
    [Export] bool RotateSprite = false;
    [Export] bool OverrideTexture = false;
    [Export] bool OverrideColor = false;


    protected IBlockSprite BlockSprite;
    protected Sprite2D AboveBoardIndicatorSprite;
    protected Sprite2D SlamPreviewSprite;
    protected Sprite2D WhiteFlashSprite;
    protected PointLight2D WhiteFlashLight;

    static readonly CompressedTexture2D Atlas = ResourceLoader.Load<CompressedTexture2D>("uid://dib0bjbdg8no8");
    static readonly CompressedTexture2D GlowTexture64 = ResourceLoader.Load<CompressedTexture2D>("uid://bgnyfnd1av01w");
    static readonly CompressedTexture2D GlowTexture128 = ResourceLoader.Load<CompressedTexture2D>("uid://bgnyfnd1av01w");


    const float PlacementFlashLightEnergy = .12f; //brightness of placement flash's light
    const double PlacementFlashLength = .18;

    //Creates and sets up all basic required nodes
    public override void _Ready()
	{
        CollisionData.Solid = SolidWhenFalling;
        IsPlaced = false;

        //Get drop preview sprite. If nonsolid we have a different sprite
        Rect2 DropPreviewRect = new(60, 0, 10, 10);
        if (!SolidWhenFalling)
        {
            DropPreviewRect = new(50, 0, 10, 10);
        }

        //Set up all basic required nodes

        AboveBoardIndicatorSprite = new Sprite2D
        {
            Name = "AboveBoardIndicatorSprite",
            Texture = Atlas,
            RegionRect = new(20, 10, 10, 10),
            RegionEnabled = true,
            Visible = false
        };
        SlamPreviewSprite = new Sprite2D
        {
            Name = "SlamPreviewSprite",
            Texture = Atlas,
            RegionRect = DropPreviewRect,
            RegionEnabled = true,
            SelfModulate = new(1, 1, 1, 0.8f)
        };
        WhiteFlashSprite = new Sprite2D
        {
            Name = "WhiteFlashSprite",
            Texture = Atlas,
            RegionRect = new(0, 10, 10, 10),
            RegionEnabled = true,
            Visible = false
        };
        WhiteFlashLight = new PointLight2D()
        {
            Name = "WhiteFlashLight",
            Energy = PlacementFlashLightEnergy,
            Texture = GlowTexture64,
            Enabled = false
        };

        AddChild(AboveBoardIndicatorSprite);
        AddChild(SlamPreviewSprite);
        AddChild(WhiteFlashSprite);
        WhiteFlashSprite.AddChild(WhiteFlashLight);
    }

    public void Delete()
    {
        if (IsQueuedForDeletion())
        {
            return;
        }
        QueueFree();
        EmitSignalDeleted();
    }

    public void OnAreaEntered(Area2D area)
    {
        if(area is Block)
        {
            CollidedWithBlock(area as Block);
        }
    }

    #region === Movement ===

    public void Rotate(int direction)
    {
        Vector2 newPos = Utils.GetRotatedVector(Position, direction);
        Position = newPos;
        if (RotateSprite)
        {
            BlockSprite.Rotate(Mathf.DegToRad(90 * direction));
        }

    }


    #endregion
    #region === Placement & Collision ===

    /// <summary>
    /// Updates collision and position using new root position. Ran whenever our root moves or the board is updated
    /// </summary>
    /// <param name="RootPosition"></param>
    public void UpdateFallingCollision(Vector2 RootPosition)
    {
        CollisionData.UpdateFallingBlock(Position, RootPosition);

        UpdateOutsideBoardSprite();
        //check if we should use the offboard sprite here !!!!
    }

    public virtual void CollidedWithBlock(Block otherBlock, bool attemptingPlacement = false)
    {
        //todo: this bool is a bandaid fix to correct placed hologram blocks disappearing on touching stuff.
        //maybe find another way to do this?
    }

    public void UpdateOutsideBoardSprite()
    {
        bool OutsideBoardBounds = BoardAccessor.IsOutsideVisualBoard(BoardPos);

        ToggleOutsideBoardSprite(OutsideBoardBounds);
    }

    /// <summary>
    /// Place on the board, setting our values and telling the board
    /// </summary>
    /// <param name="DoPlacementFlash"></param>
    public void Place(bool DoPlacementFlash = true)
    {
        RunPlacementBehavior();

        SlamPreviewSprite.Visible = false;
        if (DoPlacementFlash)
        {
            FlashWhite();
        }
        EmitSignalPlaced(this);

        //set this AFTER we place ourselves! very important for collision events!
        IsPlaced = true;
        JustPlaced = true;
    }

    protected virtual void RunPlacementBehavior()
    {

    }


    #endregion
    #region === Board Events ===

    /// <summary>
    /// Reset certain values on turn started
    /// </summary>
    public void TurnStarted()
    {
        IsTicked = false;
    }

    /// <summary>
    /// Scores the block, running behavior and reporting to the board
    /// </summary>
    public void Score()
    {
        IsScored = true;
        RunScoreBehavior();
        EmitSignalScored(this);
    }
    /// <summary>
    /// Custom overridable scoring behavior
    /// </summary>
    protected virtual void RunScoreBehavior()
    {
        ForceHidden = true;
        ToggleVisibility(false);
    }

    /// <summary>
    /// Run block's tick behavior, if not already ticked this turn
    /// </summary>
    public void Tick(bool PriorityTick)
    {
        if(PriorityTick != PriorityTicker)
        {
            //if we don't match the current priority:
            //don't tick!
            return;
        }

        if (!IsTicked && !IsQueuedForDeletion())
        {
            RunTickBehavior();

            JustPlaced = false;
            IsTicked = true;
        }
    }
    /// <summary>
    /// Custom overridable ticking behavior
    /// </summary>
    protected virtual void RunTickBehavior()
    {

    }


    #endregion
    #region === Visuals ===

    public void AddBlockSprite(Node2D sprite)
    {
        if (sprite is not IBlockSprite)
        {
            GD.PushError("Block given invalid sprite!");
            return;
        }

        AddChild(sprite);
        BlockSprite = (IBlockSprite)sprite;
    }
    public void UpdateSlamSprite()
    {
        SlamPreviewSprite.Position = new(0, CollisionData.SlamYOffset * 10);
    }

    public void HideSlamSprite()
    {
        SlamPreviewSprite.Visible = false;
    }

    public void SetEnemySlamSprite()
    {
        SlamPreviewSprite.RegionRect = new(40, 10, 10, 10);
        SlamPreviewSprite.SelfModulate = new(1, 1, 1, 0.6f);
    }

    public void ToggleOutsideBoardSprite(bool outsideBoard)
    {
        AboveBoardIndicatorSprite.Visible = outsideBoard;
        ToggleVisibility(!outsideBoard);
    }

    /// <summary>
    /// Flash white briefly. Occurs on placement
    /// </summary>
    void FlashWhite()
    {
        ToggleVisibility(false);

        WhiteFlashSprite.Visible = true;
        WhiteFlashLight.Enabled = true;

        Tween tween = GetTree().CreateTween().SetParallel();
        tween.TweenCallback(Callable.From(() => WhiteFlashSprite.Visible = false)).SetDelay(PlacementFlashLength)
            .Finished += WhiteFlashEnded;

        //i think changing the energy harms performance
        /*
        tween.TweenProperty(WhiteFlashLight, "energy", 0, PlacementFlashLength)
            .Finished += WhiteFlashEnded; //reset brightness after animation is done*/
    }
    void WhiteFlashEnded()
    {
        //WhiteFlashLight.Energy = PlacementFlashLightEnergy;
        WhiteFlashLight.Enabled = false;
        ToggleVisibility(true); //this will break shit prob

    }

    public virtual void SetTexture(Rect2 region)
    {
        if (OverrideTexture) { return; }
        BlockSprite.SetTexture(region);
    }
    public virtual void SetTexture(Texture2D texture, Rect2 region)
    {
        if (OverrideTexture) { return; }
        BlockSprite.SetTexture(texture, region);
    }
    public virtual void SetColor(float hue, float sat, float val)
    {
        if (OverrideColor) { return; }
        BlockSprite.SetColor(hue, sat, val);
    }
    public virtual void ToggleVisibility(bool visible)
    {
        if (ForceHidden)
        {
            BlockSprite.ToggleVisibility(false);
            return;
        }

        BlockSprite.ToggleVisibility(visible);
    }
    #endregion

}

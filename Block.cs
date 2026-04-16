using Godot;
using System;
using System.Security.Cryptography;

[GlobalClass, Icon("res://texture/icon/blockicon.png")]
public partial class Block : Area2D
{
    public float SlamOffset;

    public bool CanMoveLeft = true;
    public bool CanMoveDown = true;
    public bool CanMoveRight = true;
    public bool CanRotateNegative = true;
    public bool CanRotatePositive = true;

    protected bool isPlaced = false;

    [Export] public bool Scorable = true;

    [Export] bool SolidWhenFalling = true;
    [Export] bool SolidWhenPlaced = true;
    [Export] bool OverrideColor = false;
    [Export] bool OverrideTexture = false;

    [Export] Sprite2D SpriteLayer1;
    [Export] Sprite2D SpriteLayer2;
    [Export] Sprite2D AboveBoardIndicatorSprite;
    [Export] Sprite2D DropPreviewSprite;
    [Export] Area2D LeftArea;
    [Export] Area2D DownArea;
    [Export] Area2D RightArea;
    [Export] Area2D NegativeRotationArea;
    [Export] Area2D PositiveRotationArea;
    [Export] RayCast2D DropPreviewRaycast;

    [Signal] public delegate void PlacedEventHandler(Block block);

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
        }

        UpdateRotationDestinations();
        //SetRandomColor();
    }

    public void UpdateSlamSprite(float yOffset)
    {
        DropPreviewSprite.Offset = new(0, yOffset);
    }


    // <([=========================|[ Board Interaction/Positional Methods ]|=========================])>
   
    public virtual void Play()
    {
        UpdateRotationDestinations();
        ToggleOffBoardTexture(true);
    }

    public virtual void Place()
    {
        SetCollisionLayerValue(1, SolidWhenPlaced); //we are now solid on the placedblocks layer

        DropPreviewRaycast.Enabled = false;
        DropPreviewSprite.Visible = false;

        LeftArea.ProcessMode = ProcessModeEnum.Disabled;
        RightArea.ProcessMode = ProcessModeEnum.Disabled;
        DownArea.ProcessMode = ProcessModeEnum.Disabled;
        NegativeRotationArea.ProcessMode = ProcessModeEnum.Disabled;
        PositiveRotationArea.ProcessMode = ProcessModeEnum.Disabled;

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

    /// ///=========================|[ Visual Methods ]|=========================\\\

    public void SetTexture(Rect2 rect)
    {
        if (OverrideTexture) { return; }

        SpriteLayer1.RegionRect = rect;
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


    /// ///=========================|[ Event Methods ]|=========================\\\

    public void OnEnterBoard()
    {
        ToggleOffBoardTexture(false);
    }

    public void OnExitBoard()
    {
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
}

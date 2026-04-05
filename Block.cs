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

    [Export] bool SolidWhenFalling;
    [Export] bool SolidWhenPlaced;

    [Export] Sprite2D SpriteLayer1;
    [Export] Sprite2D SpriteLayer2;
    [Export] Sprite2D SlamPreviewSprite;
    [Export] Area2D LeftArea;
    [Export] Area2D DownArea;
    [Export] Area2D RightArea;
    [Export] Area2D NegativeRotationArea;
    [Export] Area2D PositiveRotationArea;
    [Export] RayCast2D DropPreviewRaycast;

    [Signal] public delegate void OnPlacementEventHandler();

    public override void _Process(double delta)
    {
        //run process event here
        //we need to make an event (empty by default) that runs every frame here for stuff like attack falling code using this block
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
        SetRandomColor();
    }

    public void UpdateSlamSprite(float yOffset)
    {
        SlamPreviewSprite.Offset = new(0, yOffset);
    }


    // <([=========================|[ Board Interaction/Positional Methods ]|=========================])>
   
    public virtual void Play()
    {
        UpdateRotationDestinations();
    }

    public virtual void Place()
    {
        SetCollisionLayerValue(1, SolidWhenPlaced); //we are now solid on the placedblocks layer

        DropPreviewRaycast.Enabled = false;

        //do clipping? idk
    }

    public void Rotate(int direction = 1)
    {
        if(direction == -1)
        {
            Position = NegativeRotationArea.Position;
        }
        else if(direction == 1)
        {
            Position = PositiveRotationArea.Position;
        }

        else { throw new ArgumentOutOfRangeException($"Attempted to rotate block with invalid direction value: {direction}"); }

        //Move to new positions
        UpdateRotationDestinations();
    }

    protected void UpdateRotationDestinations()
    {
        NegativeRotationArea.Position = SwapPositionXY(Position, -1);
        PositiveRotationArea.Position = SwapPositionXY(Position, 1);
    }

    Vector2 SwapPositionXY(Vector2 initialPosition, int direction)
    {
        return new Vector2(initialPosition.Y * -direction, initialPosition.X * direction);
    }

    public void UpdateSlamPosition()
    {
        //update our slam pos

        DropPreviewRaycast.ForceRaycastUpdate();
        if (DropPreviewRaycast.GetCollider() is not Node2D nodeHit) 
        { throw new NullReferenceException("FUCK slam raycast detected nothing"); }

        SlamOffset = nodeHit.GlobalPosition.Y - GlobalPosition.Y - 10;

    }

    /// ///=========================|[ Event Methods ]|=========================\\\


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


    /// <summary>
    /// Set the color of all sprites adjusting different layers' color
    /// </summary>
    /// <param name="hue"></param>
    /// <param name="sat"></param>
    /// <param name="val"></param>
    public virtual void SetColor(float hue, float sat, float val)
    {
        SpriteLayer1.SelfModulate = Color.FromHsv(hue, sat, val);

        float hueOffset = 0.045f;

        //we want to make the color colder. if the hue is past the cold colors, we need to bring it back down towards them
        if(hue < 0.3f) { hueOffset = -hueOffset; }

        hue += hueOffset;

        if(hue < 0) { hue = 1 - hue; }

        //lower saturation and brightness
        sat -= 0.03f;
        val -= 0.12f;

        SpriteLayer2.SelfModulate = Color.FromHsv(hue, sat, val);
    }

    public void SetRandomColor()
    {
        (float, float, float) hsv = Utils.GetRandomBlockHSV();

        SetColor(hsv.Item1, hsv.Item2, hsv.Item3);
    }
}

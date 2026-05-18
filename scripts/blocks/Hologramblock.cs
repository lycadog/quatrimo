using Godot;
using System;

[GlobalClass]
public partial class Hologramblock : Block
{
    [Export] PointLight2D glowy;

    public override void CollidedWithBlock(Block otherBlock, bool attemptingPlacement = false)
    {
        if (IsPlaced && attemptingPlacement && otherBlock is not Hologramblock)
        {
            Delete();
        }
    }

    public override void SetColor(float hue, float sat, float val)
    {
        base.SetColor(hue, sat, val);
        glowy.Color = Color.FromHsv(hue, sat, val);
    }

    public override void ToggleVisibility(bool visible)
    {
        base.ToggleVisibility(visible);
        glowy.Enabled = visible;
    }

    
}

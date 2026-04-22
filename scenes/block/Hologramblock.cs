using Godot;
using System;

[GlobalClass]
public partial class Hologramblock : Block
{
    [Export] PointLight2D glowy;
    public override void FallingBlockAttemptingPlacementOnUs(Block fallingBlock)
    {
		QueueFree();
    }

    public override void SetColor(float hue, float sat, float val)
    {
        base.SetColor(hue, sat, val);
        glowy.Color = Color.FromHsv(hue, sat, val);
    }
}

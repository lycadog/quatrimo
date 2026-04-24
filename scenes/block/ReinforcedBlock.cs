using Godot;
using System;

public partial class ReinforcedBlock : Block
{
    [Export] GpuParticles2D particles;

    int scoringsLeft = 4;

    protected override void CustomScore()
    {
        scoringsLeft--;

        if(scoringsLeft < 0) { HideSprites(); RemovedOnScoring = true; return; }

        Sprite2D sprite1 = SpriteLayer1 as Sprite2D;
        Sprite2D sprite2 = SpriteLayer2 as Sprite2D;

        sprite1.RegionRect = new(sprite1.RegionRect.Position.X + 10, 0, 10, 10);
        sprite2.RegionRect = new(sprite2.RegionRect.Position.X + 10, 10, 10, 10);

        particles.Restart();
    }

    public override void SetColor(float hue, float sat, float val)
    {
        base.SetColor(hue, sat, val);
        particles.SelfModulate = Color.FromHsv(hue, sat, val);
    }
}

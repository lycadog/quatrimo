using Godot;
using System;

public partial class ReinforcedBlock : Block
{
    [Export] GpuParticles2D particles;

    int scoringsLeft = 4;

    Rect2 textureRect = new(160, 0, 10, 10);

    protected override void RunScoreBehavior()
    {
        scoringsLeft--;

        if(scoringsLeft < 0) { ToggleVisibility(false); RemovedOnScoring = true; return; }

        textureRect = new(textureRect.Position.X + 10, 0, 10, 10);
        Rect2 layer2Rect = new(textureRect.Position.X, 10, 10, 10);

        BlockSprite.SetTexture(textureRect);
        BlockSprite.SetSecondLayerTexture(layer2Rect);

        particles.Restart();
    }

    public override void SetColor(float hue, float sat, float val)
    {
        base.SetColor(hue, sat, val);
        particles.SelfModulate = Color.FromHsv(hue, sat, val);
    }
}

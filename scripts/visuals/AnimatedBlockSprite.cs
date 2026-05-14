using Godot;
using System;

[GlobalClass]
public partial class AnimatedBlockSprite : AnimatedSprite2D, IBlockSprite
{
    [Export] AnimatedSprite2D Layer2;
    [Export] bool OverrideColor;

    public void SetTexture(Rect2 region)
    {
        return;
    }

    public void SetTexture(Texture2D texture, Rect2 region)
    {
        return;
    }

    public void SetAnimation(SpriteFrames frames)
    {
        SpriteFrames = frames;
    }

    public void SetColor(float hue, float sat, float val)
    {
        if (OverrideColor)
        {
            return;
        }

        SelfModulate = Color.FromHsv(hue, sat, val);

        Layer2.SelfModulate = Utils.GetSecondLayerColor(hue, sat, val);
    }

    public void ToggleVisibility(bool visible)
    {
        Visible = visible;
    }

    public void SetSecondLayerTexture(Rect2 region)
    {
        return;
    }

    public void SetSecondLayerTexture(Texture2D texture, Rect2 region)
    {
        return;
    }
}

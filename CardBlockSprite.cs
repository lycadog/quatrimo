using Godot;
using System;

[GlobalClass, Icon("res://texture/icon/cardblockicon.png")]
public partial class CardBlockSprite : Sprite2D
{
    [Export] bool OverrideColor = false;
    [Export] Sprite2D SecondLayer;
    [Export] AnimatedSprite2D AnimatedSprite;

    public void SetColor(float h, float s, float v)
    {
        if (OverrideColor) { return; }

        Color color = Color.FromHsv(h, s, v);
        SelfModulate = color;
        SecondLayer.SelfModulate = Utils.GetSecondLayerColor(h, s, v);

        if(AnimatedSprite != null) { AnimatedSprite.SelfModulate = color; }

    }

}

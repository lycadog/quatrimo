using Godot;
using System;

[GlobalClass, Icon("res://texture/icon/cardblockicon.png")]
public partial class CardBlockSprite : Node2D
{
    /// <summary>
    /// If we should use scene color to override color set in code
    /// </summary>
    [Export] bool OverrideColor = false;
    [Export] Node2D FirstLayer;
    [Export] Node2D SecondLayer;

    public void SetColor(float h, float s, float v)
    {
        if (OverrideColor) { return; }

        Color color = Color.FromHsv(h, s, v);
        FirstLayer.SelfModulate = color;
        SecondLayer.SelfModulate = Utils.GetSecondLayerColor(h, s, v);
    }

}

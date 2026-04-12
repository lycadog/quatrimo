using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Utils
{
    public static Color GetRandomPieceColor()
    {
        (float, float, float) hsv = GetRandomPieceHSV();
        return Color.FromHsv(hsv.Item1, hsv.Item2, hsv.Item3);
    }

    public static (float, float, float) GetRandomPieceHSV()
    {
        (float, float, float) HSV = (
            GD.Randf(),
            1 - GD.RandRange(0, 1) * .015f,
            1 - GD.RandRange(0, 1) * .008f
            );

        return HSV;
    }

    /// <summary>
    /// Returns darkened color, used for block's darkened second layer
    /// </summary>
    /// <param name="hue"></param>
    /// <param name="saturation"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Color GetSecondLayerColor(float h, float s, float v)
    {
        float hueOffset = 0.045f;

        //we want to make the color colder. if the hue is past the cold colors, we need to bring it back down towards them
        if (h < 0.3f) { hueOffset = -hueOffset; }

        h += hueOffset;

        if (h < 0) { h = 1 - h; }

        //lower saturation and brightness
        s -= 0.03f;
        v -= 0.12f;

        return Color.FromHsv(h, s, v);
    }




}
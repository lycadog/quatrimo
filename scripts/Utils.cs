using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Utils
{
    public static Color GetRandomBlockColor()
    {
        (float, float, float) hsv = GetRandomBlockHSV();
        return Color.FromHsv(hsv.Item1, hsv.Item2, hsv.Item3);
    }

    public static (float, float, float) GetRandomBlockHSV()
    {
        (float, float, float) HSV = (
            GD.Randf(),
            1 - GD.RandRange(0, 1) * .015f,
            1 - GD.RandRange(0, 1) * .008f
            );

        return HSV;


    }




}
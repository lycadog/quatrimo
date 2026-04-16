using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class RunStats
{
    public static int HandDrawSize = 3;
    public static int CardCountRequiredBeforeDrawing = 0;


    public static void ResetValues()
    {
        HandDrawSize = 3;
        CardCountRequiredBeforeDrawing = 0;
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class RunStats
{
    /// <summary>
    /// Empty spaces allowed when scoring a row. Default: 0
    /// </summary>
    public static int EmptySpacesAllowedInScoring = 0;      
    /// <summary>
    /// Empty spaces required for us to score a row. Also functions as the above stat, adding an allowed empty space for every value.
    /// </summary>
    public static int EmptySpacesRequiredInScoring = 0;
    /// <summary>
    /// When we draw our hand, we draw this many cards
    /// </summary>
    public static int HandDrawSize = 3;
    /// <summary>
    /// If player has this many cards or less in hand they will draw
    /// </summary>
    public static int CardCountRequiredBeforeDrawing = 0;


    public static void ResetValues()
    {
        EmptySpacesAllowedInScoring = 0;
        EmptySpacesRequiredInScoring = 0;
        HandDrawSize = 3;
        CardCountRequiredBeforeDrawing = 0;
    }
}


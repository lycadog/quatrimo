using Godot;
using System;

public partial class Run : Node
{
    public static Run Current;

    /// <summary>
    /// Empty spaces allowed when scoring a row. Default: 0
    /// </summary>
    public int EmptySpacesAllowedInScoring = 0;
    /// <summary>
    /// Empty spaces required for us to score a row. Also functions as the above stat, adding an allowed empty space for every value.
    /// </summary>
    public int EmptySpacesRequiredInScoring = 0;

    /// <summary>
    /// When we draw our hand, we draw this many cards
    /// </summary>
    public int HandDrawSize = 3;
    /// <summary>
    /// If player has this many cards or less in hand they will draw
    /// </summary>
    public int CardCountRequiredBeforeDrawing = 0;

    /// <summary>
    /// Score the player has at the start of their turn
    /// </summary>
    public double BaseScore = 0;
    /// <summary>
    /// Mult the player has at the start of encounter
    /// </summary>
    public double BaseMult = 1;

    public int BaseLevel = 1;
    /// <summary>
    /// Multiplier added each level up
    /// </summary>
    public double MultPerLevel = 0.5;
    /// <summary>
    /// Scored rows required for each level up
    /// </summary>
    public int RowsNeededForLevelup = 10;
    /// <summary>
    /// Cleared rows required for the level multiplier to activate
    /// </summary>
    public int RowsNeededForMultiplier = 4;

    public override void _Ready()
    {
        Current = this;
    }
}

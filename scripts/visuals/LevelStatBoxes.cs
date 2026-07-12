using Godot;
using System;

public partial class LevelStatBoxes : Control
{
    [Export] public Label LevelLabel;
    [Export] public Label XPerLevelLabel;
    [Export] public Label RowsUntilLevelUpLabel;

    public int MultPerLevel;
    public int RowsRequiredEachlevelUp;

    //some of these ui elements need actual real score values internally since they have to change their numbers during animations
    //these numbers are originally sourced from board so they should be synced even though they are seperate
    //but we can't sync them directly from the board as they need to update on certain animations
    //like us levelling up from getting enough rows when the RowClearedDial reached a new number

    //todo: figure out how we should sync these values from the ones in the board.
    //some values should be synced immediately, like mult per level and rows required

    //however mult per level is shown - maybe when it changes we should show a graphic?
    //we will probably make the corresponding fields on board into properties for these synced values


    public void RowAdded()
    {
        
    }

    void LevelUp()
    {

    }

}

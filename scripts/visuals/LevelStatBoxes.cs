using Godot;
using System;

public partial class LevelStatBoxes : Control
{
    [Export] public Label LevelLabel;
    [Export] public Label MultPerLevelLabel;
    [Export] public Label RowsUntilLevelUpLabel;

    [Export] GpuParticles2D LevelUpParticles;
    [Export] AudioStreamPlayer LevelUpSFX;


    public void LevelUp()
    {
        //text is synced automatically, all we need to do here is play sounds and show visuals !!!
        LevelUpParticles.Restart();
        LevelUpSFX.Play();
    }

 



    //some of these ui elements need actual real score values internally since they have to change their numbers during animations
    //these numbers are originally sourced from board so they should be synced even though they are seperate
    //but we can't sync them directly from the board as they need to update on certain animations
    //like us levelling up from getting enough rows when the RowClearedDial reached a new number

    //todo: figure out how we should sync these values from the ones in the board.
    //some values should be synced immediately, like mult per level and rows required

    //however mult per level is shown - maybe when it changes we should show a graphic?
    //we will probably make the corresponding fields on board into properties for these synced values

    //IDEA: when these boxes levelup, they will fire an event - this event is acted upon by the board, which updates all numbers
    //itself. this means these ui elements need less internal values and desyncs will be unlikely, or at least short-lived
    //since the board handles the actual values when levelling up

    //EVEN BETTER IDEA: have RowsClearedDial run an event back to the board, then the board handles everything that happens
    //when rows happen. this MIGHT be a bad idea. more clutter on the board, but more central management of ui

}

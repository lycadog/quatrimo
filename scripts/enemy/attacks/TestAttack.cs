using Godot;
using System;

public partial class TestAttack : Attack
{
    public override void ExecuteAttack()
    {
        GD.Print("rurhhh im attacking!!!!");
        EmitSignalExecutionFinished();
    }
}

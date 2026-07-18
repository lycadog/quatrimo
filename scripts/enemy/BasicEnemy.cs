using Godot;
using System;

public partial class BasicEnemy : Enemy
{
    protected override void GetNewAttack()
    {
        int RandomIndex = GD.RandRange(0, AvailableAttacks.Count - 1);
        CurrentAttack = AvailableAttacks[RandomIndex];
    }
}

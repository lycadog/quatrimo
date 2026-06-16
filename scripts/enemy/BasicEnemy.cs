using Godot;
using System;

public partial class BasicEnemy : Enemy
{
    protected override Attack GetNewAttack()
    {
        return AvailableAttacks[GD.RandRange(0, AvailableAttacks.Count - 1)];
    }
}

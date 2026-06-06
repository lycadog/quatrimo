
using Godot;
using System;
using System.Numerics;

public abstract class Attack
{
    public int TurnsToExecute = 0;
    public int TurnsToReveal = 0;

    protected float IntensityFactor = 1;

    int MinTurnsToExecute, MaxTurnsToExecute;
    int MinTurnsToReveal, MaxTurnsToReveal;

    public event Action AttackCompleted;

    public virtual void StartAttack(int EnemyLevel)
    {
        TurnsToExecute = GD.RandRange(MinTurnsToExecute, MaxTurnsToExecute);
        MinTurnsToReveal = GD.RandRange(MinTurnsToReveal, MaxTurnsToReveal);

        float CooldownMultiplier = Math.Max(1 - EnemyLevel * 0.08f, 0.2f);

        TurnsToExecute *= (int)CooldownMultiplier;
        TurnsToReveal *= (int)CooldownMultiplier;
    }

    public virtual void Update(int EnemyLevel)
    {
        TurnsToExecute--;
        TurnsToReveal--;

        if(TurnsToExecute < 0)
        {
            IntensityFactor = 1 + EnemyLevel * 0.08f;
            ExecuteAttack();
        }
    }

    protected abstract void ExecuteAttack();
    

}
using Godot;
using System;

[GlobalClass]
public abstract partial class Attack : Node
{
    public int TurnsToExecute = 0;
    public int TurnsToReveal = 0;

    protected float IntensityFactor = 1;

    [Export] protected int MinTurnsToExecute = 10, MaxTurnsToExecute = 16;
    [Export] protected int MinTurnsToReveal = 6, MaxTurnsToReveal = 10;

    [Export] protected bool ScaleCooldownWithLevel = true;
    [Export] protected float CooldownScalingPerLevel = .05f;
    [Export] protected float MinimumCooldown = .2f;

    [Export] protected bool ScaleIntensityWithLevel = true;
    [Export] protected float IntensityPerLevel = .08f;
    [Export] protected float MaximumIntensity = 1000;

    public event Action ExecutionFinished;
    public event Action UpdatingFinished;

    public void StartAttack(int EnemyLevel)
    {
        TurnsToExecute = GD.RandRange(MinTurnsToExecute, MaxTurnsToExecute);
        MinTurnsToReveal = GD.RandRange(MinTurnsToReveal, MaxTurnsToReveal);

        if (ScaleCooldownWithLevel)
        {
            float CooldownMultiplier = Math.Max(1 - EnemyLevel * CooldownScalingPerLevel, MinimumCooldown);
            //scale cooldown to be 5% faster per level, maxxing out at 5x faster

            TurnsToExecute = Math.Max((int)CooldownMultiplier * TurnsToExecute, 1);
            TurnsToReveal = Math.Max((int)CooldownMultiplier * TurnsToReveal, 1);
        }
        
        if (ScaleIntensityWithLevel)
        {
            IntensityFactor = Math.Min(1 + EnemyLevel * IntensityPerLevel, MaximumIntensity);
        }

        SetupAttack();
    }

    protected virtual void SetupAttack() { }

    public void UpdateAttack()
    {
        TurnsToReveal--;
        if(TurnsToReveal <= 0) { return; }

        TurnsToExecute--;

        if(TurnsToExecute <= 0)
        {
            GD.Print("Executing attack!");

            //Execute Attack should trigger ExecutionFinished event !!!

            ExecuteAttack();
            return;
        }

        UpdatingFinished.Invoke();
    }

    public abstract void ExecuteAttack();

}

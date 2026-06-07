using Godot;
using System;

public abstract partial class Attack : Node
{
    public int TurnsToExecute = 0;
    public int TurnsToReveal = 0;

    protected float IntensityFactor = 1;


    [Export] int BecomesAvailableAtLevel = 0;
    [Export] int BecomesUnavailableAtLevel = 1000000;

    [Export] int MinTurnsToExecute = 10, MaxTurnsToExecute = 16;
    [Export] int MinTurnsToReveal = 6, MaxTurnsToReveal = 10;

    [Export] bool ScaleCooldownWithLevel = true;
    [Export] float CooldownScalingPerLevel = .05f;
    [Export] float MinimumCooldown = .2f;

    [Export] bool ScaleIntensityWithLevel = true;
    [Export] float IntensityPerLevel = .08f;
    [Export] float MaximumIntensity = 1000;


    public void StartAttack(int EnemyLevel)
    {
        TurnsToExecute = GD.RandRange(MinTurnsToExecute, MaxTurnsToExecute);
        MinTurnsToReveal = GD.RandRange(MinTurnsToReveal, MaxTurnsToReveal);

        if (ScaleCooldownWithLevel)
        {
            float CooldownMultiplier = Math.Max(1 - EnemyLevel * CooldownScalingPerLevel, MinimumCooldown);
            //scale cooldown to be 5% faster per level, maxxing out at 5x faster

            TurnsToExecute *= (int)CooldownMultiplier;
            TurnsToReveal *= (int)CooldownMultiplier;
        }
        
        if (ScaleIntensityWithLevel)
        {
            IntensityFactor = Math.Min(1 + EnemyLevel * IntensityPerLevel, MaximumIntensity);
        }
    }

    public void UpdateAttack()
    {
        TurnsToReveal--;
        if(TurnsToReveal > 0) { return; }

        TurnsToExecute--;

        if(TurnsToExecute <= 0)
        {
            ExecuteAttack();
        }
        
    }

    public abstract void ExecuteAttack();

}

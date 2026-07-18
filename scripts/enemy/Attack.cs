using Godot;
using System;

[GlobalClass]
public abstract partial class Attack : Node
{
    public int ChargeupTurnsLeft = 0;

    public float IntensityFactor = 1;

    [Export] protected int MinChargeupTime = 10, MaxChargeupTime = 16;
    [Export] protected int MinCooldownTime = 4, MaxCooldownTime = 8;

    [Export] protected bool ReduceChargeupTimeWithLevel = true;
    [Export] protected float ChargeupTimeReductionPerLevel = .05f;
    [Export] protected float MinimumReduction = .2f;

    [Export] protected bool ScaleIntensityWithLevel = true;
    [Export] protected float IntensityPerLevel = .08f;
    [Export] protected float MaximumIntensity = 1000;

    [Export] public Rect2 IconTextureRegion = new(0, 0, 26, 16);

    [Signal] public delegate void ExecutionFinishedEventHandler();
    [Signal] public delegate void UpdatingFinishedEventHandler();

    public void StartAttack(int EnemyLevel)
    {
        EnemyLevel--;
        //subtract 1 from level so scaling starts after level 1

        ChargeupTurnsLeft = GD.RandRange(MinChargeupTime, MaxChargeupTime);

        if (ReduceChargeupTimeWithLevel)
        {
            float ChargeupReduction = Math.Max(1 - EnemyLevel * ChargeupTimeReductionPerLevel, MinimumReduction);
            //scale cooldown to be 5% faster per level, maxxing out at 5x faster

            ChargeupTurnsLeft = Math.Max((int)ChargeupReduction * ChargeupTurnsLeft, 1);
        }
        
        if (ScaleIntensityWithLevel)
        {
            IntensityFactor = Math.Min(1 + EnemyLevel * IntensityPerLevel, MaximumIntensity);
        }

        SetupAttack();
    }

    public int GetCooldown()
    {
        return GD.RandRange(MinCooldownTime, MaxCooldownTime);
    }

    protected virtual void SetupAttack() { }

    public void TickDownTimers()
    {
        ChargeupTurnsLeft--;
    }

    public void UpdateAttack()
    {
        if (ChargeupTurnsLeft <= 0)
        {
            GD.Print("Executing attack!");

            //Execute Attack should trigger ExecutionFinished event !!!

            ExecuteAttack();
            return;
        }

        EmitSignalUpdatingFinished();
    }

    public abstract void ExecuteAttack();

}

using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public abstract partial class Enemy : Node
{
	public int Level;
	public double Health;
	
	protected int TurnNumber = 0;

	Attack _CurrentAttack;
	protected Attack CurrentAttack
	{
		get => _CurrentAttack;
		set
		{
			_CurrentAttack = value;
			value.StartAttack(Level);
			AttackPanel.SetNewAttack(CurrentAttack.IconTextureRegion, CurrentAttack.IntensityFactor);
            AttackPanel.UpdateTurnsLeft(CurrentAttack.ChargeupTurnsLeft);
        }
	}

    public bool AttackingThisTurn = false;
    protected int AttackCooldown = 18;

    [Export] public double MaxHealth;

    [Export] Attack[] Attacks;
    protected List<Attack> AvailableAttacks = [];

    [Export] protected int StartingLevel = 1;
	[Export] protected int MaxLevel = -1;

    [Export] protected bool ScaleLevelOvertime = true;
	[Export] protected int LevelupEveryXTurns = 30;

    [Export] protected float CooldownReductionPerLevel = 0.05f;
    [Export] protected bool ReduceCooldownPerLevel = true;

	public EnemyAttackPanel AttackPanel;

    [Signal] public delegate void TurnCompletedEventHandler();
	
	public void PlayTurn()
	{
		TurnNumber++;

        AttackingThisTurn = false;

        if (ScaleLevelOvertime)
        {
            ScaleLevel();
        }

        CustomTurnBehavior();

        AttackCooldown--;
        if (AttackCooldown > 0)
		{
			EmitSignalTurnCompleted();
			return;
		}
		else if(AttackCooldown == 0)
		{
            GetNewAttack();
            EmitSignalTurnCompleted();
            return;
		}

        //these are split into two otherwise we would incorrectly overwrite the panel cooldown state immediately after attacking
        CurrentAttack.TickDownTimers();
        AttackPanel.UpdateTurnsLeft(CurrentAttack.ChargeupTurnsLeft);

        CurrentAttack.UpdateAttack();



        if (CurrentAttack.ChargeupTurnsLeft == 1)
		{
			AttackingThisTurn = true;
		}
    }

	protected virtual void CustomTurnBehavior() { }

	public void Attack_Completed()
	{
        //subtract 1 from level so scaling starts after level 1
        float CooldownMultiplier = 1 - (CooldownReductionPerLevel * (Level - 1));

		if (!ReduceCooldownPerLevel)
		{
			CooldownMultiplier = 1;
		}

		AttackCooldown = Math.Max((int)(CurrentAttack.GetCooldown() * CooldownMultiplier), 1);

		AttackPanel.AttackOnCooldown();
		EmitSignalTurnCompleted();
	}

    protected virtual void ScaleLevel()
    {
        if (TurnNumber % LevelupEveryXTurns == 0)
        {
            if (Level == MaxLevel) { return; }
            Level++;
        }
    }

    protected abstract void GetNewAttack();

    #region == Initialization ==

    void InitializeAttacks()
    {
        foreach(var attack in Attacks)
		{
			RegisterAttack(attack);
		}
    }

    void RegisterAttack(Attack attack)
    {
        GD.Print("registering attack!");
        attack.ExecutionFinished += Attack_Completed;
        attack.UpdatingFinished += EmitSignalTurnCompleted;
        AvailableAttacks.Add(attack);
    }

    public override void _Ready()
	{
		Level = StartingLevel;
		InitializeAttacks();
	}

    public override void _EnterTree()
    {
		Health = MaxHealth;
    }

    #endregion
}

using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public abstract partial class Enemy : Node
{
	public int Level;

	protected int TurnNumber = 0;

	Attack _CurrentAttack;
	protected Attack CurrentAttack
	{
		get => _CurrentAttack;
		set
		{
			_CurrentAttack = value;
			value.StartAttack(Level);
			value.Completed += Attack_Completed;
		}
	}

	protected List<Attack> AllAttacks = [];
    protected List<Attack> AvailableAttacks = [];

	[Export] protected int StartingLevel = 1;
	[Export] protected int MaxLevel = -1;

	[Export] protected bool ScaleLevelOvertime = true;
	[Export] protected int LevelupEveryXTurns = 30;
	
	public void PlayTurn()
	{
		TurnNumber++;
        if (ScaleLevelOvertime)
        {
            ScaleLevel();
        }

		CustomTurnBehavior();
        CurrentAttack.UpdateAttack();
	}

	protected virtual void CustomTurnBehavior() { }

	public void Attack_Completed()
	{
		CurrentAttack = GetNewAttack();
	}

	protected abstract Attack GetNewAttack();

    protected virtual void ScaleLevel()
    {
        if (TurnNumber % LevelupEveryXTurns == 0)
        {
            if (Level == MaxLevel) { return; }
            Level++;
        }
    }

    void InitializeAttacks()
    {
        var children = GetChildren();
        foreach (var node in children)
        {
            if (node is Attack attack)
            {
                AvailableAttacks.Add(attack);
            }
        }
    }
    public override void _Ready()
	{
		Level = StartingLevel;
		InitializeAttacks();
	}
}

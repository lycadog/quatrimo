using Godot;
using System;
using System.Collections.Generic;

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

	[Export] int StartingLevel = 1;
	[Export] int MaxLevel = -1;

	[Export] bool ScaleLevelOvertime = true;
	[Export] int LevelupEveryXTurns = 30;
	
	void InitializeAttacks()
	{
		var children = GetChildren();
		foreach(var node in children)
		{
			if(node is Attack attack)
			{
				AvailableAttacks.Add(attack);
			}
		}
	}
	
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

	protected virtual void CustomTurnBehavior()
	{

	}


	protected virtual void ScaleLevel()
	{
		if(TurnNumber % LevelupEveryXTurns == 0)
		{
			Level++;
		}
	}

	public void Attack_Completed()
	{
		CurrentAttack = GetNewAttack();
	}

	protected abstract Attack GetNewAttack();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitializeAttacks();
	}
}

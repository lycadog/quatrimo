using Godot;
using System;

public abstract class EnemyOld
{
    public int Level = 1;
    protected int TurnNumber = 0;

    protected AttackOld[] Attacks;

    AttackOld _CurrentAttack;
    protected AttackOld CurrentAttack
    {
        get => _CurrentAttack;
        set
        {
            _CurrentAttack = value;
            _CurrentAttack.StartAttack(Level);
        }
    }
    
    public string NameKey;
    
    public event Action<AttackOld> AttackUpdated;

    protected abstract AttackOld[] GetAttacks();
    protected void AddNewAttack(AttackOld attack)
    {
        attack.AttackCompleted += Attack_Completed;
    }


    /// <summary>
    /// Run enemy turn, updating and using attacks as needed. DO NOT run repeatedly!!!!
    /// </summary>
    public void PlayTurn()
    {
        TurnNumber++; //use this for level up behavior
        CustomTurnBehavior(); //and this too

        CurrentAttack.Update(Level);
        AttackUpdated.Invoke(CurrentAttack);
    }

    public virtual void Attack_Completed()
    {
        CurrentAttack = DrawNewAttack();
    }
    protected virtual AttackOld DrawNewAttack()
    {
        //get random attack
        return Attacks[GD.RandRange(0, Attacks.Length - 1)];
    }

    protected virtual void CustomTurnBehavior()
    {
        //by default: level up every 20 turns
        if(TurnNumber % 20 == 0)
        {
            Level++;
        }
    }

    


}
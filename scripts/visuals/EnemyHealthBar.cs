using Godot;
using System;

public partial class EnemyHealthBar : Control
{
    int HP, MaxHP;

    [Export] Label HPTextBlack;
    [Export] Label HPTextWhite;
    [Export] ColorRect HPRect;

    public const double BarChangeTime = 0.8;
    

    public void DealDamage(double damage)
    {
        
        if(damage > 0)
        {
            DamageAnimation();
        }

        int oldHP = HP;
        HP -= (int)damage;

        float BarSize = HP / (float)MaxHP * 117f;

        Tween tween = GetTree().CreateTween().SetParallel();
        tween.TweenMethod(Callable.From<int>(TweenText), oldHP, HP, BarChangeTime);
        tween.TweenProperty(HPRect, "size:x", BarSize, BarChangeTime).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
    }

    public override void _Ready()
    {
        HP = 140;
        MaxHP = 140;
        UpdateText();
    }

    void DamageAnimation()
    {

    }


    void UpdateText()
    {
        HPTextBlack.Text = HP.ToString();
        HPTextWhite.Text = HPTextBlack.Text;
    }

    void TweenText(int number)
    {
        HPTextBlack.Text = number.ToString();
        HPTextWhite.Text = HPTextBlack.Text;
    }
}

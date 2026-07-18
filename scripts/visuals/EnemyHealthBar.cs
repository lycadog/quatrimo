using Godot;
using System;

public partial class EnemyHealthBar : Control
{
    int HP, MaxHP;

    [Export] Label HPTextBlack;
    [Export] Label HPTextWhite;
    [Export] ColorRect HPRect;
    [Export] AudioStreamPlayer HPDamageSFX;
    
    public void DealDamage(double damage, double TickdownTime)
    {
        
        if(damage > 0)
        {
            DamageAnimation();
            HPDamageSFX.Play();
        }

        int oldHP = HP;
        HP -= (int)damage;

        float BarSize = HP / (float)MaxHP * 117f;

        Tween tween = GetTree().CreateTween().SetParallel();
        tween.TweenMethod(Callable.From<int>(TweenText), oldHP, HP, TickdownTime);
        tween.TweenProperty(HPRect, "size:x", BarSize, TickdownTime).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
    }

    public void SetupBar(double hp, double maxHP)
    {
        HP = (int)hp;
        MaxHP = (int)maxHP;
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

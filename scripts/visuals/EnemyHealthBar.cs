using Godot;
using System;

public partial class EnemyHealthBar : Control
{
    int HP, MaxHP;

    [Export] Label HPTextBlack;
    [Export] Label HPTextWhite;
    [Export] ColorRect HPRect;

    const double BarChangeTime = 0.4;

    public void AddHealth(double hp)
    {
        if(hp < 0)
        {
            DamageAnimation();
        }

        HP += (int)hp;
        UpdateText();

        float BarSize = (HP / (float)MaxHP) * 117f;

        GD.Print("hp size: " + BarSize);

        GetTree().CreateTween().TweenProperty(HPRect, "size:x", BarSize, BarChangeTime).SetTrans(Tween.TransitionType.Cubic);
    }

    public override void _Ready()
    {
        HP = 10000;
        MaxHP = 10000;
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
}

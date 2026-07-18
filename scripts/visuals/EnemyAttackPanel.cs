using Godot;
using System;

public partial class EnemyAttackPanel : Control
{
    [Export] Label TurnsLeftNumberLabel;
    [Export] Label TurnsTextLabel;

    [Export] AtlasTexture AttackIconTexture;

    [Export] TextureRect IncomingWarning;
    [Export] ColorRect Intensity1Glowy;
    [Export] ColorRect Intensity2Glowy;
    [Export] ColorRect Intensity3Glowy;

    public void UpdateTurnsLeft(int turns)
    {
        TurnsLeftNumberLabel.Text = turns.ToString();
        if(turns == 1)
        {
            TurnsTextLabel.Text = "TURN";
        }
        else
        {
            TurnsTextLabel.Text = "TURNS";
        }

        if(turns <= 3)
        {
            IncomingWarning.Visible = true;
        }
    }

    public void AttackOnCooldown()
    {
        TurnsLeftNumberLabel.Text = "???";
        TurnsTextLabel.Text = "TURNS";
        AttackIconTexture.Region = new(0, 0, 26, 16);

        IncomingWarning.Visible = false;

        Intensity1Glowy.Visible = false;
        Intensity2Glowy.Visible = false;
        Intensity3Glowy.Visible = false;
    }

    public void SetNewAttack(Rect2 region, float intensity)
    {
        AttackIconTexture.Region = region;

        //gotta just tank the if chain
        if(intensity >= 1.4f)
        {
            Intensity1Glowy.Visible = true;
        }
     
        if (intensity >= 1.8f)
        {
            Intensity2Glowy.Visible = true;
        }
    
        if (intensity >= 2.4f)
        {
            Intensity3Glowy.Visible = true;
        }

    }
}

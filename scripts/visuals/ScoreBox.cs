using Godot;
using System;

public partial class ScoreBox : TextureRect
{
    [Export] public Label ScoreLabel;
    [Export] public Label MultiplierLabel;

    [Export] TextureRect XSprite;
    [Export] TextureRect ScaledXSprite;
    [Export] TextureRect ColoredMultGlowy;

    [Export] ColorRect GlowyClippingRect;
    [Export] GpuParticles2D XParticles;

    bool AlreadyScored4 = false;

    const double GlowySpreadSpeed = 0.25;

    public void SetScore(double score)
    {
        ScoreLabel.Text = ((int)score).ToString();
    }

    public void SetMult(double mult)
    {
        MultiplierLabel.Text = mult.ToString();
    }

    public void RowScored(short RowsScored)
    {
        GD.Print("Score box reached rows: " + RowsScored);
        if(RowsScored >= 4)
        {
            if (!AlreadyScored4)
            {
                GetTree().CreateTween().TweenProperty(GlowyClippingRect, "size:x", 40, GlowySpreadSpeed)
                    .SetTrans(Tween.TransitionType.Cubic)
                    .Finished += SetXGlow;
            }

            AlreadyScored4 = true;
            ColoredMultGlowy.Modulate = new Color(.957f, .173f, .4f);

            if(RowsScored >= 8)
            {
                ColoredMultGlowy.Modulate = new Color(.973f, .596f, .322f);
            }
            else if(RowsScored >= 6)
            {
                ColoredMultGlowy.Modulate = new Color(.22f, .776f, .518f);
            }
        }
    }

    void SetXGlow()
    {
        XParticles.Restart();
        XSprite.SelfModulate = Colors.White;
    }

    void TurnOffGlowies()
    {
        AlreadyScored4 = false;
        XSprite.SelfModulate = new Color(.188f, .159f, .168f);
        GlowyClippingRect.Size = new(0, 17);
    }

    /// <summary>
    /// Reset per-turn effects like multiplied values or temporary effects
    /// </summary>
    public void ResetValues(double baseScore, double baseMult)
    {
        TurnOffGlowies();

        ScoreLabel.Text = ((int)baseScore).ToString();
        MultiplierLabel.Text = baseMult.ToString();
    }

    /// <summary>
    /// Execute multiplication animations, showing the specified value afterwards as the result
    /// </summary>
    /// <param name="finalScore"></param>
    public void MultiplyScore(double finalScore)
    {
        ScoreLabel.Text = ((int)finalScore).ToString();

        ScaledXSprite.Scale = Vector2.One;
        ScaledXSprite.SelfModulate = Colors.White;

        XParticles.Restart();

        Tween tween = GetTree().CreateTween().SetParallel(true);
        tween.TweenProperty(ScaledXSprite, "scale", new Vector2(4, 4), 0.6).SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
        tween.TweenProperty(ScaledXSprite, "self_modulate", new Color(0.7f, 0.7f, 0.9f, 0), 0.6);
    }

}

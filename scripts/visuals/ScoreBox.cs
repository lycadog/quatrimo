using Godot;
using System;

public partial class ScoreBox : TextureRect
{
    [Export] public Label ScoreLabel;
    [Export] public Label MultiplierLabel;

    [Export] TextureRect XSprite;
    [Export] GpuParticles2D XParticles;
    [Export] TextureRect AnimatedXSprite;

    [Export] TextureRect StaticGlowy;
    [Export] TextureRect StaticColoredGlowy;

    [Export] ColorRect AnimatedGlowyClippingRect;
    [Export] TextureRect AnimatedColoredGlowy;

    [Signal] public delegate void AnimationCreatedEventHandler();
    [Signal] public delegate void AnimationFinishedEventHandler();

    [Signal] public delegate void FinishedProcessingScoreEventHandler();

    [Signal] public delegate void ResetFinishedEventHandler();

    int ScoreNumber = 0;

    const double GlowySpreadDuration = 0.25;
    const double MultiplicationAnimationDuration = 0.6;
    const double WaitTimeAfterProcessing = 0.4;

    public const double ResetTime = 0.8;

    static Color XOfflineColor = new(.188f, .159f, .168f);

    public override void _Ready()
    {
        ScoreLabel.Text = Run.Current.BaseScore.ToString();
        MultiplierLabel.Text = Run.Current.BaseMult.ToString();
    }

    public void SetScore(double score)
    {
        ScoreNumber = (int)score;
        ScoreLabel.Text = ScoreNumber.ToString();
        
        if(ScoreNumber >= 0)
        {
            ScoreLabel.SelfModulate = Colors.White;
        }
        else
        {
            ScoreLabel.SelfModulate = new Color(.9f, .1f, .1f);
        }
    }

    public void SetMult(double mult, bool temporary = false)
    {
        MultiplierLabel.Text = mult.ToString();
        if (temporary)
        {
            //change text to yellow if temp mult is added
            return;
        }
    }

    public void RowScored(short RowsScored)
    {
        if (RowsScored == 4)
        {
            TweenGlowies(new Color(.957f, .173f, .4f));
        }
        else if (RowsScored == 6)
        {
            TweenGlowies(new Color(.22f, .776f, .518f));
        }
        else if(RowsScored == 8)
        {
            TweenGlowies(new Color(.973f, .596f, .322f));
        }
        else if(RowsScored == 10)
        {
            TweenGlowies(Colors.White);
        }
    }

    void TweenGlowies(Color color)
    {
        EmitSignalAnimationCreated();
        GetTree().CreateTween().TweenProperty(AnimatedGlowyClippingRect, "size:x", 40, GlowySpreadDuration)
                    .SetTrans(Tween.TransitionType.Cubic)
                    .Finished += GlowyDoneTweening;
        AnimatedColoredGlowy.Modulate = color;
    }

    void GlowyDoneTweening()
    {
        EmitSignalAnimationFinished();
        XParticles.Restart();
        XSprite.SelfModulate = Colors.White;

        AnimatedGlowyClippingRect.Size = new(0, 17);
        StaticGlowy.Visible = true;
        StaticColoredGlowy.Modulate = AnimatedColoredGlowy.Modulate;
    }

    void TurnOffGlowies()
    {
        if(StaticGlowy.Visible == false)
        {
            EmitSignalResetFinished();
            return;
        }

        Tween tween = GetTree().CreateTween().SetParallel();
        tween.TweenProperty(XSprite, "self_modulate", XOfflineColor, ResetTime).SetTrans(Tween.TransitionType.Quint)
            .Finished += EmitSignalResetFinished;

        StaticGlowy.Visible = false;
        AnimatedGlowyClippingRect.Size = new(40, 17);
        tween.TweenProperty(AnimatedGlowyClippingRect, "size:x", 0, ResetTime - 0.2).SetTrans(Tween.TransitionType.Cubic);
    }

    /// <summary>
    /// Reset per-turn effects like multiplied values or temporary effects
    /// </summary>
    public void ResetValues(double Mult)
    {
        ScoreLabel.SelfModulate = new Color(.58f, .48f, .48f);
        ScoreLabel.Text = Run.Current.BaseScore.ToString();
        MultiplierLabel.Text = Mult.ToString();

        TurnOffGlowies();
    }

    public void ProcessScore(double finalScore, bool isMultiplied)
    {
        /* THIS should be unnecessary due to score checks inside board! TODO re-evaluate later
        if(finalScore == 0)
        {
            EmitSignalFinishedProcessingScore();
            return;
        }*/

        if (!isMultiplied)
        {
            ScoreNumber = (int)finalScore;
            ScoreLabel.Text = ScoreNumber.ToString();
            GetTree().CreateTween().TweenCallback(
                Callable.From(EmitSignalFinishedProcessingScore))
                .SetDelay(WaitTimeAfterProcessing);
        }
        else
        {
            //multiply!
            //add a small delay here before this starts!
            CreateTween().TweenCallback(Callable.From(() => 
            MultiplyScore(finalScore)))
                .SetDelay(0.6);
        }
    }

    /// <summary>
    /// Execute multiplication animations, showing the specified value afterwards as the result
    /// </summary>
    /// <param name="finalScore"></param>
    void MultiplyScore(double finalScore)
    {
        ScoreNumber = (int)finalScore;
        ScoreLabel.Text = ScoreNumber.ToString();

        AnimatedXSprite.Scale = Vector2.One;
        AnimatedXSprite.SelfModulate = Colors.White;

        XParticles.Restart();

        Tween tween = GetTree().CreateTween().SetParallel(true);
        tween.TweenProperty(AnimatedXSprite, "scale", new Vector2(4, 4), MultiplicationAnimationDuration)
            .SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
        tween.TweenProperty(AnimatedXSprite, "self_modulate", new Color(0.7f, 0.7f, 0.9f, 0), MultiplicationAnimationDuration);

        //after a delay, finish the processing calling back to board to damage the enemy!
        tween.TweenCallback(Callable.From(EmitSignalFinishedProcessingScore))
            .SetDelay(MultiplicationAnimationDuration + WaitTimeAfterProcessing);
    }

    /// <summary>
    /// Tick down score! This should be in sync with the enemy being damaged !!!!
    /// </summary>
    public void TickDownScore(double TickDownTime)
    {
        //TODO: adjust tickdown time!
        //tick score down!
        GetTree().CreateTween().TweenMethod(Callable.From<int>(SetLabelNumber), ScoreNumber, Run.Current.BaseScore,
            TickDownTime)
            .Finished += TickingDownFinished;
    }

    void TickingDownFinished()
    {
        ScoreLabel.SelfModulate = new Color(.58f, .48f, .48f);
    }

    void SetLabelNumber(int number)
    {
        ScoreLabel.Text = number.ToString();
    }

}

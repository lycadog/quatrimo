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

    [Signal] public delegate void TurnEndAnimationCreatedEventHandler();
    [Signal] public delegate void TurnEndAnimationCompletedEventHandler();

    [Signal] public delegate void StartedTickingDownEventHandler();
    [Signal] public delegate void FinishedTickingDownEventHandler();

    int ScoreNumber = 0;

    const double GlowySpreadDuration = 0.25;
    const double MultiplicationAnimationDuration = 0.6;
    const double WaitTimeBeforeTickingDown = 0.4;

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
        GD.Print("Score box reached rows: " + RowsScored);

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
    }

    void TweenGlowies(Color color)
    {
        EmitSignalTurnEndAnimationCreated();
        GetTree().CreateTween().TweenProperty(AnimatedGlowyClippingRect, "size:x", 40, GlowySpreadDuration)
                    .SetTrans(Tween.TransitionType.Cubic)
                    .Finished += GlowyDoneTweening;
        AnimatedColoredGlowy.Modulate = color;
    }

    void GlowyDoneTweening()
    {
        EmitSignalTurnEndAnimationCompleted();
        XParticles.Restart();
        XSprite.SelfModulate = Colors.White;

        AnimatedGlowyClippingRect.Size = new(0, 17);
        StaticGlowy.Visible = true;
        StaticColoredGlowy.Modulate = AnimatedColoredGlowy.Modulate;
    }

    void TurnOffGlowies()
    {
        XSprite.SelfModulate = new Color(.188f, .159f, .168f);
        StaticGlowy.Visible = false;
        AnimatedGlowyClippingRect.Size = new(0, 17);
    }

    /// <summary>
    /// Reset per-turn effects like multiplied values or temporary effects
    /// </summary>
    public void ResetValues(double Mult)
    {
        TurnOffGlowies();
        ScoreLabel.SelfModulate = new Color(.58f, .48f, .48f);
        MultiplierLabel.Text = Mult.ToString();
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

        tween.TweenCallback(Callable.From(TickDownScore)).SetDelay(MultiplicationAnimationDuration + WaitTimeBeforeTickingDown);
    }

    public void ProcessScore(double finalScore, bool isMultiplied)
    {
        if(finalScore == 0)
        {
            EmitSignalFinishedTickingDown();
            return;
        }

        if (!isMultiplied)
        {
            ScoreNumber = (int)finalScore;
            ScoreLabel.Text = ScoreNumber.ToString();
            GetTree().CreateTween().TweenCallback(
                Callable.From(TickDownScore))
                .SetDelay(WaitTimeBeforeTickingDown);
        }
        else
        {
            //add a small delay here before this starts!
            CreateTween().TweenCallback(Callable.From(() => 
            MultiplyScore(finalScore)))
                .SetDelay(0.6);
        }
    }
    
    /// <summary>
    /// Tick down score, calling a signal that should also damage the enemy
    /// </summary>
    void TickDownScore()
    {
        EmitSignalStartedTickingDown();
        //tick score down!
        GetTree().CreateTween().TweenMethod(Callable.From<int>(SetLabelText), ScoreNumber, Run.Current.BaseScore,
            EnemyHealthBar.BarChangeTime)
            .Finished += TickingDownFinished;
    }

    void TickingDownFinished()
    {
        EmitSignalFinishedTickingDown();
        ScoreLabel.SelfModulate = new Color(.58f, .48f, .48f);
    }

    void SetLabelText(int text)
    {
        ScoreLabel.Text = text.ToString();
    }

}

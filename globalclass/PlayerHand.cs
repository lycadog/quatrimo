using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Godot.OpenXRInterface;

[GlobalClass, Icon("res://texture/icon/handicon.png")]
public partial class PlayerHand : Container
{
	[Export] float NewCardYPosition;
    [Signal] public delegate void DrawOperationStartedEventHandler();
    [Signal] public delegate void DrawOperationCompletedEventHandler();

	[Signal] public delegate void CardPlayedEventHandler(PieceCard card);   //Signal to the board that we be playing something!

	PlayerBag Bag;

    public List<PieceCard> Hand = [];
    Stack<PieceCard> queuedCards = [];

    int _TotalCards;
	int TotalCards
    {
        get => _TotalCards;
        set
        {
            ChangeInCardCount = Math.Abs(_TotalCards - value);
            _TotalCards = value;
        }
    }

    int ChangeInCardCount;

    int TotalCardsDiscarding = 0;
    int CardsFinishedDiscarding = 0;

    const double DrawAnimationLength = .4f;
	const double ScaleTweenLength = .3f;
	const double MoveHandAnimationLength = .25f;
	const double TimeBeforeDrawingNextCard = .22f;

    float Spacing = 8;
	bool CurrentlyDrawing = false; //if we are animating cards being added or not. this extends further than just drawing the hand
    bool CurrentlyDiscarding = false;
    
	public bool InputEnabled = false;
    int SelectionIndex = -1;


    #region === Logic & Godot Methods ===

    public override void _EnterTree()
    {
        Bag = Data.magnetBag.CreateBag();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Position = new(0, GetNewHandYPosition());
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        TryStartDrawing();

        //UpdateCards();

        if (InputEnabled && !CurrentlyDrawing)
        {
            CheckInput();
        }

    }

    void CheckInput()
    {
        if (Input.IsActionJustPressed("One"))
        {
            SelectCard(0);
        }
        else if (Input.IsActionJustPressed("Two"))
        {
            SelectCard(1);
        }
        else if (Input.IsActionJustPressed("Three"))
        {
            SelectCard(2);
        }
        else if (Input.IsActionJustPressed("Four"))
        {
            SelectCard(3);
        }
        else if (Input.IsActionJustPressed("Five"))
        {
            SelectCard(4);
        }
        else if (Input.IsActionJustPressed("Six"))
        {
            SelectCard(5);
        }
        else if (Input.IsActionJustPressed("Up"))
        {
            if (SelectionIndex == -1) //if no card selected: select the bottom-most card
            {
                SelectCard(Hand.Count - 1);
            }
            else
            {
                SelectCard(SelectionIndex - 1);
            }
        }
        else if (Input.IsActionJustPressed("Down"))
        {
            SelectCard(SelectionIndex + 1);
        }

        if (Input.IsActionJustPressed("SlamPiece"))
        {
            if (SelectionIndex != -1)
            {
                PlayCard(Hand[SelectionIndex]);
                return;
            }
        }

        if (Input.IsActionJustPressed("Debug1"))
        {
            DiscardHand();
            DrawHand();
        }
    }

    void UpdateHand()
    {
        TotalCards = Hand.Count + queuedCards.Count;

        Tween tween = GetTree().CreateTween().SetParallel();
        SetNewSpacing();

        float yOffset = GetNewHandYPosition();

        tween.TweenProperty(this, "position", new Vector2(Position.X, yOffset), 
            MoveHandAnimationLength).SetTrans(Tween.TransitionType.Quad);

        for (int i = 0; i < Hand.Count; i++)
        {
            UpdateCard(Hand[i], i, tween);
        }
    }

    void UpdateCard(PieceCard card, int newIndex, Tween tween)
    {
        card.SetIndex(newIndex);

        tween.TweenProperty(card, "BaseYPosition",
            GetCardBaseY(newIndex), MoveHandAnimationLength + ChangeInCardCount * .08).SetTrans(Tween.TransitionType.Quad);
    }

    #endregion
    #region === Event Stuff ===

    public void OnTurnStart()
    {
        InputEnabled = true;
        if (Hand.Count <= RunStats.CardCountRequiredBeforeDrawing)
        {
            DrawHand();
        }
    }

    public void OnPiecePlaced()
    {
        DiscardCard(SelectionIndex);
    }

    void OnDrawTweenFinished()
    {
        if (queuedCards.Count == 0 && CurrentlyDrawing)
        {
            CurrentlyDrawing = false;
            EmitSignalDrawOperationCompleted();
        }
    }

    public void OnCardDiscardComplete()
    {
        CardsFinishedDiscarding++;
        if (CardsFinishedDiscarding == TotalCardsDiscarding)
        {
            CurrentlyDiscarding = false;
            CardsFinishedDiscarding = 0;
            TotalCardsDiscarding = 0;

            if(queuedCards.Count == 0)
            {
                UpdateHand();
            }   
        }
    }

    #endregion
    #region === Drawing & Removal ===



    public void AddToHand(PieceCard card)
    {
        queuedCards.Push(card);
        card.DiscardAnimationComplete += OnCardDiscardComplete;
    }

    public void AddToHand(BagPiece piece)
    {
        AddToHand(PieceCard.CreateNewCard(piece));
    }

    public void DrawHand()
    {
        for (int i = 0; i < RunStats.HandDrawSize; i++)
        {
            AddToHand(Bag.DrawRandomCard());
        }
    }

    public void DiscardCard(int index)
    {
        if (index < 0 || index >= Hand.Count)
        {
            GD.PushError("Attempted to discard card at invalid index " + index + "!");
            return;
        }
        PieceCard card = Hand[index];
        card.Discard();
        Hand.RemoveAt(index);

        CurrentlyDiscarding = true;
        TotalCardsDiscarding++;

        if (index == SelectionIndex) { SelectionIndex = -1; }

        return;
    }

    /// <summary>
    /// Debug feature, do not actually use
    /// </summary>
    void DiscardHand()
    {
        while(Hand.Count > 0)
        {
            DiscardCard(0);
        }
    }

    #region << Drawing Logic >>
    void TryStartDrawing() //start drawing everything thats queued up and move the hand to match
    {
        if (!CurrentlyDrawing && queuedCards.Count > 0 && !CurrentlyDiscarding)
        {
            EmitSignalDrawOperationStarted();
            CurrentlyDrawing = true;

            Tween tween = GetTree().CreateTween().SetParallel();

            //setup our bitches
            for (int i = 1; i < queuedCards.Count; i++)
            {
                tween.TweenCallback(Callable.From(StartDrawingNextCard)).SetDelay(TimeBeforeDrawingNextCard * i);
            }

            UpdateHand();

            StartDrawingNextCard();
        }
    }

    void StartDrawingNextCard()
    {
        //get our next card and draw it, setting up tweens for all the add to hand animations and such

        PieceCard card = queuedCards.Pop();
        AddChild(card);

        card.Position = new Vector2(0, 1000);

        card.Visible = true;
        Hand.Add(card);

        card.AnimatedYOffset = 130 - Hand.Count * 17; //TODO: sync these positions up with real objects/global position

        card.SetIndex(Hand.Count - 1);
        card.BaseYPosition = GetCardBaseY(card.index);

        card.Scale = new(.7f, .7f);

        Tween tween = GetTree().CreateTween().SetParallel();

        tween.TweenProperty(card, "AnimatedYOffset", 0, DrawAnimationLength).SetTrans(Tween.TransitionType.Quad).Finished += OnDrawTweenFinished;
        tween.TweenProperty(card, "scale", new Vector2(1, 1), ScaleTweenLength);
    }

    #endregion
    #endregion
    #region === Card Methods & Visuals ===

    void PlayCard(PieceCard card)
    {
        EmitSignalCardPlayed(card);
        InputEnabled = false;
    }

    void SelectCard(int index)
    {
        if (index < Hand.Count && index > -1)
        {
            if (SelectionIndex != -1)
            {
                Hand[SelectionIndex].Deselect();

                if (index == SelectionIndex) { SelectionIndex = -1; return; }
            }

            Hand[index].Select();
            SelectionIndex = index;
        }
    }

    #endregion

    float GetCardBaseY(int index)
    {
        return (34 + Spacing) * index;
    }

	void SetNewSpacing()
	{
		Spacing = Math.Max(8 - (Hand.Count + queuedCards.Count - 3) * 2.5f, 0);
	}

    float GetNewHandYPosition()
    {
        return -(TotalCards * 17);
    }

}

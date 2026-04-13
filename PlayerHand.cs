using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

[GlobalClass, Icon("res://texture/icon/handicon.png")]
public partial class PlayerHand : Container
{

	[Export] float NewCardYPosition;

	int SelectionIndex = -1;

	//we can remove this later
	static PackedScene CardScene = ResourceLoader.Load<PackedScene>("uid://lcfq5pnqafa3");

    const float DrawAnimationLength = .4f;
	const float MoveHandAnimationLength = .3f;
	const float TimeBeforeDrawingNextCard = .15f;

	double counter = 0;

	public float Spacing = 8;

	public bool InputEnabled = false;
    bool CurrentlyDrawing = false; //if we are animating cards being added or not. this extends further than just drawing the hand

    public List<PieceCard> Hand = [];
	Stack<PieceCard> queuedCards = [];

	[Signal] public delegate void DrawOperationStartedEventHandler();
	[Signal] public delegate void DrawOperationCompletedEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		Position = new(0, GetNewYOffset());

		//DrawHand();

		InputEnabled = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
		counter += delta;

		TryStartDrawing();
		UpdateHandPosition();

		if (InputEnabled && !CurrentlyDrawing)
		{
			CheckInput();
		}

		if(counter > 1)
		{
			counter = -1000000;
			DrawHand();

		}
    }

    void UpdateHandPosition()
    {
        for (int i = 0; i < Hand.Count; i++)
        {
            var card = Hand[i];

            card.UpdatePosition(Spacing, i);
            card.Name = "card #" + i;
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
			SelectCard(SelectionIndex - 1);
		}
		else if (Input.IsActionJustPressed("Down"))
		{
			SelectCard(SelectionIndex + 1);
		}

		if (Input.IsActionJustPressed("SlamPiece"))
		{

		}
    }

	void SelectCard(int index)
	{
		if(index < Hand.Count && index > -1)
		{
			if(SelectionIndex != -1)
			{
				Hand[SelectionIndex].Deselect();

                if (index == SelectionIndex) { SelectionIndex = -1; return; }
            }

			Hand[index].Select();
			SelectionIndex = index;
		}
	}

	

	void TryStartDrawing()
	{
		if (!CurrentlyDrawing && queuedCards.Count > 0)
		{
            EmitSignalDrawOperationStarted();
            CurrentlyDrawing = true;

            Tween tween = GetTree().CreateTween().SetParallel();

            //setup our bitches
            for (int i = 1; i < queuedCards.Count; i++)
			{
				tween.TweenCallback(Callable.From(StartDrawingNextCard)).SetDelay(TimeBeforeDrawingNextCard * i);
			}

            float finalSpacing = GetNewSpacing();
            float yOffset = GetNewYOffset();

            tween.TweenProperty(this, "Spacing", finalSpacing, MoveHandAnimationLength * queuedCards.Count);
            tween.TweenProperty(this, "position", new Vector2(Position.X, yOffset), MoveHandAnimationLength * queuedCards.Count).SetTrans(Tween.TransitionType.Quad);

            StartDrawingNextCard();
        }
	}

	public void DrawHand()
	{
		for(int i = 0; i < RunStats.HandDrawSize; i++)
		{
			AddCard((PieceCard)CardScene.Instantiate());
        }
	}

    public void AddCard(PieceCard card)
	{
		queuedCards.Push(card);
    }

	void StartDrawingNextCard()
	{
		PieceCard card = queuedCards.Pop();
		AddChild(card);

		card.Position = new Vector2(0, 1000);

		card.Visible = true;
		Hand.Add(card);

		card.YOffset = 150 - (Hand.Count * 34 + Spacing * Hand.Count); //TODO: sync these positions up with real objects
        card.Scale = new(.8f, .8f);

        Tween tween = GetTree().CreateTween().SetParallel(); 

		tween.TweenProperty(card, "YOffset", 0, DrawAnimationLength).SetTrans(Tween.TransitionType.Quad).Finished += DrawTweenFinished;
		tween.TweenProperty(card, "scale", new Vector2(1, 1), MoveHandAnimationLength);
    }

	void DrawTweenFinished()
	{
		if (queuedCards.Count == 0 && CurrentlyDrawing) 
		{
			CurrentlyDrawing = false;
			EmitSignalDrawOperationCompleted();
		}
	}

	float GetNewSpacing()
	{
		float spacing = Math.Max(8 - (Hand.Count + queuedCards.Count - 3) * 2.5f, 0);
		return spacing;
	}
	float GetNewYOffset()
	{
		int totalCards = Hand.Count + queuedCards.Count;

		float yoffset =  totalCards * 17 ;
        return -yoffset;
	}
}

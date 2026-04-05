using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

[GlobalClass, Icon("res://texture/icon/handicon.png")]
public partial class PlayerHand : Container
{

	bool InputEnabled = true;
	int SelectionIndex = 0;

	const float DrawAnimationLength = .4f;
	const float MoveHandAnimationLength = .3f;
	const float TimeBeforeDrawingNextCard = .2f;

	double counter = 0;

	public float Spacing = 8;

    bool currentlyDrawing = false; //if we are animating cards being added or not. this extends further than just drawing the hand

    public PieceCard SelectedCard;
    public List<PieceCard> Hand = [];
	Stack<PieceCard> queuedCards = [];

	[Signal] public delegate void DrawOperationStartedEventHandler();
	[Signal] public delegate void DrawOperationCompletedEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		Position = new(0, GetNewYOffset(8));

        PieceCard card1 = (PieceCard)ResourceLoader.Load<PackedScene>("res://board/piece_card.tscn").Instantiate();
        AddCard(card1);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
		counter += delta;
		TryStartDrawing();
		UpdateHandPosition();

		if (InputEnabled)
		{
			CheckInput();
		}
    }

	void CheckInput()
	{
		if (Input.IsActionJustPressed("One"))
		{

		}
	}

	void SelectCard(int index)
	{
		if(index >= Hand.Count)
		{
			//do stuff
		}
	}

	void TryStartDrawing()
	{
		if (!currentlyDrawing && queuedCards.Count > 0)
		{
            EmitSignalDrawOperationStarted();
            currentlyDrawing = true;

            Tween tween = GetTree().CreateTween().SetParallel();

            //setup our bitches
            for (int i = 0; i < queuedCards.Count; i++)
			{
				tween.TweenCallback(Callable.From(StartDrawingNextCard)).SetDelay(TimeBeforeDrawingNextCard * i);
			}

            float finalSpacing = GetNewSpacing();
            float yOffset = GetNewYOffset(finalSpacing);

            tween.TweenProperty(this, "Spacing", finalSpacing, MoveHandAnimationLength * queuedCards.Count);
            tween.TweenProperty(this, "position", new Vector2(Position.X, yOffset), MoveHandAnimationLength * queuedCards.Count);
        }
	}

	public void DrawHand()
	{

	}

    public void AddCard(PieceCard card)
	{
		queuedCards.Push(card);
    }

	void StartDrawingNextCard()
	{
		PieceCard card = queuedCards.Pop();
		AddChild(card);

		card.Position = Vector2.Zero;

		card.Visible = true;
		Hand.Add(card);

		card.YOffset = 70;
		card.Scale = new(.8f, .8f);

        Tween tween = GetTree().CreateTween().SetParallel(); 

		tween.TweenProperty(card, "YOffset", 0, DrawAnimationLength).Finished += DrawTweenFinished;
		tween.TweenProperty(card, "scale", new Vector2(1, 1), MoveHandAnimationLength);
    }

	void DrawTweenFinished()
	{
		if (queuedCards.Count == 0) 
		{ 
			currentlyDrawing = false;
			EmitSignalDrawOperationCompleted();
		}
	}

	void UpdateHandPosition()
	{
		for(int i = 0; i < Hand.Count; i++)
		{
			var card = Hand[i];

			card.UpdatePosition(Spacing, i);
			card.Name = "card #" + i;
		}
	}

	float GetNewSpacing()
	{
		float spacing = Math.Max(8 - (Hand.Count + queuedCards.Count - 3) * 2.5f, 0);
		return spacing;
	}
	float GetNewYOffset(float finalSpacing)
	{
		int totalCards = Hand.Count + queuedCards.Count;

		float yoffset =  135 - totalCards * 17 ;
		GD.Print(yoffset);
        return yoffset;
	}

}

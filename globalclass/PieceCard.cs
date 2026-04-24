using Godot;
using System;

[GlobalClass, Icon("res://texture/icon/cardicon.png")]
public partial class PieceCard : Control
{

    public BagPiece LinkedPiece;

    public int index = -1;
    bool IsSelected = false;

	[Export] TextureRect FullCard;
    [Export] TextureRect CardBorder;
	[Export] TextureRect CardNumber;
	[Export] TextureRect HighlightBars;
	[Export] Control BlockBox;
	[Export] ColorRect WhiteFlashBox;
	[Export] AnimationPlayer animation;

	public event Action DiscardAnimationComplete;

    static readonly PackedScene CardScene = ResourceLoader.Load<PackedScene>("uid://lcfq5pnqafa3");

	public float BaseYPosition = 0;
	public float AnimatedYOffset;

    public static PieceCard CreateNewCard(BagPiece piece)
	{
		PieceCard card = (PieceCard)CardScene.Instantiate();
		card.Initialize(piece);
		return card;
	}

	void Initialize(BagPiece piece)
	{
		LinkedPiece = piece;
        CardBorder.SelfModulate = Color.FromHsv(piece.h, piece.s, piece.v);

        //TODO: add piece type icon here!

        foreach (var block in piece.Blocks) //create every preview sprite
        {
            var sprite = block.GetCardPreviewSprite(piece.h, piece.s, piece.v);

			BlockBox.AddChild(sprite);
        }

		//we try to automatically center the piece based on dimensions here
		Vector2 offset = new(piece.Dimensions.X * 3.5f, piece.Dimensions.Y * 3.5f);

        BlockBox.Position -= offset;
    }

	public void Select()
	{
		HighlightBars.Visible = true;
		IsSelected = true;
	}

	public void Deselect()
	{
		HighlightBars.Visible = false;
		IsSelected = false;
	}

	public void SetIndex(int index)
	{
		this.index = index;
		Rect2 textureRect = new(index * 5, 120, 5, 8);

		(CardNumber.Texture as AtlasTexture).Region = textureRect;
	}

	public void Discard()
	{
		FullCard.Visible = false;
        WhiteFlashBox.Visible = true;

        animation.Play("CardFlashDisappear");
	}

	public void OnDiscardAnimationFinished(StringName name)
	{
		DiscardAnimationComplete?.Invoke();
		QueueFree();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = new(Position.X, BaseYPosition + AnimatedYOffset);
	}
}

using Godot;
using System;

[GlobalClass, Icon("res://texture/icon/cardicon.png")]
public partial class PieceCard : TextureRect
{
	[Export] TextureRect CardBorder;
	[Export] TextureRect CardNumber;
	[Export] TextureRect HighlightBars;
	[Export] Control BlockBox;

	int index = -1;

	bool IsSelected = false;
	public float YOffset;

	public PieceCard(BagPiece piece)
	{
		CardBorder.SelfModulate = Color.FromHsv(piece.h, piece.s, piece.v);
		
		//TODO: add piece type icon here!

		foreach(var block in piece.Blocks)
		{
			var sprite = block.GetCardPreviewSprite();
			AddChild(sprite);
			sprite.Reparent(BlockBox);
		}

        //we try to automatically center the piece based on dimensions here
        //this may need adjustments !!! TODO
        float xOffset = 7 - piece.Dimensions.Y * 3.5f;
        float yOffset = 7 - piece.Dimensions.Y * 3.5f;

		Vector2 offset = new(xOffset, yOffset);

		BlockBox.Position -= offset;
	}

	public PieceCard() { }

	public void UpdatePosition(float spacing, int index)
	{
		float y = (index * 34) + (spacing * index) + YOffset; 
		
		Position = new(0, y);
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
		Rect2 textureRect = new(index * 5, 120, 5, 8);

		(CardNumber.Texture as AtlasTexture).Region = textureRect;
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

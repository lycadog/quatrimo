using Godot;
using System;

[GlobalClass]
public partial class Cursedblock : Block
{
	bool open = false;
	bool justOpened = false;
	int openCounter = 0;

	static readonly Rect2 closedRect = new(140, 0, 10, 10);
    static readonly Rect2 closedRectLayer2 = new(140, 10, 10, 10);

    static readonly Rect2 openRect = new(150, 00, 10, 10);
    static readonly Rect2 openRectLayer2 = new(150, 10, 10, 10);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    protected override void CustomScore()
    {
		openCounter = 4;
		if (!open)
		{
            OpenEye();
        }
    }
	void OpenEye()
	{
		open = true;
		justOpened = true; //we can't increase the score here since this runs before! our score value is obtained
		//so we must increase it the first tick after, using this bool!
		//then when we close again we remove the score and force close

		(SpriteLayer1 as Sprite2D).RegionRect = openRect;
		(SpriteLayer2 as Sprite2D).RegionRect = openRectLayer2;
	}

	void CloseEye()
	{
		open = false;
		ScoreValue -= 3;

        (SpriteLayer1 as Sprite2D).RegionRect = closedRect;
        (SpriteLayer2 as Sprite2D).RegionRect = closedRectLayer2;

    }

    protected override void TickBlock()
    {
		if (open)
		{
			openCounter--;
            if (justOpened)
            {
                ScoreValue += 3;
				justOpened = false;
				return;
            }

            if (openCounter <= 0)
			{
				CloseEye();
			}
		}
    }
}

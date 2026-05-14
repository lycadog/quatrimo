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

    protected override void RunScoreBehavior()
    {
		openCounter = 4;
		if (!open)
		{
            OpenEye();
			return;
        }

		if (justOpened)
		{
			ScoreValue += 3;
			justOpened = false;
		}
    }

	void OpenEye()
	{
		open = true;
		justOpened = true; //we can't increase the score here since this runs before! our score value is obtained
						   //so we must increase it the first tick after, using this bool!
						   //then when we close again we remove the score and force close

		BlockSprite.SetTexture(openRect);
		BlockSprite.SetSecondLayerTexture(openRectLayer2);
	}

	void CloseEye()
	{
		open = false;
		ScoreValue -= 3;

        BlockSprite.SetTexture(closedRect);
        BlockSprite.SetSecondLayerTexture(closedRectLayer2);

    }

    protected override void RunTickBehavior()
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

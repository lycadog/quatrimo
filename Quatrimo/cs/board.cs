using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quatrimo;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class board
{
    public block[,] blocks;
    public List<drawable> sprites = [];
    public List<drawable> staleSprites = [];
	public Vector2I dimensions;
	public static readonly Vector2I eDimensions = new Vector2I(45, 25); //element dimensions
	public static Vector2I offset;
    public pieceBox nextbox;
    public pieceBox holdbox;
    public spriteObject pauseText = new spriteObject();

    public static readonly int boardy = 22;

    public board(Vector2I dim)
    {
        dimensions = dim;
		blocks = new block[dimensions.x, dimensions.y];
        offset = new Vector2I((eDimensions.x - dimensions.x) / 2 - 1, 3);
        createBoardElements();
    }

    /// <summary>
    /// Draws the entire board scene
    /// </summary>
    /// <param name="spriteBatch"></param>
    /// <param name="gameTime"></param>
    public void draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        foreach(var sprite in sprites)
        {
            sprite.draw(spriteBatch, gameTime, this);
        }

        foreach(var staleSprite in staleSprites)
        {
            sprites.Remove(staleSprite);
        }
        staleSprites.Clear();
    }


    /// <summary>
    /// Creates sprites needed for the board border and background
    /// </summary>
	public void createBoardElements()
	{
        nextbox = new pieceBox(new Vector2I(90, 190), Game1.nextBox);
        holdbox = new pieceBox(new Vector2I(90, 130), Game1.holdBox);
        pauseText.tex = Game1.pausedtext;
        pauseText.depth = 1;
        pauseText.size = new Vector2I(120, 40);
        pauseText.pos = new Vector2I(155, 100);
        sprites.Add(nextbox);
        sprites.Add(holdbox);

        sprites.Add(new element(Game1.borderDL, Color.White, new Vector2I(1, 0), 0.9f)); //top bar
        sprites.Add(new element(Game1.borderDR, Color.White, new Vector2I(43, 0), 0.9f));

		for(int i = 2; i < eDimensions.x-2; i++) //create top border
		{
			if(i == 17) //create title
			{
                sprites.Add(new element(Game1.nameQ, new Color(new Vector3(1f, 0.067f, 0.933f)), new Vector2I(18, 0), 0.9f));
                sprites.Add(new element(Game1.nameU, new Color(new Vector3(1f, 0.067f, 0.933f)), new Vector2I(19, 0), 0.9f));
                sprites.Add(new element(Game1.nameA, new Color(new Vector3(1f, 0.067f, 0.933f)), new Vector2I(20, 0), 0.9f));
                sprites.Add(new element(Game1.nameT, new Color(new Vector3(0.133f, 1f, 0.8f)), new Vector2I(21, 0), 0.9f));
                sprites.Add(new element(Game1.nameR, new Color(new Vector3(0.133f, 1f, 0.8f)), new Vector2I(22, 0), 0.9f));
                sprites.Add(new element(Game1.nameI, new Color(new Vector3(0.133f, 1f, 0.8f)), new Vector2I(23, 0), 0.9f));
                sprites.Add(new element(Game1.nameM, new Color(new Vector3(0.667f, 0f, 1f)), new Vector2I(24, 0), 0.9f));
                sprites.Add(new element(Game1.nameO, new Color(new Vector3(0.667f, 0f, 1f)), new Vector2I(25, 0), 0.9f));
            }
            else if (i > 17 && i < 26) { continue; }
            sprites.Add(new element(Game1.borderD, Color.White, new Vector2I(i,0), 0.9f));
		}

        //create board border

        sprites.Add(new element(Game1.borderUL, Color.White, new Vector2I(offset.x, 2), 0.9f)); // create corner pieces
        sprites.Add(new element(Game1.borderDL, Color.White, new Vector2I(offset.x, boardy + 1), 0.9f));
        sprites.Add(new element(Game1.borderUR, Color.White, new Vector2I(offset.x + dimensions.x+1, 2), 0.9f));
        sprites.Add(new element(Game1.borderDR, Color.White, new Vector2I(offset.x + dimensions.x+1, boardy + 1), 0.9f));


		for (int x = 0; x < dimensions.x; x++) //create top/bottom border
		{
            sprites.Add(new element(Game1.borderU, Color.White, new Vector2I(offset.x + x + 1, 2), 0.9f));
            sprites.Add(new element(Game1.borderD, Color.White, new Vector2I(offset.x + x + 1, boardy + 1), 0.9f));
        }

		for(int y = 0; y < boardy - 2; y++) //create left/right border
		{
            sprites.Add(new element(Game1.borderL, Color.White, new Vector2I(offset.x, y + 3), 0.9f));
            sprites.Add(new element(Game1.borderR, Color.White, new Vector2I(offset.x + dimensions.x+1, y + 3), 0.9f));
        }

        var sprite = new spriteObject(); //create black box behind board
        sprite.size = element.elementPos2WorldPos(new Vector2I(14, 22));
        sprite.tex = Game1.solid;
        sprite.color = Color.Black;
        sprite.pos = element.elementPos2WorldPos(new Vector2I(offset.x, 2));
        sprite.depth = 0.05f;
        sprites.Add(sprite);

		//rewrite these two for blocks to be more efficient ie. place "full" during board border gen and in the loop below
        int counter = 0;
        Color[] colors = [new Color(new Vector3(0.063f, 0.055f, 0.067f)), new Color(new Vector3(0.09f, 0.086f, 0.094f))];
        for (int x = 0; x < dimensions.x; x++) //generate board square background
		{
			for(int y = 0; y < boardy -2; y++)
			{
                sprites.Add(new element(Game1.box, colors[counter % 2], new Vector2I(offset.x + x + 1, y + 3), 0.1f));
                counter++;
            }
            counter++;
        }

    }

    public void lowerRows(List<int> scoredRows)
    {
        int length = scoredRows.Count;
        int[] rows = new int[length];
        for (int l = 0; l < length; l++)//sort scoredRows by ascending
        {
            rows[l] = scoredRows.Min();
            scoredRows.Remove(rows[l]);
        }

        for(int i = 0; i < length; i++) 
        {
            for(int y = rows[i]-1; y >= 0; y--)
            {
                for(int x = 0; x < dimensions.x; x++)//iterate through rows above the scored row, bringing each block down
                {
                    block block = blocks[x, y];
                    if(block != null)
                    {
                        block.movePlaced(new Vector2I(0,1), block);
                    }
                }
            }
        }
    }

}

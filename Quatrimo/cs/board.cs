using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quatrimo;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class board
{
    public block[,] blocks;
    public List<spriteObject> sprites = new List<spriteObject> { };
	public elementold[,,] elementsold;
	public Vector2I dimensions;
	public static readonly Vector2I eDimensions = new Vector2I(45, 25); //element dimensions
	public static Vector2I offset;

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
            sprite.draw(spriteBatch, gameTime);
        }
    }


    /// <summary>
    /// Creates sprites needed for the board border and background
    /// </summary>
	public void createBoardElements()
	{
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

        var sprite = new spriteObject(element.elementPos2WorldPos(new Vector2I(14, 22))); //create black box behind board
        sprite.tex = Game1.full;
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

    public Vector2I convertToElementPos(int x, int y)
    {
        return new Vector2I(x + offset.x + 1, y - 6 + 1);
    }

    public Vector2I convertToElementPos(Vector2I pos)
    {
        return new Vector2I(pos.x + offset.x + 1, pos.y-6 + 1);
    }

    public void lowerRows(List<int> scoredRows)
    {
        int length = scoredRows.Count;
        List<tile> movedTiles = new List<tile>();
        int[] rows = new int[length];

        for (int l = 0; l < length; l++) //sort scoredRows by descending
        {
            rows[l] = scoredRows.Min();
            scoredRows.Remove(scoredRows.Min());
        }

        for (int i = 0; i < length; i++)
        {
            for (int y = rows[i]-1; y > 0; y--)
            {
                for(int x = 0; x < dimensions.x; x++)
                {
                    //Debug.WriteLine("===== LOWERING POS: " + x + ", " + y);
                    block block = blocks[x, y];
                    if(block != null)
                    {
                        block.movePlaced(0, 1);

                    }
                }
            }
        }

    }

    /*public bool isPositionValid(Vector2I pos, bool shouldCollide) //checks if a tile is occupied or otherwise outside of the board
	{ //shouldCollide refers to colliding with other tiles

        //if the tile is outside the board dimensions return false (invalid)
        if (pos.x < 0) { return false; }
        if (pos.x >= dimensions.x) { return false; }
        if (pos.y < 0) { return false; }
        if (pos.y >= dimensions.y) { return false; }
        Debug.WriteLine($"{pos.x}, {pos.y} IS VALID: {tiles[pos.x,pos.y] == null} =============");
		

        //if (tiles[pos.X, pos.Y] != null) { return false; }
		return tiles[pos.x, pos.y] == null;
	}*/

}

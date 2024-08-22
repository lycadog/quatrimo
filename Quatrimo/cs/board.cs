using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quatrimo;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

public class board
{   //fix tiles placing in the air
	
	public tile[,] tiles;
	public element[,,] elements; 
	public Vector2I boardDim;
	public readonly Vector2I eDimensions = new Vector2I(44, 25); //element dimensions
	public Vector2I boardOffset;
	

    public board(Vector2I dim)
    {
        boardDim = dim;
		tiles = new tile[boardDim.x, boardDim.y];
        elements = new element[eDimensions.x, eDimensions.y, 5];

    }


    public void initializeElements()
	{
		
		for(int x = 0; x < eDimensions.x; x++){ 
			for(int y = 0; y < eDimensions.y; y++){ //create every element
				for (int z = 0; z < 5; z++){
                    elements[x, y, z] = new element(new Vector2I(x, y), z);
				}}}
		boardOffset = new Vector2I((eDimensions.x - boardDim.x) / 2 - 1, 3);

        createBoardElements();
    }


	public void createBoardElements()
	{
		elements[1, 0, 4].tex = Game1.borderDL; elements[42, 0, 4].tex = Game1.borderDR; //top bar
		for(int i = 2; i < eDimensions.x-2; i++) //create top border
		{
			if(i == 17) //create title
			{
				elements[18, 0, 4].tex = Game1.nameQ; elements[18, 0, 4].color = new Color(new Vector3(1f, 0.067f, 0.933f));

                elements[19, 0, 4].tex = Game1.nameU; elements[19, 0, 4].color = new Color(new Vector3(1f, 0.067f, 0.933f));

                elements[20, 0, 4].tex = Game1.nameA; elements[20, 0, 4].color = new Color(new Vector3(1f, 0.067f, 0.933f));

                elements[21, 0, 4].tex = Game1.nameT; elements[21, 0, 4].color = new Color(new Vector3(0.133f, 1f, 0.8f));

                elements[22, 0, 4].tex = Game1.nameR; elements[22, 0, 4].color = new Color(new Vector3(0.133f, 1f, 0.8f));

                elements[23, 0, 4].tex = Game1.nameI; elements[23, 0, 4].color = new Color(new Vector3(0.133f, 1f, 0.8f));

                elements[24, 0, 4].tex = Game1.nameM; elements[24, 0, 4].color = new Color(new Vector3(0.667f, 0f, 1f));

                elements[25, 0, 4].tex = Game1.nameO; elements[25, 0, 4].color = new Color(new Vector3(0.667f, 0f, 1f));
            }
            else if (i > 17 && i < 26) { continue; }
			elements[i, 0, 4].tex = Game1.borderD;
		}

		//create board border
		

		elements[boardOffset.x,2, 4].tex = Game1.borderUL; //create 4 corner pieces
        elements[boardOffset.x, boardDim.y+1, 4].tex = Game1.borderDL;
        elements[boardOffset.x + boardDim.x+1, 2, 4].tex = Game1.borderUR;
        elements[boardOffset.x + boardDim.x+1, boardDim.y+1, 4].tex = Game1.borderDR;

		for (int x = 0; x < boardDim.x; x++) //create top/bottom border
		{
			elements[boardOffset.x + x + 1, 2, 4].tex = Game1.borderU;
			elements[boardOffset.x + x + 1, boardDim.y+1, 4].tex = Game1.borderD;
        }

		for(int y = 0; y < boardDim.y-2; y++) //create left/right border
		{
			elements[boardOffset.x, y + 3, 4].tex = Game1.borderL;
            elements[boardOffset.x + boardDim.x+1, y + 3, 4].tex = Game1.borderR;
        }



		for (int x = 0; x < boardDim.x + 2; x++){ //generate black board background
			for(int y = 0; y < boardDim.y; y++)
			{
				element element = elements[boardOffset.x + x, y+2, 0];
                element.tex = Game1.full;
				element.color = new Color(new Vector3(0.035f, 0.031f, 0.032f));
            }   
        }
		//rewrite these two for blocks to be more efficient ie. place "full" during board border gen and in the loop below
        int counter = 0;
        Vector3[] colors = new Vector3[] { new Vector3(0.063f, 0.055f, 0.067f), new Vector3(0.09f, 0.086f, 0.094f) };
        for (int x = 0; x < boardDim.x; x++) //generate board square background
		{
			for(int y = 0; y < boardDim.y-2; y++)
			{
                element element = elements[boardOffset.x + x + 1, y + 3, 1];
                element.tex = Game1.box;
                element.color = new Color(colors[counter % 2]);
                counter++;
            }
            counter++;
        }

    }



    public void drawElements(SpriteBatch spriteBatch, GameTime gameTime)
	{
		for(int z = 0; z < 5; z++){
			for(int x = 0; x < eDimensions.x; x++){
				for (int y = 0; y < eDimensions.y; y++) {
					elements[x,y,z].draw(spriteBatch, gameTime);
				}}}
	}

    /*
    public void initializeTiles()
	{
		tiles = new tile[dimensions.X,dimensions.Y];
		asciiBackground = new RichTextLabel[dimensions.X,dimensions.Y];
        asciiForeground = new RichTextLabel[dimensions.X, dimensions.Y];
        asciiAnimations = new RichTextLabel[dimensions.X, dimensions.Y];

		initializeAsciiNodes();


		//load bullshit
		//level.loadStarterTiles whatever
    }

	public void updateGraphics() //RUNS ON TICK
	{
        foreach (RichTextLabel text in tickStaleTiles) //remove old tiles
        {
            text.Text = " ";
        }
        tickStaleTiles.Clear();


        foreach (renderable render in tickRenderQueue) //render new tiles
        {
            renderTile(render);
        }
        tickRenderQueue.Clear();
    }
    public void markTickStale(Vector2I pos, int z) //mark a position to clear graphics
    {
        tickStaleTiles.Add(getAsciiNode(pos, z));
    }

    public void updateBoard() //RUNS ON BOARD UPDATE
    { //unrender old tiles and render new ones
        foreach (RichTextLabel text in boardStaleTiles) //remove old tiles
        {
			text.Text = " ";
		}
		boardStaleTiles.Clear();


		foreach(renderable render in boardRenderQueue) //render new tiles
		{
			renderTile(render);
		}
		boardRenderQueue.Clear();
	}
    public void markBoardStale(Vector2I pos, int z) //mark a position to clear graphics
    {
        boardStaleTiles.Add(getAsciiNode(pos, z));
    }

    public void animationTick(double delta) //run from main
    {
        foreach (animatable animate in animatables)
        {
            animate.tick(delta);
        }
		foreach (animatable stale in staleAnimatables)
		{
			animatables.Remove(stale);
		}
    }

    public void renderTile(renderable render) //renders a renderable object to the ascii board
	{
		RichTextLabel node = getAsciiNode(render.pos, render.z);
        node.Text = render.text;
		if (render.temporary)
		{
			boardStaleTiles.Add(node);
		}
	}

	

	public RichTextLabel getAsciiNode(Vector2I pos, int z)
	{
        RichTextLabel node;
        switch (z)
        {
            case 0:
                node = asciiBackground[pos.X, pos.Y];
                break;
            case 1:
                node = asciiForeground[pos.X, pos.Y];
                break;
            case 2:
                node = asciiAnimations[pos.X, pos.Y];
                break;
            default:
				return null;
        }
		return node;
    }*/

    public void lowerRows(List<int> scoredRows) //lowers rows above the scored rows after scoring
	{
		int length = scoredRows.Count; 
		List<tile> movedTiles = new List<tile>();
		int[] rows = new int[length];

		for(int l = 0; l < length; l++) //sort scoredRows by descending
		{
			rows[l] = scoredRows.Max();
			scoredRows.Remove(scoredRows.Max());
		}

        Debug.WriteLine(length);
        for (int i = 0; i < length; i++)
		{
		
			for (int y = rows[i] + 1; y < boardDim.y; y++)
			{
				for (int x = 0; x < boardDim.x; x++)
				{
					tile tile = tiles[x, y];
					if (tile != null)
					{
                        //GD.Print($"MOVING PIECE from {tile.boardPos} to [{tile.boardPos.X}, {tile.boardPos.Y - 1}]");


						//boardStaleTiles.Add(asciiForeground[x, y]);
                        tiles[x, y - 1] = tile;
                        tile.boardPos = new Vector2I(tile.boardPos.x, tile.boardPos.y - 1);
                        tiles[x, y] = null;
						
							
						movedTiles.Add(tile);
					}
				}
			}
		}
		foreach(tile tile in movedTiles)
		{

		}
	}

	/*
	public void updatePiecePreview(boardPiece currentPiece, boardPiece nextPiece)
	{
		currentPreview.Text = $"NOW PLAYING:\n{currentPiece.name}";
		nextPreview.Text = $"NEXT UP:\n{nextPiece.name}";
	}
	public void updateScore(long value)
	{
		score.Text = $"SCORE:\n{value}";
	}

	public void updateLevelUI(int level, double times)
	{
		levelUI.Text = level.ToString();
		levelTimesUI.Text = times.ToString();
	}

	public void updateHeldUI(boardPiece piece)
	{
		if(piece != null)
		{
            heldPiece.Text = $"HOLD:\n{piece.name}";
        }
    }
	public void initializeAsciiNodes() //create and set all ascii nodes properly
	{
		
		for(int x = 0; x < dimensions.X; x++)
		{
			for(int y = 0; y < dimensions.Y; y++)
			{
                RichTextLabel node0 = createAsciiNode(x, y, 0);
                RichTextLabel node1 = createAsciiNode(x, y, 1);
                RichTextLabel node2 = createAsciiNode(x, y, 2);

                asciiBackground[x, y] = node0;
                asciiForeground[x, y] = node1;
                asciiAnimations[x, y] = node2;
            }
		}
	}
	
	public RichTextLabel createAsciiNode(int x, int y, int z)
	{
        RichTextLabel node = new RichTextLabel();
        AddChild(node);
        node.Reparent(asciiControl);
        node.Position = new Vector2((x + 1) * 18, 616 - (y * 28));
        node.Size = new Vector2(18, 28);
        node.Text = " ";
        node.BbcodeEnabled = true;
		node.ZIndex = z;
		return node;
    }

	public void clearGraphics()
	{
		foreach(RichTextLabel text in asciiBackground)
		{
			text.Clear();
		}
        foreach (RichTextLabel text in asciiForeground)
        {
            text.Clear();
        }
        foreach (RichTextLabel text in asciiAnimations)
        {
            text.Clear();
        }
    }

	public void resetUI()
	{
		score.Text = "SCORE:";
		currentPreview.Text = "NOW PLAYING:";
		nextPreview.Text = "NEXT UP:";
		heldPiece.Text = "HOLD:";
	}*/

	public bool isPositionValid(Vector2I pos, bool shouldCollide) //checks if a tile is occupied or otherwise outside of the board
	{ //shouldCollide refers to colliding with other tiles

        //if the tile is outside the board dimensions return false (invalid)
        if (pos.x < 0) { return false; }
        if (pos.x >= boardDim.x) { return false; }
        if (pos.y < 0) { return false; }
        if (pos.y >= boardDim.y) { return false; }
        Debug.WriteLine($"{pos.x}, {pos.y} IS VALID: {tiles[pos.x,pos.y] == null} =============");
		

        //if (tiles[pos.X, pos.Y] != null) { return false; }
		return tiles[pos.x, pos.y] == null;
	}

}

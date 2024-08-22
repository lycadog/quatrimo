using Microsoft.Xna.Framework;
using Quatrimo;
using System;

public static class data
{ //URGENT: ADD NEW PIECES AND BAGS
  //hook, pick, boson, etc


    static tileSet common = new(new tileType[] { new basicTile() }, new float[] { 100 }); //this contains all possible tiletypes for the Common tileset; rewrite this eventually to have weights as well

    //default colors, broad color palette
    static readonly colorSet deflt = new colorSet(new Color[] { new Color(new Vector3(1, 0.16f, 0.16f)), new Color(new Vector3(1, .396f, .396f)), new Color(new Vector3(1, .325f, .102f)), new Color(new Vector3(.96f, .561f, .427f)), new Color(new Vector3(1, .561f, .102f)), new Color(new Vector3(.969f, .702f, .427f)), new Color(new Vector3(1, .729f, .102f)), new Color(new Vector3(.969f, .804f, .427f)), new Color(new Vector3(.961f, 1, 10.2f)), new Color(new Vector3(.945f, .969f, .427f)), new Color(new Vector3(.71f, 1, .102f)), new Color(new Vector3(.79f, .969f, .427f)), new Color(new Vector3(.243f, .957f, .169f)), new Color(new Vector3(.463f, .992f, .404f)), new Color(new Vector3(.106f, 1, .376f)), new Color(new Vector3(.404f, .992f, .58f)), new Color(new Vector3(.059f, 1, .714f)), new Color(new Vector3(.4f, .969f, .796f)), new Color(new Vector3(.059f, .965f, 1)), new Color(new Vector3(.4f, .945f, .969f)), new Color(new Vector3(.059f, .765f, 1)), new Color(new Vector3(.4f, .824f, .969f)), new Color(new Vector3(.059f, .518f, 1)), new Color(new Vector3(.4f, .675f, .969f)), new Color(new Vector3(.059f, .341f, 1)), new Color(new Vector3(.4f, .569f, .969f)), new Color(new Vector3(.102f, .059f, 1)), new Color(new Vector3(.427f, .4f, .969f)), new Color(new Vector3(36.5f, .059f, 1)), new Color(new Vector3(.584f, .4f, .969f)), new Color(new Vector3(.573f, .059f, 1)), new Color(new Vector3(.71f, .4f, .969f)), new Color(new Vector3(.647f, .059f, 1)), new Color(new Vector3(.753f, .4f, .969f)), new Color(new Vector3(.773f, .059f, 1)), new Color(new Vector3(.831f, .4f, .969f)), new Color(new Vector3(.945f, .059f, 1)), new Color(new Vector3(.933f, .4f, .969f)), new Color(new Vector3(1, .059f, .855f)), new Color(new Vector3(.969f, .4f, .878f)), new Color(new Vector3(1, .059f, .608f)), new Color(new Vector3(.969f, .4f, .729f)), new Color(new Vector3(1, .059f, .431f)), new Color(new Vector3(.969f, .4f, .624f)), new Color(new Vector3(1, .059f, .31f)), new Color(new Vector3(.969f, .4f, .553f)) });
    public static pieceType bLong;
    public static pieceType bSquare;
    public static pieceType bTBlock;
    public static pieceType bLBlockL;
    public static pieceType bLBlockR;
    public static pieceType bZBlockL;
    public static pieceType bZBlockR;

    public static pieceType bWedge;
    public static pieceType bStick;
    public static pieceType bTwig;

    public static pieceType bBowl;
    public static pieceType bCorner;
    public static pieceType bRectangle;
    public static pieceType bCrowbarL;
    public static pieceType bCrowbarR;
    public static pieceType bLongT;
    public static pieceType bCaret;
    public static pieceType bNub;
    public static pieceType bDipole;
    public static pieceType bSlash;
    public static pieceType bStump;
    public static pieceType bHatchetL;
    public static pieceType bHatchetR;
    public static pieceType bHookL;
    public static pieceType bHookR;
    public static pieceType bPickL;
    public static pieceType bPickR;
    public static pieceType bLepton;
    public static pieceType bBoson;
    public static pieceType bDiamond;
    public static pieceType bBrick;

    public static starterBag classicBag;
    public static starterBag freakyBag;
    public static starterBag bag1;
    public static starterBag bag2;
    public static starterBag bag3;
  
    public static void dataInit()
    {
        bLong = new(4, 1, new tileSet[,] { { common }, { common }, { common }, { common } }, new Vector2I(2, 0), 4, "Line", deflt, Game1.round);
    bSquare = new(2, 2, new tileSet[,] { { common, common }, { common, common } }, new Vector2I(1, 1), 4, "Square", deflt, Game1.round);
    bTBlock = new(3, 2, new tileSet[,] { { common, null }, { common, common }, { common, null } }, new Vector2I(1, 0), 4, "T Block", deflt, Game1.round);
    bLBlockR = new(2, 3, new tileSet[,] { { common, common, common }, { common, null, null } }, new Vector2I(0, 1), 4, "Right L Block", deflt, Game1.round);
    bLBlockL = new(2, 3, new tileSet[,] { { common, null, null }, { common, common, common } }, new Vector2I(0, 1), 4, "Left L Block", deflt, Game1.round);
    bZBlockR = new(2, 3, new tileSet[,] { { null, common, common }, { common, common, null } }, new Vector2I(0, 1), 4, "Right Z Block", deflt, Game1.round);
    bZBlockL = new(2, 3, new tileSet[,] { { common, common, null }, { null, common, common } }, new Vector2I(0, 1), 4, "Left Z Block", deflt, Game1.round);

    bWedge = new(2, 2, new tileSet[,] { { common, common }, { common, null } }, new Vector2I(0, 0), 3, "Wedge", deflt, Game1.box);
    bTwig = new(1, 2, new tileSet[,] { { common, common } }, new Vector2I(0, 1), 2, "Twig", deflt, Game1.round);

    bStick = new(3, 1, new tileSet[,] { { common }, { common }, { common } }, new Vector2I(1, 0), 3, "Stick", deflt, Game1.round);
    bBowl = new(3, 2, new tileSet[,] { { common, common }, { common, null }, { common, common } }, new Vector2I(1, 0), 5, "Bowl", deflt, Game1.circle);
    bCorner = new(3, 3, new tileSet[,] { { common, common, common }, { common, null, null }, { common, null, null } }, new Vector2I(0, 0), 5, "Corner", deflt, Game1.box);
    bRectangle = new(3, 2, new tileSet[,] { { common, common }, { common, common }, { common, common } }, new Vector2I(1, 0), 6, "Rectangle", deflt, Game1.box);
    bCrowbarL = new(2, 5, new tileSet[,] { { common, common, common, common, common }, { null, null, null, null, common } }, new Vector2I(0, 3), 6, "Left Crowbar", deflt, Game1.box);
    bCrowbarR = new(2, 5, new tileSet[,] { { null, null, null, null, common }, { common, common, common, common, common } }, new Vector2I(0, 3), 6, "Right Crowbar", deflt, Game1.box);
    bLongT = new(3, 3, new tileSet[,] { { null, null, common }, { common, common, common }, { null, null, common } }, new Vector2I(1, 1), 5, "Long T Block", deflt, Game1.box_full);

    bCaret = new(2, 3, new tileSet[,] { { common, null, common }, { null, common, null } }, new Vector2I(0, 1), 3, "Caret", deflt, Game1.circle);
    bNub = new(1, 1, new tileSet[,] { { common } }, Vector2I.zero, 1, "Nub", deflt, Game1.circle);
    bDipole = new(3, 2, new tileSet[,] { { common, common }, { null, null }, { common, common } }, new Vector2I(1, 1), 4, "Dipole", deflt, Game1.box_full);
    bSlash = new(2, 2, new tileSet[,] { { common, null }, { null, common } }, Vector2I.zero, 2, "Slash", deflt, Game1.circle);
    bStump = new(4, 2, new tileSet[,] { { common, null }, { common, common }, { common, common }, { common, null } }, new Vector2I(1, 1), 6, "Stump", deflt, Game1.circle);
    bHatchetL = new(4, 2, new tileSet[,] { { common, common }, { common, common }, { null, common }, { null, common } }, new Vector2I(2, 0), 6, "Left Hatchet", deflt, Game1.box);
    bHatchetR = new(4, 2, new tileSet[,] { { null, common }, { null, common }, { common, common }, { common, common } }, new Vector2I(1, 0), 6, "Right Hatchet", deflt, Game1.box);

    bHookL = new(2, 3, new tileSet[,] { { null, null, common }, { common, common, null } }, new Vector2I(1, 1), 3, "Left Hook", deflt, Game1.box);
    bHookR = new(2, 3, new tileSet[,] { { common, common, null }, { null, null, common } }, new Vector2I(0, 1), 3, "Right Hook", deflt, Game1.box);
    bPickL = new(4, 2, new tileSet[,] { { common, null }, { common, common }, { common, null }, { common, null } }, new Vector2I(2, 0), 5, "Left Pick", deflt, Game1.box);
    bPickR = new(4, 2, new tileSet[,] { { common, null }, { common, null }, { common, common }, { common, null } }, new Vector2I(2, 0), 5, "Right Pick", deflt, Game1.box);
    bLepton = new(3, 1, new tileSet[,] { { common }, { null }, { common } }, new Vector2I(1, 0), 2, "Lepton", deflt, Game1.circle);
    bBoson = new(3, 3, new tileSet[,] { { common, common, null }, { common, common, null }, { null, null, common } }, new Vector2I(1, 1), 5, "Boson", deflt, Game1.circle_full);

    bDiamond = new(3, 3, new tileSet[,] { { null, common, null }, { common, common, common }, { null, common, null } }, new Vector2I(1, 1), 5, "Diamond", deflt, Game1.circle);
    bBrick = new(4, 3, new tileSet[,] { { common, common, common }, { common, common, common }, { common, common, common }, { common, common, common } }, new Vector2I(2, 2), 12, "Brick", deflt, Game1.heavy_full);

    classicBag = new starterBag(new pieceType[] { bLong, bSquare, bTBlock, bLBlockR, bLBlockL, bZBlockR, bZBlockL }, "Classic Bag");
    freakyBag = new starterBag(new pieceType[] { bCaret, bNub, bDipole, bSlash, bStump, bHatchetL, bHatchetR, bWedge, bTwig, bTwig, bLong }, "Freaky Bag");

    bag1 = new starterBag(new pieceType[] { bCorner, bSquare, bWedge, bStick, bRectangle, bPickL, bPickR, bLong }, "bag1");
    bag2 = new starterBag(new pieceType[] { bDipole, bLongT, bHatchetL, bHatchetR, bTwig, bWedge, bHookL, bHookR, bLong }, "bag2");
    bag3 = new starterBag(new pieceType[] { bNub, bCaret, bBowl, bStump, bSlash, bStick, bLong, bLepton, bBoson }, "bag3");

}


}
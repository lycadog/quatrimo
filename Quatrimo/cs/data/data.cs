using Microsoft.Xna.Framework;
using Quatrimo;
using System;

public static class data
{ //ADJUST boson origin for better rotation (probably should be 1,1)


    static readonly wSet<blockType> common = new([new basicBlock(null)], [100]); //this contains all possible tiletypes for the Common tileset; rewrite this eventually to have weights as well
    
    static readonly wSet<pieceMod> basic = new([new basicMod()], [100]);
    static readonly wSet<pieceMod> spectral = new([new spectralMod()], [100]);


    //default colors, broad color palette
    static readonly colorSet deflt = new colorSet([new Color(new Vector3(1, 0.16f, 0.16f)), new Color(new Vector3(1, .396f, .396f)), new Color(new Vector3(1, .325f, .102f)), new Color(new Vector3(.96f, .561f, .427f)), new Color(new Vector3(1, .561f, .102f)), new Color(new Vector3(.969f, .702f, .427f)), new Color(new Vector3(1, .729f, .102f)), new Color(new Vector3(.969f, .804f, .427f)), new Color(new Vector3(.961f, 1, 10.2f)), new Color(new Vector3(.945f, .969f, .427f)), new Color(new Vector3(.71f, 1, .102f)), new Color(new Vector3(.79f, .969f, .427f)), new Color(new Vector3(.243f, .957f, .169f)), new Color(new Vector3(.463f, .992f, .404f)), new Color(new Vector3(.106f, 1, .376f)), new Color(new Vector3(.404f, .992f, .58f)), new Color(new Vector3(.059f, 1, .714f)), new Color(new Vector3(.4f, .969f, .796f)), new Color(new Vector3(.059f, .965f, 1)), new Color(new Vector3(.4f, .945f, .969f)), new Color(new Vector3(.059f, .765f, 1)), new Color(new Vector3(.4f, .824f, .969f)), new Color(new Vector3(.059f, .518f, 1)), new Color(new Vector3(.4f, .675f, .969f)), new Color(new Vector3(.059f, .341f, 1)), new Color(new Vector3(.4f, .569f, .969f)), new Color(new Vector3(.102f, .059f, 1)), new Color(new Vector3(.427f, .4f, .969f)), new Color(new Vector3(36.5f, .059f, 1)), new Color(new Vector3(.584f, .4f, .969f)), new Color(new Vector3(.573f, .059f, 1)), new Color(new Vector3(.71f, .4f, .969f)), new Color(new Vector3(.647f, .059f, 1)), new Color(new Vector3(.753f, .4f, .969f)), new Color(new Vector3(.773f, .059f, 1)), new Color(new Vector3(.831f, .4f, .969f)), new Color(new Vector3(.945f, .059f, 1)), new Color(new Vector3(.933f, .4f, .969f)), new Color(new Vector3(1, .059f, .855f)), new Color(new Vector3(.969f, .4f, .878f)), new Color(new Vector3(1, .059f, .608f)), new Color(new Vector3(.969f, .4f, .729f)), new Color(new Vector3(1, .059f, .431f)), new Color(new Vector3(.969f, .4f, .624f)), new Color(new Vector3(1, .059f, .31f)), new Color(new Vector3(.969f, .4f, .553f))]);
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
    public static pieceType bBarycenter;
    public static pieceType bBasin;
    public static pieceType bHammer;
    public static pieceType bBar;
    public static pieceType bLonger;
    public static pieceType bAngle;

    public static pieceType bSpectralZ;

    public static starterBag classicBag;
    public static starterBag bag1;
    public static starterBag bag2;
    public static starterBag bag3;
    public static starterBag foundryBag;

    public static starterBag debugbag;

    public static keybind[] keys;

    public static keybind leftKey;
    public static keybind rightKey;
    public static keybind upKey;
    public static keybind downKey;
    public static keybind slamKey;
    public static keybind leftRotateKey;
    public static keybind rightRotateKey;
    public static keybind holdKey;
    public static keybind restartKey;

    public static void dataInit()
    {
        leftKey = new keybind(Microsoft.Xna.Framework.Input.Keys.Left, Microsoft.Xna.Framework.Input.Keys.A);
        rightKey = new keybind(Microsoft.Xna.Framework.Input.Keys.Right, Microsoft.Xna.Framework.Input.Keys.D);
        upKey = new keybind(Microsoft.Xna.Framework.Input.Keys.Up, Microsoft.Xna.Framework.Input.Keys.W);
        downKey = new keybind(Microsoft.Xna.Framework.Input.Keys.Down, Microsoft.Xna.Framework.Input.Keys.S);
        slamKey = new keybind(Microsoft.Xna.Framework.Input.Keys.Space, Microsoft.Xna.Framework.Input.Keys.None);
        leftRotateKey = new keybind(Microsoft.Xna.Framework.Input.Keys.Q, Microsoft.Xna.Framework.Input.Keys.None);
        rightRotateKey = new keybind(Microsoft.Xna.Framework.Input.Keys.E, Microsoft.Xna.Framework.Input.Keys.None);
        holdKey = new keybind(Microsoft.Xna.Framework.Input.Keys.F, Microsoft.Xna.Framework.Input.Keys.None);
        restartKey = new keybind(Microsoft.Xna.Framework.Input.Keys.R, Microsoft.Xna.Framework.Input.Keys.None);

        keys = [leftKey, rightKey, upKey, downKey, slamKey, leftRotateKey, rightRotateKey, holdKey, restartKey];
    }

    public static void dataInitContent()
    {
        bLong = new(4, 1, new wSet<blockType>[,] { { common }, { common }, { common }, { common } }, basic, new Vector2I(2, 0), 4, "Line", deflt, Game1.round);
        bSquare = new(2, 2, new wSet<blockType>[,] { { common, common }, { common, common } }, basic, new Vector2I(1, 1), 4, "Square", deflt, Game1.round);
        bTBlock = new(3, 2, new wSet<blockType>[,] { { common, null }, { common, common }, { common, null } }, basic, new Vector2I(1, 0), 4, "T Block", deflt, Game1.round);
        bLBlockR = new(2, 3, new wSet<blockType>[,] { { common, common, common }, { common, null, null } }, basic, new Vector2I(0, 1), 4, "Right L Block", deflt, Game1.round);
        bLBlockL = new(2, 3, new wSet<blockType>[,] { { common, null, null }, { common, common, common } }, basic, new Vector2I(0, 1), 4, "Left L Block", deflt, Game1.round);
        bZBlockR = new(2, 3, new wSet<blockType>[,] { { null, common, common }, { common, common, null } }, basic, new Vector2I(0, 1), 4, "Right Z Block", deflt, Game1.round);
        bZBlockL = new(2, 3, new wSet<blockType>[,] { { common, common, null }, { null, common, common } }, basic, new Vector2I(0, 1), 4, "Left Z Block", deflt, Game1.round);

        bSpectralZ = new(2, 3, new wSet<blockType>[,] { { common, common, null }, { null, common, common } }, spectral, new Vector2I(0, 1), 4, "Left Z Block", deflt, Game1.round);

        bWedge = new(2, 2, new wSet<blockType>[,] { { common, common }, { common, null } }, basic, new Vector2I(0, 0), 3, "Wedge", deflt, Game1.box);
        bTwig = new(1, 2, new wSet<blockType>[,] { { common, common } }, basic, new Vector2I(0, 1), 2, "Twig", deflt, Game1.round);

        bStick = new(3, 1, new wSet<blockType>[,] { { common }, { common }, { common } }, basic, new Vector2I(1, 0), 3, "Stick", deflt, Game1.round);
        bBowl = new(3, 2, new wSet<blockType>[,] { { common, common }, { common, null }, { common, common } }, basic, new Vector2I(1, 0), 5, "Bowl", deflt, Game1.circle);
        bCorner = new(3, 3, new wSet<blockType>[,] { { common, common, common }, { common, null, null }, { common, null, null } }, basic, new Vector2I(0, 0), 5, "Corner", deflt, Game1.box);
        bRectangle = new(3, 2, new wSet<blockType>[,] { { common, common }, { common, common }, { common, common } }, basic, new Vector2I(1, 0), 6, "Rectangle", deflt, Game1.box);
        bCrowbarL = new(2, 5, new wSet<blockType>[,] { { common, common, common, common, common }, { null, null, null, null, common } }, basic, new Vector2I(0, 3), 6, "Left Crowbar", deflt, Game1.box);
        bCrowbarR = new(2, 5, new wSet<blockType>[,] { { null, null, null, null, common }, { common, common, common, common, common } }, basic, new Vector2I(0, 3), 6, "Right Crowbar", deflt, Game1.box);
        bLongT = new(3, 3, new wSet<blockType>[,] { { null, null, common }, { common, common, common }, { null, null, common } }, basic, new Vector2I(1, 1), 5, "Long T Block", deflt, Game1.box_full);

        bCaret = new(2, 3, new wSet<blockType>[,] { { common, null, common }, { null, common, null } }, basic, new Vector2I(0, 1), 3, "Caret", deflt, Game1.circle);
        bNub = new(1, 1, new wSet<blockType>[,] { { common } }, basic, Vector2I.zero, 1, "Nub", deflt, Game1.circle);
        bDipole = new(3, 2, new wSet<blockType>[,] { { common, common }, { null, null }, { common, common } }, basic, new Vector2I(1, 1), 4, "Dipole", deflt, Game1.box_full);
        bSlash = new(2, 2, new wSet<blockType>[,] { { common, null }, { null, common } }, basic, Vector2I.zero, 2, "Slash", deflt, Game1.circle);
        bStump = new(4, 2, new wSet<blockType>[,] { { common, null }, { common, common }, { common, common }, { common, null } }, basic, new Vector2I(1, 1), 6, "Stump", deflt, Game1.circle);
        bHatchetL = new(4, 2, new wSet<blockType>[,] { { common, common }, { common, common }, { null, common }, { null, common } }, basic, new Vector2I(2, 0), 6, "Left Hatchet", deflt, Game1.box);
        bHatchetR = new(4, 2, new wSet<blockType>[,] { { null, common }, { null, common }, { common, common }, { common, common } }, basic, new Vector2I(1, 0), 6, "Right Hatchet", deflt, Game1.box);

        bHookL = new(2, 3, new wSet<blockType>[,] { { null, null, common }, { common, common, null } }, basic, new Vector2I(1, 1), 3, "Left Hook", deflt, Game1.box);
        bHookR = new(2, 3, new wSet<blockType>[,] { { common, common, null }, { null, null, common } }, basic, new Vector2I(0, 1), 3, "Right Hook", deflt, Game1.box);
        bPickL = new(4, 2, new wSet<blockType>[,] { { common, null }, { common, common }, { common, null }, { common, null } }, basic, new Vector2I(2, 0), 5, "Left Pick", deflt, Game1.box);
        bPickR = new(4, 2, new wSet<blockType>[,] { { common, null }, { common, null }, { common, common }, { common, null } }, basic, new Vector2I(2, 0), 5, "Right Pick", deflt, Game1.box);
        bLepton = new(3, 1, new wSet<blockType>[,] { { common }, { null }, { common } }, basic, new Vector2I(1, 0), 2, "Lepton", deflt, Game1.circle);
        bBoson = new(3, 3, new wSet<blockType>[,] { { common, common, null }, { common, common, null }, { null, null, common } }, basic, new Vector2I(1, 1), 5, "Boson", deflt, Game1.circle_full);

        bDiamond = new(3, 3, new wSet<blockType>[,] { { null, common, null }, { common, common, common }, { null, common, null } }, basic, new Vector2I(1, 1), 5, "Diamond", deflt, Game1.circle);
        bBrick = new(4, 3, new wSet<blockType>[,] { { common, common, common }, { common, common, common }, { common, common, common }, { common, common, common } }, basic, new Vector2I(2, 2), 12, "Brick", deflt, Game1.heavy_full);

        bBarycenter = new(5, 2, new wSet<blockType>[,] { { common, common }, { common, common }, { null, null }, { common, common }, { common, common } }, basic, new Vector2I(3, 1), 8, "Barycenter", deflt, Game1.heavy);
        bBasin = new(4, 2, new wSet<blockType>[,] { { common, common }, { null, common }, { null, common }, { common, common } }, basic, new Vector2I(2, 2), 6, "Basin", deflt, Game1.box);
        bHammer = new(4, 3, new wSet<blockType>[,] { { null, common, null }, { null, common, null }, { common, common, common }, { common, common, common } }, basic, new Vector2I(1, 1), 8, "Hammer", deflt, Game1.heavy);
        bBar = new(4, 2, new wSet<blockType>[,] { { common, common }, { common, common }, { common, common }, { common, common } }, basic, new Vector2I(2, 1), 8, "Bar", deflt, Game1.block_fuller);
        bLonger = new(5, 1, new wSet<blockType>[,] { { common }, { common }, { common }, { common }, { common } }, basic, new Vector2I(3, 0), 5, "Loong", deflt, Game1.box_solid);
        bAngle = new(3, 3, new wSet<blockType>[,] { { null, common, common }, { common, common, common }, { common, common, common } }, basic, new Vector2I(2, 2), 8, "Angle", deflt, Game1.heavy_full);

        classicBag = new starterBag([bLong, bSquare, bTBlock, bLBlockR, bLBlockL, bZBlockR, bZBlockL], "Classic Bag");

        bag1 = new starterBag([bCorner, bSquare, bWedge, bStick, bRectangle, bPickL, bPickR, bLong], "bag1");
        bag2 = new starterBag([bDipole, bLongT, bHatchetL, bHatchetR, bTwig, bWedge, bHookL, bHookR, bLong], "bag2");
        bag3 = new starterBag([bNub, bCaret, bBowl, bStump, bSlash, bTwig, bLong, bLepton, bBoson, bSpectralZ], "bag3");

        foundryBag = new starterBag([bBarycenter, bHammer, bBar, bLong, bAngle, bCorner, bTwig, bSquare], "Foundry");

        debugbag = new starterBag([bLong], "debugbag");

    }
}
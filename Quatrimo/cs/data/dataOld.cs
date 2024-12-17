using Microsoft.Xna.Framework;

namespace Quatrimo
{
    public static class dataOld
    {
        /*
        static readonly wSet<basePieceOld> noMod = new([new basicBasePiece()], [100]);
        static readonly wSet<basePieceOld> spectral = new([new baseSpectralPiece()], [100]);

        static readonly wSet<baseblockType> basic = new([new baseBasicBlock()], [100]);
        static readonly wSet<baseblockType> bomb = new([new baseBombBlock()], [100]);
        static readonly wSet<baseblockType> cursed = new([new baseCursedBlock()], [100]);
        static readonly wSet<baseblockType> piercing = new([new basePiercingBlock()], [100]);


        //default colors, broad color palette


        public static pieceTypeOld bLong;
        public static pieceTypeOld bSquare;
        public static pieceTypeOld bTBlock;
        public static pieceTypeOld bLBlockL;
        public static pieceTypeOld bLBlockR;
        public static pieceTypeOld bZBlockL;
        public static pieceTypeOld bZBlockR;

        public static pieceTypeOld bWedge;
        public static pieceTypeOld bStick;
        public static pieceTypeOld bTwig;

        public static pieceTypeOld bBowl;
        public static pieceTypeOld bCorner;
        public static pieceTypeOld bRectangle;
        public static pieceTypeOld bCrowbarL;
        public static pieceTypeOld bCrowbarR;
        public static pieceTypeOld bLongT;
        public static pieceTypeOld bCaret;
        public static pieceTypeOld bNub;
        public static pieceTypeOld bDipole;
        public static pieceTypeOld bSlash;
        public static pieceTypeOld bStump;
        public static pieceTypeOld bHatchetL;
        public static pieceTypeOld bHatchetR;
        public static pieceTypeOld bHookL;
        public static pieceTypeOld bHookR;
        public static pieceTypeOld bPickL;
        public static pieceTypeOld bPickR;
        public static pieceTypeOld bLepton;
        public static pieceTypeOld bBoson;
        public static pieceTypeOld bDiamond;
        public static pieceTypeOld bBrick;
        public static pieceTypeOld bBarycenter;
        public static pieceTypeOld bBasin;
        public static pieceTypeOld bHammer;
        public static pieceTypeOld bBar;
        public static pieceTypeOld bLonger;
        public static pieceTypeOld bAngle;
        public static pieceTypeOld bNeedle;

        public static pieceTypeOld bTestPiece;
        public static pieceTypeOld bTestPiece2;
        public static pieceTypeOld bTestPiece3;

        public static starterBag classicBag;
        public static starterBag bag1;
        public static starterBag bag2;
        public static starterBag bag3;
        public static starterBag foundryBag;

        public static starterBag debugbag;

        public static keybind[] boardKeys;
        public static keybind[] debugKeys;

        public static keybind leftKey = new(Microsoft.Xna.Framework.Input.Keys.Left, Microsoft.Xna.Framework.Input.Keys.A);
        public static keybind rightKey;
        public static keybind upKey;
        public static keybind downKey;
        public static keybind slamKey;
        public static keybind leftRotateKey;
        public static keybind rightRotateKey;
        public static keybind holdKey;
        public static keybind restartKey;
        public static keybind pauseKey;
        public static keybind toggleDebugKey;

        public static keybind debugMode1;
        public static keybind debugMode2;
        public static keybind debugMode3;

        public static keybind debugKey1;
        public static keybind debugKey2;
        public static keybind debugKey3;
        public static keybind debugKey4;
        public static keybind debugKey5;

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
            pauseKey = new keybind(Microsoft.Xna.Framework.Input.Keys.Escape, Microsoft.Xna.Framework.Input.Keys.None);
            toggleDebugKey = new keybind(Microsoft.Xna.Framework.Input.Keys.OemTilde, Microsoft.Xna.Framework.Input.Keys.None);
            boardKeys = [leftKey, rightKey, upKey, downKey, slamKey, leftRotateKey, rightRotateKey, holdKey, restartKey, pauseKey, toggleDebugKey];

            debugMode1 = new keybind(Microsoft.Xna.Framework.Input.Keys.OemOpenBrackets, Microsoft.Xna.Framework.Input.Keys.None);
            debugMode2 = new keybind(Microsoft.Xna.Framework.Input.Keys.OemCloseBrackets, Microsoft.Xna.Framework.Input.Keys.None);
            debugMode3 = new keybind(Microsoft.Xna.Framework.Input.Keys.OemBackslash, Microsoft.Xna.Framework.Input.Keys.None);

            debugKey1 = new keybind(Microsoft.Xna.Framework.Input.Keys.F1, Microsoft.Xna.Framework.Input.Keys.None);
            debugKey2 = new keybind(Microsoft.Xna.Framework.Input.Keys.F2, Microsoft.Xna.Framework.Input.Keys.None);
            debugKey3 = new keybind(Microsoft.Xna.Framework.Input.Keys.F3, Microsoft.Xna.Framework.Input.Keys.None);
            debugKey4 = new keybind(Microsoft.Xna.Framework.Input.Keys.F4, Microsoft.Xna.Framework.Input.Keys.None);
            debugKey5 = new keybind(Microsoft.Xna.Framework.Input.Keys.F5, Microsoft.Xna.Framework.Input.Keys.None);

            debugKeys = [debugMode1, debugMode2, debugMode3, debugKey1, debugKey2, debugKey3, debugKey4, debugKey5];

        }


        public static void dataInitContent()
        {
            bLong = new(4, 1, new wSet<baseblockType>[,] { { basic }, { basic }, { basic }, { basic } }, noMod, new Vector2I(2, 0), 4, "Line", deflt, Game1.box);
            bLong = new(4, 1, new wSet<baseblockType>[,] { { basic }, { basic }, { basic }, { basic } }, noMod, new Vector2I(2, 0), 4, "Line", deflt, Game1.round);
            bSquare = new(2, 2, new wSet<baseblockType>[,] { { basic, basic }, { basic, basic } }, noMod, new Vector2I(1, 1), 4, "Square", deflt, Game1.round);
            bTBlock = new(3, 2, new wSet<baseblockType>[,] { { basic, null }, { basic, basic }, { basic, null } }, noMod, new Vector2I(1, 0), 4, "T Block", deflt, Game1.round);
            bLBlockR = new(2, 3, new wSet<baseblockType>[,] { { basic, basic, basic }, { basic, null, null } }, noMod, new Vector2I(0, 1), 4, "Right L Block", deflt, Game1.round);
            bLBlockL = new(2, 3, new wSet<baseblockType>[,] { { basic, null, null }, { basic, basic, basic } }, noMod, new Vector2I(0, 1), 4, "Left L Block", deflt, Game1.round);
            bZBlockR = new(2, 3, new wSet<baseblockType>[,] { { null, basic, basic }, { basic, basic, null } }, noMod, new Vector2I(0, 1), 4, "Right Z Block", deflt, Game1.round);
            bZBlockL = new(2, 3, new wSet<baseblockType>[,] { { basic, basic, null }, { null, basic, basic } }, noMod, new Vector2I(0, 1), 4, "Left Z Block", deflt, Game1.round);

            bTestPiece = new(2, 3, new wSet<baseblockType>[,] { { cursed, cursed, null }, { null, cursed, cursed } }, noMod, new Vector2I(0, 1), 4, "Left Z Block", deflt, Game1.round);
            bTestPiece2 = new(1, 2, new wSet<baseblockType>[,] { { bomb, bomb } }, noMod, Vector2I.zero, 2, "DebugPiece2", deflt, Game1.box);
            bTestPiece3 = new(1, 1, new wSet<baseblockType>[,] { { cursed } }, noMod, Vector2I.zero, 1, "DebugPiece2", deflt, Game1.box);

            bWedge = new(2, 2, new wSet<baseblockType>[,] { { basic, basic }, { basic, null } }, noMod, new Vector2I(0, 0), 3, "Wedge", deflt, Game1.box);
            bTwig = new(1, 2, new wSet<baseblockType>[,] { { basic, basic } }, noMod, new Vector2I(0, 1), 2, "Twig", deflt, Game1.round);

            bStick = new(3, 1, new wSet<baseblockType>[,] { { basic }, { basic }, { basic } }, noMod, new Vector2I(1, 0), 3, "Stick", deflt, Game1.round);
            bBowl = new(3, 2, new wSet<baseblockType>[,] { { basic, basic }, { basic, null }, { basic, basic } }, noMod, new Vector2I(1, 0), 5, "Bowl", deflt, Game1.circle);
            bCorner = new(3, 3, new wSet<baseblockType>[,] { { basic, basic, basic }, { basic, null, null }, { basic, null, null } }, noMod, new Vector2I(0, 0), 5, "Corner", deflt, Game1.box);
            bRectangle = new(3, 2, new wSet<baseblockType>[,] { { basic, basic }, { basic, basic }, { basic, basic } }, noMod, new Vector2I(1, 0), 6, "Rectangle", deflt, Game1.box);
            bCrowbarL = new(2, 5, new wSet<baseblockType>[,] { { basic, basic, basic, basic, basic }, { null, null, null, null, basic } }, noMod, new Vector2I(0, 3), 6, "Left Crowbar", deflt, Game1.box);
            bCrowbarR = new(2, 5, new wSet<baseblockType>[,] { { null, null, null, null, basic }, { basic, basic, basic, basic, basic } }, noMod, new Vector2I(0, 3), 6, "Right Crowbar", deflt, Game1.box);
            bLongT = new(3, 3, new wSet<baseblockType>[,] { { null, null, basic }, { basic, basic, basic }, { null, null, basic } }, noMod, new Vector2I(1, 1), 5, "Long T Block", deflt, Game1.boxdetail);

            bCaret = new(2, 3, new wSet<baseblockType>[,] { { basic, null, basic }, { null, basic, null } }, noMod, new Vector2I(0, 1), 3, "Caret", deflt, Game1.circle);
            bNub = new(1, 1, new wSet<baseblockType>[,] { { basic } }, noMod, Vector2I.zero, 1, "Nub", deflt, Game1.circle);
            bDipole = new(3, 2, new wSet<baseblockType>[,] { { basic, basic }, { null, null }, { basic, basic } }, noMod, new Vector2I(1, 1), 4, "Dipole", deflt, Game1.boxdetail);
            bSlash = new(2, 2, new wSet<baseblockType>[,] { { basic, null }, { null, basic } }, noMod, Vector2I.zero, 2, "Slash", deflt, Game1.circle);
            bStump = new(4, 2, new wSet<baseblockType>[,] { { basic, null }, { basic, basic }, { basic, basic }, { basic, null } }, noMod, new Vector2I(1, 1), 6, "Stump", deflt, Game1.circle);
            bHatchetL = new(4, 2, new wSet<baseblockType>[,] { { basic, basic }, { basic, basic }, { null, basic }, { null, basic } }, noMod, new Vector2I(2, 0), 6, "Left Hatchet", deflt, Game1.box);
            bHatchetR = new(4, 2, new wSet<baseblockType>[,] { { null, basic }, { null, basic }, { basic, basic }, { basic, basic } }, noMod, new Vector2I(1, 0), 6, "Right Hatchet", deflt, Game1.box);

            bHookL = new(2, 3, new wSet<baseblockType>[,] { { null, null, basic }, { basic, basic, null } }, noMod, new Vector2I(1, 1), 3, "Left Hook", deflt, Game1.box);
            bHookR = new(2, 3, new wSet<baseblockType>[,] { { basic, basic, null }, { null, null, basic } }, noMod, new Vector2I(0, 1), 3, "Right Hook", deflt, Game1.box);
            bPickL = new(4, 2, new wSet<baseblockType>[,] { { basic, null }, { basic, basic }, { basic, null }, { basic, null } }, noMod, new Vector2I(2, 0), 5, "Left Pick", deflt, Game1.box);
            bPickR = new(4, 2, new wSet<baseblockType>[,] { { basic, null }, { basic, null }, { basic, basic }, { basic, null } }, noMod, new Vector2I(2, 0), 5, "Right Pick", deflt, Game1.box);
            bLepton = new(3, 1, new wSet<baseblockType>[,] { { basic }, { null }, { basic } }, noMod, new Vector2I(1, 0), 2, "Lepton", deflt, Game1.circle);
            bBoson = new(3, 3, new wSet<baseblockType>[,] { { basic, basic, null }, { basic, basic, null }, { null, null, basic } }, noMod, new Vector2I(1, 1), 5, "Boson", deflt, Game1.circledetail);

            bDiamond = new(3, 3, new wSet<baseblockType>[,] { { null, basic, null }, { basic, basic, basic }, { null, basic, null } }, noMod, new Vector2I(1, 1), 5, "Diamond", deflt, Game1.circle);
            bBrick = new(4, 3, new wSet<baseblockType>[,] { { basic, basic, basic }, { basic, basic, basic }, { basic, basic, basic }, { basic, basic, basic } }, noMod, new Vector2I(2, 2), 12, "Brick", deflt, Game1.box);

            bNeedle = new(3, 4, new wSet<baseblockType>[,] { { null, basic, null, null }, { basic, null, piercing, piercing }, { null, basic, null, null } }, noMod, new Vector2I(1, 1), 5, "Needle", deflt, Game1.box);

            bBarycenter = new(5, 2, new wSet<baseblockType>[,] { { basic, basic }, { basic, basic }, { null, null }, { basic, basic }, { basic, basic } }, noMod, new Vector2I(3, 1), 8, "Barycenter", deflt, Game1.box);
            bBasin = new(4, 2, new wSet<baseblockType>[,] { { basic, basic }, { null, basic }, { null, basic }, { basic, basic } }, noMod, new Vector2I(2, 2), 6, "Basin", deflt, Game1.box);
            bHammer = new(4, 3, new wSet<baseblockType>[,] { { null, basic, null }, { null, basic, null }, { basic, basic, basic }, { basic, basic, basic } }, noMod, new Vector2I(1, 1), 8, "Hammer", deflt, Game1.box);
            bBar = new(4, 2, new wSet<baseblockType>[,] { { basic, basic }, { basic, basic }, { basic, basic }, { basic, basic } }, noMod, new Vector2I(2, 1), 8, "Bar", deflt, Game1.box);
            bLonger = new(5, 1, new wSet<baseblockType>[,] { { basic }, { basic }, { basic }, { basic }, { basic } }, noMod, new Vector2I(3, 0), 5, "Loong", deflt, Game1.boxsolid);
            bAngle = new(3, 3, new wSet<baseblockType>[,] { { null, basic, basic }, { basic, basic, basic }, { basic, basic, basic } }, noMod, new Vector2I(2, 2), 8, "Angle", deflt, Game1.box);

            classicBag = new starterBag([bLong, bSquare, bTBlock, bLBlockR, bLBlockL, bZBlockR, bZBlockL], "Classic Bag");

            bag1 = new starterBag([bCorner, bSquare, bWedge, bStick, bRectangle, bPickL, bPickR, bLong], "bag1");
            bag2 = new starterBag([bDipole, bLongT, bHatchetL, bHatchetR, bTwig, bWedge, bHookL, bHookR, bLong, bNeedle], "bag2");
            bag3 = new starterBag([bNub, bCaret, bBowl, bStump, bSlash, bTwig, bLong, bLepton], "bag3");

            foundryBag = new starterBag([bBarycenter, bHammer, bBar, bLong, bAngle, bCorner, bTwig, bSquare], "Foundry");

            debugbag = new starterBag([bTestPiece3], "debugbag");

        }*/
    }
}
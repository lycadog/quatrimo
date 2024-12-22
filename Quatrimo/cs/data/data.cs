using Microsoft.Xna.Framework;
using System;

namespace Quatrimo
{
    public static class data
    {
        public static keybind leftKey = new(Microsoft.Xna.Framework.Input.Keys.Left, Microsoft.Xna.Framework.Input.Keys.A);
        public static keybind rightKey = new(Microsoft.Xna.Framework.Input.Keys.Right, Microsoft.Xna.Framework.Input.Keys.D);
        public static keybind upKey = new(Microsoft.Xna.Framework.Input.Keys.Up, Microsoft.Xna.Framework.Input.Keys.W);
        public static keybind downKey = new(Microsoft.Xna.Framework.Input.Keys.Down, Microsoft.Xna.Framework.Input.Keys.S);
        public static keybind slamKey = new(Microsoft.Xna.Framework.Input.Keys.Space, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind leftRotateKey = new(Microsoft.Xna.Framework.Input.Keys.Q, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind rightRotateKey = new(Microsoft.Xna.Framework.Input.Keys.E, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind holdKey = new(Microsoft.Xna.Framework.Input.Keys.F, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind pieceAbilityKey = new(Microsoft.Xna.Framework.Input.Keys.C, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind restartKey = new(Microsoft.Xna.Framework.Input.Keys.R, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind pauseKey = new(Microsoft.Xna.Framework.Input.Keys.Escape, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind toggleDebugKey = new(Microsoft.Xna.Framework.Input.Keys.OemTilde, Microsoft.Xna.Framework.Input.Keys.None);

        public static keybind debugMode1 = new keybind(Microsoft.Xna.Framework.Input.Keys.OemOpenBrackets, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind debugMode2 = new keybind(Microsoft.Xna.Framework.Input.Keys.OemCloseBrackets, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind debugMode3 = new keybind(Microsoft.Xna.Framework.Input.Keys.OemBackslash, Microsoft.Xna.Framework.Input.Keys.None);

        public static keybind debugKey1 = new keybind(Microsoft.Xna.Framework.Input.Keys.F1, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind debugKey2 = new keybind(Microsoft.Xna.Framework.Input.Keys.F2, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind debugKey3 = new keybind(Microsoft.Xna.Framework.Input.Keys.F3, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind debugKey4 = new keybind(Microsoft.Xna.Framework.Input.Keys.F4, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind debugKey5 = new keybind(Microsoft.Xna.Framework.Input.Keys.F5, Microsoft.Xna.Framework.Input.Keys.None);

        public static keybind[] boardKeys = [leftKey, rightKey, upKey, downKey, slamKey, leftRotateKey, rightRotateKey, holdKey, pieceAbilityKey, restartKey, pauseKey, toggleDebugKey];
        public static keybind[] debugKeys = [debugMode1, debugMode2, debugMode3, debugKey1, debugKey2, debugKey3, debugKey4, debugKey5];

        public static starterLoadout bag1;
        public static starterLoadout bag2;
        public static starterLoadout bag3;

        static Color[] allColors = [new Color(new Vector3(1, 0.16f, 0.16f)), new Color(new Vector3(1, .396f, .396f)), new Color(new Vector3(1, .325f, .102f)), new Color(new Vector3(.96f, .561f, .427f)), new Color(new Vector3(1, .561f, .102f)), new Color(new Vector3(.969f, .702f, .427f)), new Color(new Vector3(1, .729f, .102f)), new Color(new Vector3(.969f, .804f, .427f)), new Color(new Vector3(.961f, 1, 10.2f)), new Color(new Vector3(.945f, .969f, .427f)), new Color(new Vector3(.71f, 1, .102f)), new Color(new Vector3(.79f, .969f, .427f)), new Color(new Vector3(.243f, .957f, .169f)), new Color(new Vector3(.463f, .992f, .404f)), new Color(new Vector3(.106f, 1, .376f)), new Color(new Vector3(.404f, .992f, .58f)), new Color(new Vector3(.059f, 1, .714f)), new Color(new Vector3(.4f, .969f, .796f)), new Color(new Vector3(.059f, .965f, 1)), new Color(new Vector3(.4f, .945f, .969f)), new Color(new Vector3(.059f, .765f, 1)), new Color(new Vector3(.4f, .824f, .969f)), new Color(new Vector3(.059f, .518f, 1)), new Color(new Vector3(.4f, .675f, .969f)), new Color(new Vector3(.059f, .341f, 1)), new Color(new Vector3(.4f, .569f, .969f)), new Color(new Vector3(.102f, .059f, 1)), new Color(new Vector3(.427f, .4f, .969f)), new Color(new Vector3(36.5f, .059f, 1)), new Color(new Vector3(.584f, .4f, .969f)), new Color(new Vector3(.573f, .059f, 1)), new Color(new Vector3(.71f, .4f, .969f)), new Color(new Vector3(.647f, .059f, 1)), new Color(new Vector3(.753f, .4f, .969f)), new Color(new Vector3(.773f, .059f, 1)), new Color(new Vector3(.831f, .4f, .969f)), new Color(new Vector3(.945f, .059f, 1)), new Color(new Vector3(.933f, .4f, .969f)), new Color(new Vector3(1, .059f, .855f)), new Color(new Vector3(.969f, .4f, .878f)), new Color(new Vector3(1, .059f, .608f)), new Color(new Vector3(.969f, .4f, .729f)), new Color(new Vector3(1, .059f, .431f)), new Color(new Vector3(.969f, .4f, .624f)), new Color(new Vector3(1, .059f, .31f)), new Color(new Vector3(.969f, .4f, .553f))];
        static Color[] bag1Colors = [new Color(255, 41, 41), new Color(255, 69, 41), new Color(255, 152, 24), new Color(199, 255, 24), new Color(101, 255, 24), new Color(24, 255, 93), new Color(24, 215, 255), new Color(24, 142, 255), new Color(24, 93, 255), new Color(52, 24, 255), new Color(138, 24, 255), new Color(255, 24, 140)];
        static Color[] bag2Colors = [new Color(255, 182, 8), new Color(255, 236, 8), new Color(216, 255, 8), new Color(41, 255, 133), new Color(41, 255, 218), new Color(41, 179, 255), new Color(62, 41, 255), new Color(113, 41, 255), new Color(163, 8, 255), new Color(255, 8, 119), new Color(255, 41, 77)];
        static Color[] bag3Colors = [new Color(255, 190, 8), new Color(255, 244, 8), new Color(255, 46, 107), new Color(255, 46, 173), new Color(254, 46, 255), new Color(193, 46, 255), new Color(158, 29, 255), new Color(46, 230, 255), new Color(80, 255, 243), new Color(46, 255, 186)];

        public static Func<block>[] blocks = [
            () => { return new block(); },          //0
            () => { return new cursedBlock(); },    //1
            () => { return new bombBlock(); }       //2
        ];

        public static Func<encounter, Vector2I, Vector2I, string, boardPiece>[] pieces = [
            (encounter e, Vector2I dim, Vector2I orgn, string name) => { return new boardPiece(e, dim, orgn, name); }//0
        ];

        public static objPool<int> basic = new(0);

        public static pieceShape sLine = new(new bool[,] { { true }, { true }, { true }, { true } }, 1, 0, 4, "Line");
        public static pieceShape sStick = new(new bool[,] { { true }, { true }, { true } }, 1, 0, 3, "Stick");
        public static pieceShape sTwig = new(new bool[,] { { true }, { true } }, 0, 0, 2, "Twig");
        public static pieceShape sNub = new(new bool[,] { { true } }, 0, 0, 1, "Nub");
        public static pieceShape sWedge = new(new bool[,] { { true, true }, { false, true } }, 0, 1, 3, "Wedge");

        public static pieceShape sSquare = new(new bool[,] { { true, true }, { true, true } }, 1, 1, 4, "Square");
        public static pieceShape sTBlock = new(new bool[,] { { true, true, true }, { false, true, false } }, 0, 1, 4, "T Block");
        public static pieceShape sLeftL = new(new bool[,] { { false, false, true }, { true, true, true } }, 0, 2, 4, "Left L");
        public static pieceShape sRightL = new(new bool[,] { { true, true, true }, { false, false, true } }, 0, 2, 4, "Left R");
        public static pieceShape sZPieceL = new(new bool[,] { { false, true }, { true, true }, { true, false } }, 1, 1, 4, "Left Z");
        public static pieceShape sZPieceR = new(new bool[,] { { true, false }, { true, true }, { false, true } }, 1, 1, 4, "Right Z");

        public static pieceShape sFatLine = new(new bool[,] { { true, true }, { true, true }, { true, true }, { true, true } }, 1, 0, 8, "Fat Line");

        public static pieceShape sCorner = new(new bool[,] { { true, true, true }, { false, false, true }, { false, false, true } }, 0, 2, 5, "Corner");
        public static pieceShape sRectangle = new(new bool[,] { { true, true }, { true, true }, { true, true } }, 1, 0, 6, "Rectangle");
        public static pieceShape sLeftPick = new(new bool[,] { { true, false }, { true, true }, { true, false }, { true, false } }, 2, 0, 5, "Left Pick");
        public static pieceShape sRightPick = new(new bool[,] { { true, false }, { true, false }, { true, true }, { true, false } }, 1, 0, 5, "Right Pick");

        public static pieceShape sDipole = new(new bool[,] { { true, true }, { false, false }, { true, true } }, 1, 0, 4, "Dipole");
        public static pieceShape sLeftHatchet = new(new bool[,] { { true, true }, { true, true }, { true, false }, { true, false } }, 2, 0, 6, "Left Hatchet");
        public static pieceShape sRightHatchet = new(new bool[,] { { true, false }, { true, false }, { true, true }, { true, true } }, 2, 0, 6, "Right Hatchet");
        public static pieceShape sBigT = new(new bool[,] { { true, false, false }, { true, true, true }, { true, false, false } }, 1, 1, 5, "Big T");
        public static pieceShape sLeftHook = new(new bool[,] { { true, false }, { false, true }, { false, true } }, 1, 1, 3, "Left Hook");
        public static pieceShape sRightHook = new(new bool[,] { { false, true }, { false, true }, { true, false } }, 1, 1, 3, "Right Hook");

        public static pieceShape sSlash = new(new bool[,] { { true, false }, { false, true } }, 1, 1, 2, "Slash");
        public static pieceShape sCaret = new(new bool[,] { { true, false }, { false, true }, { true, false } }, 1, 1, 3, "Caret");
        public static pieceShape sStump = new(new bool[,] { { false, true }, { true, true }, { true, true }, { false, true } }, 1, 1, 6, "Stump");
        public static pieceShape sLepton = new(new bool[,] { { true }, { false }, { true } }, 1, 0, 2, "Lepton");
        public static pieceShape sBoson = new(new bool[,] { { false, true, false }, { false, true, true }, { true, false, false } }, 1, 1, 4, "Boson");
        public static pieceShape sBowl = new(new bool[,] { { true, true }, { false, true }, { true, true } }, 1, 1, 5, "Bowl");
        
        public static pieceShape sPhoton = new(new bool[,] { { true }, { false }, { true }, { true } }, 1, 0, 3, "Photon");

        public static void contentInit()
        {
            boxLine = new(sLine, basic, 0, texs.box, allColors);
            boxStick = new(sStick, basic, 0, texs.box, allColors);
            boxTwig = new(sTwig, basic, 0, texs.box, allColors);
            boxWedge = new(sWedge, basic, 0, texs.box, allColors);

            circleLine = new(sLine, basic, 0, texs.circle, bag3Colors);
            circleTwig = new(sTwig, basic, 0, texs.circle, bag3Colors);
            circleWedge = new(sWedge, basic, 0, texs.circle, bag3Colors);
            circleNub = new(sNub, basic, 0, texs.circle, bag3Colors);

            basicCorner = new(sCorner, basic, 0, texs.box, allColors);
            basicLPick = new(sLeftPick, basic, 0, texs.box, allColors);
            basicRPick = new(sRightPick, basic, 0, texs.box, allColors);
            basicRectangle = new(sRectangle, basic, 0, texs.boxdetail, allColors);

            basicDipole = new(sDipole, basic, 0, texs.boxdetail, allColors);
            basicBigT = new(sBigT, basic, 0, texs.boxdetail, allColors);
            basicLeftHatchet = new(sLeftHatchet, basic, 0, texs.box, allColors);
            basicRightHatchet = new(sRightHatchet, basic, 0, texs.box, allColors);
            basicLeftHook = new(sLeftHook, basic, 0, texs.box, allColors);
            basicRightHook = new(sRightHook, basic, 0, texs.box, allColors);

            basicCaret = new(sCaret, basic, 0, texs.circle, bag3Colors);
            basicBowl = new(sBowl, basic, 0, texs.circle, bag3Colors);
            basicBoson = new(sBoson, basic, 0, texs.circledetail, bag3Colors);
            basicStump = new(sStump, basic, 0, texs.circle, bag3Colors);
            basicSlash = new(sSlash, basic, 0, texs.circle, bag3Colors);
            basicLepton = new(sLepton, basic, 0, texs.circledetail, bag3Colors);

            basicSquare = new(sSquare, basic, 0, texs.box, allColors);
            basicTBlock = new(sTBlock, basic, 0, texs.box, allColors);
            basicLeftL = new(sLeftL, basic, 0, texs.box, allColors);
            basicRightL = new(sRightL, basic, 0, texs.box, allColors);
            basicZPieceL = new(sZPieceL, basic, 0, texs.circle, allColors);
            basicZPieceR = new(sZPieceR, basic, 0, texs.circle, allColors);


            splitLine = new(sFatLine, basic, [new blockSpecification(0), new blockSpecification(2)], [0, 1, 0, 1, 0, 1, 0, 1], texs.box, allColors);
            

            bag1 = new([boxLine, basicCorner, basicLPick, basicRPick, basicRectangle, basicSquare, boxTwig, boxWedge], bag1Colors, "bag1");
            bag2 = new([boxLine, basicDipole, basicBigT, basicLeftHatchet, basicRightHatchet, basicLeftHook, basicRightHook, boxTwig], bag2Colors, "bag2");
            bag3 = new([basicZPieceL, basicZPieceR, circleNub, basicCaret, basicStump, basicSlash, circleTwig, circleLine, basicLepton], bag3Colors, "bag3");
        }

        public static simplePiece boxLine;
        public static simplePiece boxStick;
        public static simplePiece boxTwig;
        public static simplePiece boxWedge;

        public static simplePiece circleLine;
        public static simplePiece circleTwig;
        public static simplePiece circleWedge;
        public static simplePiece circleNub;

        public static simplePiece basicSquare;
        public static simplePiece basicTBlock;
        public static simplePiece basicLeftL;
        public static simplePiece basicRightL;
        public static simplePiece basicZPieceL;
        public static simplePiece basicZPieceR;

        public static simplePiece basicCorner;
        public static simplePiece basicLPick;
        public static simplePiece basicRPick;
        public static simplePiece basicRectangle;

        public static simplePiece basicDipole;
        public static simplePiece basicBigT;
        public static simplePiece basicLeftHatchet;
        public static simplePiece basicRightHatchet;
        public static simplePiece basicLeftHook;
        public static simplePiece basicRightHook;

        public static simplePiece basicCaret;
        public static simplePiece basicBowl;
        public static simplePiece basicStump;
        public static simplePiece basicSlash;
        public static simplePiece basicLepton;
        public static simplePiece basicBoson;

        public static detailedPieceType splitLine;
        public static void initialize()
        {

        }

        
    }
}
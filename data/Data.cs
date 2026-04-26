using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

public static class Data
{

    // **********========================================[|||  SHAPES  |||]========================================**********

    public static PieceShape sLine = new(new int[,] { { 1 }, { 2 }, { 3 }, { 4 } }, 2, 0, "Line");

    public static PieceShape sStick = new(new int[,] { { 1 }, { 2 }, { 3 } }, 1, 0, "Stick");
    public static PieceShape sTwig = new(new int[,] { { 1 }, { 2 } }, 0, 0, "Twig");
    public static PieceShape sNub = new(new int[,] { { 1 } }, 0, 0, "Nub");
    public static PieceShape sWedge = new(new int[,] { { 0, 1 }, { 2, 3 } }, 1, 1, "Wedge");
    public static PieceShape sSlash = new(new int[,] { { 0, 1 }, { 2, 0 } }, 0, 0, "Slash");

    public static PieceShape sSmallTee = new(new int[,] { { 0, 1 }, { 2, 2 }, { 0, 3 } }, 1, 1, "Small Tee");
    public static PieceShape sBox = new(new int[,] { { 1, 2 }, { 3, 4 } }, 1, 1, "Box");
    public static DualPieceShape sZPiece = new(new int[,] { { 0, 1, 2 }, { 3, 4, 0 } }, 1, 1, "Zee");
    public static DualPieceShape sLPiece = new(new int[,] { { 1, 2 }, { 0, 3 }, { 0, 4 } }, 1, 1, "Elle");

    public static PieceShape sBigTee = new(new int[,] { { 1, 0, 0 }, { 4, 3, 2 }, { 1, 0, 0 } }, 1, 1, "Big Tee");
    public static DualPieceShape sHatchet = new(new int[,] { { 1, 2 }, { 1, 2 }, { 0, 3 }, { 0, 3 } }, 1, 1, "Hatchet");
    public static PieceShape sDipole = new(new int[,] { { 1, 2 }, { 0, 0 }, { 3, 4 } }, 1, 1, "Dipole");
    public static DualPieceShape sHook = new(new int[,] { { 1, 0 }, { 0, 2 }, { 0, 3 } }, 1, 0, "Hook");
    public static DualPieceShape sRaisedDipole = new(new int[,] { { 1, 2, 0 }, { 0, 0, 0 }, { 0, 3, 4} }, 1, 1, "Raised Dipole");

    public static DualPieceShape sWeirdTee = new(new int[,] { { 0, 1, 2 }, { 3, 4, 0 }, { 0, 3, 0 } }, 1, 1, "Weird Tee");
    public static DualPieceShape sLTee = new(new int[,] { { 0, 1, 0 }, { 2, 1, 0 }, { 0, 2, 0 }, { 0, 3, 4 } }, 1, 1, "Elle Tee");

    public static PieceShape sLongBigTee = new(new int[,] { { 1, 2, 1 }, { 0, 3, 0 }, { 0, 3, 0 }, { 0, 4, 0 } }, 1, 1, "Long Big Tee");
    public static DualPieceShape sLongL = new(new int[,] { { 1, 2 }, { 0, 3 }, { 0, 3 }, { 0, 4 } }, 1, 1, "Long Elle");
    public static DualPieceShape sLongHook = new(new int[,] { { 1, 0 }, { 0, 2 }, { 0, 2 }, { 0, 3 } }, 1, 1, "Long Hook");
    public static PieceShape sTripole = new(new int[,] { { 0, 1 }, { 0, 0 }, { 2, 2 }, { 0, 0 }, { 0, 4 } }, 2, 1, "Tripole");
    public static DualPieceShape sLineHook = new(new int[,] { { 1, 0 }, { 0, 2 }, { 0, 3 }, { 0, 3 }, { 0, 4 } }, 2, 1, "Line Hook");
    public static DualPieceShape sHookedWedge = new(new int[,] { { 1, 0, 2 }, { 0, 3, 4 } }, 1, 1, "Hooked Wedge A");
    public static DualPieceShape sHookedWedgeB = new(new int[,] { { 0, 0, 1 }, { 0, 2, 0 }, { 3, 4, 0 } }, 2, 1, "Hooked Wedge B");
    public static DualPieceShape sHookedTee = new(new int[,] { { 0, 1, 0 }, { 2, 2, 0 }, { 0, 3, 0 }, { 0, 0, 4 } }, 1, 1, "Hooked Tee");
    public static DualPieceShape sLongZee = new(new int[,] { { 0, 1 }, { 2, 2 }, { 3, 3 }, { 4, 0 } }, 1, 1, "Long Zee");
    public static DualPieceShape sTwinBoxes = new(new int[,] { { 1, 1, 0 }, { 2, 2, 0 }, { 0, 3, 3 }, { 0, 4, 4 } }, 2, 1, "Twin Boxes");
    public static DualPieceShape sSplitHook = new(new int[,] { { 0, 1 }, { 0, 2 }, { 0, 0 }, { 3, 0 } }, 1, 1, "Split Hook");
    public static PieceShape sElectronPair = new(new int[,] { { 1 }, { 0 }, { 0 }, { 2 } }, 1, 0, "Electron Pair");
    public static DualPieceShape sSplitL = new(new int[,] { { 1, 2 }, { 0, 0 }, { 0, 3 }, { 0, 4 } }, 1, 1, "Split Elle");
    public static DualPieceShape sBranch = new(new int[,] { { 0, 1, 0 }, { 2, 1, 0 }, { 0, 3, 0 }, { 0, 3, 4 }, { 0, 1, 0 } }, 2, 1, "Branch");
    public static PieceShape sHammer = new(new int[,] { { 0, 1, 0, }, { 0, 1, 0 }, { 2, 3, 2 }, { 4, 5, 4 } }, 1, 1, "Hammer");
    public static DualPieceShape sLongPick = new(new int[,] { { 0, 1 }, { 5, 1 }, { 0, 2 }, { 0, 3 }, { 0, 4 } }, 2, 1, "Long Pick");
    public static DualPieceShape sCurvedCorner = new(new int[,] { { 1, 2, 5 }, { 0, 0, 3 }, { 0, 4, 4 } }, 1, 2, "Curved Corner");
    public static DualPieceShape sHookedBowl = new(new int[,] { { 1, 0 }, { 2, 2 }, { 0, 3 }, { 4, 4 } }, 2, 1, "Hooked Bowl");
    public static PieceShape sSword = new(new int[,] { { 0, 1, 0 }, { 2, 3, 2 }, { 0, 3, 0 }, { 0, 4, 0 } }, 1, 1, "Sword");
    public static PieceShape sSplitSlash = new(new int[,] { { 1, 0, 0 }, { 0, 0, 0 }, { 0, 0, 2 } }, 1, 1, "Split Slash");

    public static PieceShape sEllipse = new(new int[,] { { 0, 1, 0 }, { 2, 3, 4 }, { 2, 3, 4 }, { 0, 5, 0 } }, 2, 1, "Ellipse");

    public static DualPieceShape sSpikes = new(new int[,] { { 0, 1 }, { 2, 3 }, { 0, 3 }, { 4, 1 } }, 1, 1, "Spikes");

    public static DualPieceShape sCane = new(new int[,] { { 0, 1 }, { 2, 0 }, { 0, 3 }, { 0, 4 } }, 2, 1, "Cane");

    public static PieceShape sCorner = new(new int[,] { { 1, 2, 3 }, { 0, 0, 2 }, { 0, 0, 4 } }, 0, 2, "Corner");
    public static PieceShape sRectangle = new(new int[,] { { 1, 1 }, { 2, 2 }, { 3, 3 } }, 1, 1, "Rectangle");
    public static DualPieceShape sPick = new(new int[,] { { 0, 1 }, { 2, 3 }, { 0, 4 }, { 0, 4 } }, 1, 1, "Pick");

    public static PieceShape sCaret = new(new int[,] { { 0, 1 }, { 2, 0 }, { 0, 3 } }, 1, 1, "Caret");
    public static PieceShape sGlider = new(new int[,] { { 0, 1, 0 }, { 0, 2, 3 }, { 4, 0, 0 } }, 1, 1, "Glider");
    public static DualPieceShape sStep = new(new int[,] { { 1, 2 }, { 0, 0 }, { 0, 3 } }, 1, 0, "Step");
    public static PieceShape sStump = new(new int[,] { { 0, 1 }, { 2, 3 }, { 2, 3 }, { 0, 4 } }, 1, 1, "Stump");

    public static PieceShape sScatteredWedge = new(new int[,] { { 4, 0, 1 }, { 0, 0, 0 }, { 2, 0, 0 } }, 1, 1, "Scattered Wedge");
    public static PieceShape sTripleHole = new(new int[,] { { 1 }, { 0 }, { 4 }, { 0 }, { 2 } }, 2, 0, "Triple Hole");
    public static DualPieceShape sWeirdLine = new(new int[,] { { 1, 0, 2 }, { 0, 0, 2 }, { 0, 0, 3 }, { 0, 0, 4 } }, 1, 2, "Weird line");
    public static PieceShape sLepton = new(new int[,] { { 1 }, { 0 }, { 3 } }, 1, 0, "Lepton");
    public static DualPieceShape sTangle = new(new int[,] { { 0, 1 }, { 0, 0 }, { 0, 0 }, { 3, 0 } }, 1, 1, "Tangle");
    public static PieceShape sBowl = new(new int[,] { { 1, 1 }, { 0, 2 }, { 3, 3 } }, 1, 1, "Bowl");

    public static PieceShape sTrifecta = new(new int[,] { { 1, 0, 0 }, { 0, 0, 4 }, { 2, 0, 0 } }, 1, 1, "Trifecta");
    public static PieceShape sSeperatedT = new(new int[,] { { 1, 0, 0 }, { 3, 0, 4 }, { 2, 0, 0 } }, 1, 1, "Split Tee");
    public static DualPieceShape sScatteredL = new(new int[,] { { 1, 0, 4 }, { 1, 0, 0 }, { 2, 0, 0 } }, 1, 0, "Split Elle");

    public static DualPieceShape sSPiece = new(new[,] { { 1, 1, 0 }, { 0, 1, 0 }, { 0, 1, 1 } }, 1, 1, "Sssnake");
    public static DualPieceShape sDiagonalLine = new(new[,] { { 1, 0 }, { 1, 0 }, { 0, 1 }, { 0, 1 } }, 1, 0, "Diagonal line");
    public static PieceShape sStairs = new(new int[,] { { 0, 0, 1 }, { 0, 4, 2 }, { 1, 2, 3 } }, 1, 1, "Stairs");
    public static PieceShape sArrow = new(new int[,] { { 1, 0, 2 }, { 3, 4, 3 }, { 0, 4, 0 } }, 1, 1, "Arrow");
    public static DualPieceShape sWeirdStick = new(new int[,] { { 1, 0 }, { 0, 2 }, { 3, 2 }, { 0, 4 } }, 2, 1, "Weird Stick");
    public static PieceShape sPin = new(new int[,] { { 1 }, { 0 }, { 2 }, { 3 } }, 0, 0, "Pin");
    public static PieceShape sNeedle = new(new int[,] { { 0, 1, 0 }, { 2, 0, 2 }, { 0, 3, 0 }, { 0, 4, 0 } }, 2, 1, "Needle");

    public static PieceShape sLongBoy = new(new int[,] { { 1 }, { 2 }, { 3 }, { 2 }, { 4 } }, 2, 0, "long boy");
    public static PieceShape sFatFuck = new(new int[,] { { 1, 1, 1, 1 }, { 1, 4, 4, 1 }, { 2, 4, 4, 2 }, { 3, 3, 3, 3 } }, 1, 1, "fat fuck");
    public static PieceShape sBrick = new(new int[,] { { 1, 2, 1 }, { 3, 4, 3 }, { 3, 4, 3 }, { 1, 2, 1 } }, 2, 2, "Brick");
    public static PieceShape sMegalith = new(new int[,] { { 1, 1 }, { 2, 2 }, { 3, 3 }, { 3, 3 }, { 2, 2 }, { 1, 1 } }, 3, 1, "Megalith");
    public static PieceShape sObelisk = new(new int[,] { { 1, 2 }, { 1, 2 }, { 3, 4 }, { 3, 4 } }, 1, 1, "Obelisk");

    public static SimplePieceDefinition LaserLine = new(sStick, BlockType.Laser);
    public static SimplePieceDefinition BoomBox = new(sBox, BlockType.Bomb);
    public static SimplePieceDefinition FireNub = new(sNub, BlockType.Fire);

    public static ObjectPool<BlockType> AllBlocksPool = new([BlockType.Basic], [BlockType.Reinforced, BlockType.Hologram, BlockType.Diamond, BlockType.Fire], [BlockType.Cursed, BlockType.Laser], 8, 2, 1);

    public static ObjectPool<IHasShape> AllRandomShapes = new([sLine, sStick, sTwig, sNub, sWedge, sSlash, sSmallTee, sBox, sZPiece, 
        sLPiece, sBigTee, sHatchet, sDipole, sHook, sCorner, sRectangle, sPick, sCaret, sGlider, sStump, sStep, sTripleHole, 
        sTrifecta, sLongBoy, sWeirdLine, sLepton, sTangle, sBowl, sSeperatedT, sScatteredL, sScatteredWedge, sSPiece, 
        sDiagonalLine, sStairs, sArrow, sWeirdStick, sPin, sNeedle, sWeirdTee, sLongBigTee, sLongHook, sLongL, sTripole, 
        sRaisedDipole, sRaisedDipole, sLineHook, sSpikes, sHookedWedge, sHookedWedgeB, sHookedTee, sFatFuck, sBrick, sMegalith, 
        sLongZee, sObelisk, sLongZee, sTwinBoxes, sSplitHook, sElectronPair, sSplitL, sBranch, sEllipse, sCane, sHammer, sLongPick,
    sCurvedCorner, sHookedBowl, sSword, sSplitSlash],
        1);



    public static PooledPieceDefinition RandomPiece = new(AllBlocksPool, AllRandomShapes);


    // **********========================================[|||  BAGS  |||]========================================**********

    public static StarterBag magnetBag = new([sBigTee.B, sHatchet.LB, sHatchet.RB, sDipole.B, sHook.LB, sHook.RB, sLine.B, sTwig.B, sWedge.B, FireNub, RandomPiece, RandomPiece, RandomPiece],
           [(334, .62f, .97f), (294, .77f, .973f), (280, .94f, .94f), (258, .65f, .98f), (163, .56f, .97f), (124, .66f, .96f), (348, .75f, .9f), (36, .73f, 1f), (240, .73f, .95f)],
               "magnet bag");

    public static StarterBag ElectricBag = new([sCaret.B, sGlider.B, sStep.LB, sStep.RB, sStump.B, sSlash.B, sSmallTee.B, sLine.B, sTwig.B, sNub.B], [
        (330, 1f, .93f), (310, .9f, .91f), (288, .83f, .97f), (274, .88f, 1f), (266, .83f, .85f), (258, .91f, 1f), (180, .88f, 1f), (160, .92f, .92f), (60, .88f, 1f), (50, .75f, 1f)
        ], new Rect2(0, 40, 10, 10), "quantum bag");

    public static StarterBag longDistanceBag = new([sScatteredWedge.B, sSeperatedT.B, sScatteredL.LB, sScatteredL.RB, sSlash.B, sLepton.B, sTangle.LB, sTangle.RB, sBowl.B, sStick.B, sWedge.B, sNub.B, sLine.B], new Rect2(0, 50, 10, 10), "quanto bag idk");

    public static StarterBag randomBullshitBag = new([sStick.B, sWedge.B, sPin.B, RandomPiece, RandomPiece, RandomPiece, RandomPiece, RandomPiece, RandomPiece, RandomPiece, RandomPiece], "fuck");

    public static StarterBag debugBag = new([sHookedWedgeB.RB], "debug");

}
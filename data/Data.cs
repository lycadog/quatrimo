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
    public static PieceShape sSlash = new(new int[,] { { 1, 0 }, { 0, 2 } }, 0, 0, "Slash");

    public static PieceShape sSmallTee = new(new int[,] { { 0, 1 }, { 2, 2 }, { 0, 3 } }, 1, 1, "Small Tee");
    public static PieceShape sSquare = new(new int[,] { { 1, 2 }, { 3, 4 } }, 1, 1, "Square");
    public static DualPieceShape sZPiece = new(new int[,] { { 0, 1, 2 }, { 3, 4, 0 } }, 1, 1, "Zee");
    public static DualPieceShape sLPiece = new(new int[,] { { 1, 2 }, { 0, 3 }, { 0, 4 } }, 1, 1, "Elle");

    public static PieceShape sBigTee = new(new int[,] { { 1, 0, 0 }, { 4, 3, 2 }, { 1, 0, 0 } }, 1, 1, "Big Tee");
    public static DualPieceShape sHatchet = new(new int[,] { { 1, 2 }, { 1, 2 }, { 0, 3 }, { 0, 3 } }, 1, 1, "Hatchet");
    public static PieceShape sDipole = new(new int[,] { { 1, 2 }, { 0, 0 }, { 3, 4 } }, 1, 1, "Dipole");
    public static DualPieceShape sHook = new(new int[,] { { 1, 0 }, { 0, 2 }, { 0, 3 } }, 1, 0, "Hook");
    public static PieceShape sRaisedDipole = new(new int[,] { { 0, 1, 2 }, { 0, 0, 0 }, { 3, 4, 0 } }, 1, 1, "Raised Dipole");

    public static DualPieceShape sWeirdTee = new(new int[,] { { 0, 1, 2 }, { 3, 4, 0 }, { 0, 3, 0 } }, 1, 1, "temp");
    public static DualPieceShape sLTee = new(new int[,] { { 0, 1, 0 }, { 2, 1, 0 }, { 0, 2, 0 }, { 0, 3, 4 } }, 1, 1, "temp");

    public static PieceShape sLongBigTee = new(new int[,] { { 1, 2, 1 }, { 0, 3, 0 }, { 0, 3, 0 }, { 0, 4, 0 } }, 1, 1, "Long Big Tee");
    public static DualPieceShape sLongL = new(new int[,] { { 1, 2 }, { 0, 3 }, { 0, 3 }, { 0, 4 } }, 1, 1, "Long Elle");
    public static DualPieceShape sLongHook = new(new int[,] { { 1, 0 }, { 0, 2 }, { 0, 2 }, { 0, 3 } }, 1, 1, "Long Hook");
    public static PieceShape sTripole = new(new int[,] { { 0, 1 }, { 0, 0 }, { 2, 2 }, { 0, 0 }, { 0, 4 } }, 2, 1, "Tripole");
    public static DualPieceShape sLineHook = new(new int[,] { { 1, 0 }, { 0, 2 }, { 0, 3 }, { 0, 3 }, { 0, 4 } }, 2, 1, "Line Hook");
    public static DualPieceShape sHookedWedge = new(new int[,] { { 1, 0, 2 }, { 0, 3, 4 } }, 1, 1, "Hooked Wedge A");
    public static DualPieceShape sHookedWedgeB = new(new int[,] { { 0, 0, 1 }, { 0, 2, 0 }, { 3, 4, 0 } }, 2, 1, "Hooked Wedge B");
    public static DualPieceShape sHookedTee = new(new int[,] { { 0, 1, 0 }, { 2, 2, 0 }, { 0, 3, 0 }, { 0, 0, 4 } }, 1, 1, "Hooked Tee");

    public static DualPieceShape sSpikes = new(new int[,] { { 0, 1 }, { 2, 3 }, { 0, 3 }, { 4, 1 } }, 1, 1, "Spikes");

    public static PieceShape sCorner = new(new int[,] { { 4, 2, 1 }, { 2, 0, 0 }, { 3, 0, 0 } }, 0, 0, "Corner");
    public static PieceShape sRectangle = new(new int[,] { { 1, 1 }, { 2, 2 }, { 3, 3 } }, 1, 1, "Rectangle");
    public static DualPieceShape sPick = new(new int[,] { { 0, 1 }, { 2, 3 }, { 0, 4 }, { 0, 4 } }, 1, 1, "Pick");

    public static PieceShape sCaret = new(new int[,] { { 0, 1 }, { 2, 0 }, { 0, 3 } }, 1, 1, "Caret");
    public static PieceShape sGlider = new(new int[,] { { 0, 1, 0 }, { 0, 2, 3 }, { 4, 0, 0 } }, 1, 1, "Glider");
    public static DualPieceShape sStep = new(new int[,] { { 1, 2 }, { 0, 0 }, { 0, 3 } }, 1, 0, "Step");
    public static PieceShape sStump = new(new int[,] { { 0, 1 }, { 2, 3 }, { 2, 3 }, { 0, 4 } }, 1, 1, "Stump");

    public static PieceShape sScatteredWedge = new(new int[,] { { 4, 0, 1 }, { 0, 0, 0 }, { 2, 0, 0 } }, 1, 1, "temp");
    public static PieceShape sTripleHole = new(new int[,] { { 1 }, { 0 }, { 4 }, { 0 }, { 2 } }, 2, 0, "temp");
    public static DualPieceShape sWeirdLine = new(new int[,] { { 1, 0, 2 }, { 0, 0, 2 }, { 0, 0, 3 }, { 0, 0, 4 } }, 1, 0, "temp");
    public static PieceShape sLepton = new(new int[,] { { 1 }, { 0 }, { 3 } }, 1, 0, "Lepton");
    public static DualPieceShape sTangle = new(new int[,] { { 0, 1 }, { 0, 0 }, { 0, 0 }, { 3, 0 } }, 1, 1, "Tangle");
    public static PieceShape sBowl = new(new int[,] { { 1, 1 }, { 0, 2 }, { 3, 3 } }, 1, 1, "Bowl");

    public static PieceShape sTrifecta = new(new int[,] { { 1, 0, 0 }, { 0, 0, 4 }, { 2, 0, 0 } }, 1, 1, "Trifecta");
    public static PieceShape sSeperatedT = new(new int[,] { { 1, 0, 0 }, { 3, 0, 4 }, { 2, 0, 0 } }, 1, 1, "temp");
    public static DualPieceShape sScatteredL = new(new int[,] { { 1, 0, 4 }, { 1, 0, 0 }, { 2, 0, 0 } }, 1, 0, "temp");

    public static DualPieceShape sSPiece = new(new[,] { { 1, 1, 0 }, { 0, 1, 0 }, { 0, 1, 1 } }, 1, 1, "Sssnake");
    public static DualPieceShape sDiagonalLine = new(new[,] { { 1, 0 }, { 1, 0 }, { 0, 1 }, { 0, 1 } }, 1, 0, "Diagonal line");
    public static PieceShape sStairs = new(new int[,] { { 0, 0, 1 }, { 0, 4, 2 }, { 1, 2, 3 } }, 1, 1, "Stairs");
    public static PieceShape sArrow = new(new int[,] { { 1, 0, 2 }, { 3, 4, 3 }, { 0, 4, 0 } }, 1, 1, "Arrow");
    public static DualPieceShape sBranch = new(new int[,] { { 1, 0 }, { 0, 2 }, { 3, 2 }, { 0, 4 } }, 2, 1, "Branch");
    public static PieceShape sPin = new(new int[,] { { 1 }, { 0 }, { 2 }, { 3 } }, 0, 0, "Pin");
    public static PieceShape sNeedle = new(new int[,] { { 0, 2, 0, 0 }, { 1, 0, 3, 4 }, { 0, 2, 0, 0 } }, 1, 1, "Needle");

    public static PieceShape sLongBoy = new(new int[,] { { 1 }, { 2 }, { 3 }, { 2 }, { 4 } }, 2, 0, "long boy");
    public static PieceShape sFatFuck = new(new int[,] { { 1, 1, 1, 1 }, { 1, 4, 4, 1 }, { 2, 4, 4, 2 }, { 3, 3, 3, 3 } }, 1, 1, "fat fuck");

    public static SimplePieceDefinition LaserLine = new(sStick, BlockType.Laser);
    public static SimplePieceDefinition BoomBox = new(sSquare, BlockType.Bomb);

    public static ObjectPool<BlockType> AllBlocksPool = new([BlockType.Basic], [BlockType.Reinforced, BlockType.Hologram, BlockType.Diamond], [BlockType.Cursed], 8, 2, 1);

    public static ObjectPool<PieceShape> AllRandomShapes = new([sLine, sStick, sTwig, sNub, sWedge, sSlash, sSmallTee, sSquare, sZPiece.L, sZPiece.R, sLPiece.L, sLPiece.R, sBigTee, sHatchet.L, sHatchet.R, sDipole, sHook.L, sHook.R, sCorner, sRectangle, 
        sPick.L, sPick.R, sCaret, sGlider, sStump, sStep.L, sStep.R, sTripleHole, sTrifecta, sLongBoy, sWeirdLine.L, sWeirdLine.R, 
        sLepton, sTangle.L, sTangle.R, sBowl, sSeperatedT, sScatteredL.L, sScatteredL.R, sScatteredWedge, sSPiece.L, sSPiece.R,
    sDiagonalLine.L, sDiagonalLine.R, sStairs, sArrow, sBranch.L, sBranch.R, sPin, sNeedle, sWeirdTee.L, sWeirdTee.R, sLongBigTee,
    sLongHook.L, sLongHook.R, sLongL.L, sLongL.R, sTripole, sRaisedDipole, sRaisedDipole, sLineHook.L, sLineHook.R,
    sSpikes.L, sSpikes.R, sHookedWedge.L, sHookedWedge.R, sHookedWedgeB.L, sHookedWedgeB.R, sHookedTee.L, sHookedTee.R],
        1);

    public static SimplePieceDefinition CursedBoy = new(sLepton, BlockType.Cursed);
    public static SimplePieceDefinition CursedNub = new(sNub, BlockType.Cursed);
    public static SimplePieceDefinition BigHolo = new(sRectangle, BlockType.Hologram);
    public static SimplePieceDefinition LaserBeam = new(sTwig, BlockType.Laser);
    public static SimplePieceDefinition ElDiamonde = new(sLongBoy, BlockType.Diamond);

    public static PooledPieceDefinition RandomPiece = new(AllBlocksPool, AllRandomShapes);


    // **********========================================[|||  BAGS  |||]========================================**********

    public static StarterBag magnetBag = new([sBigTee.B, sHatchet.LB, sHatchet.RB, sDipole.B, sHook.LB, sHook.RB, sLine.B, sTwig.B, sWedge.B, BigHolo, ElDiamonde, CursedNub],
           [(334, .62f, .97f), (294, .77f, .973f), (280, .94f, .94f), (258, .65f, .98f), (163, .56f, .97f), (124, .66f, .96f), (348, .75f, .9f), (36, .73f, 1f), (240, .73f, .95f)],
               "magnet bag");

    public static StarterBag ElectricBag = new([sCaret.B, sGlider.B, sStep.LB, sStep.RB, sStump.B, sSlash.B, sSmallTee.B, sLine.B, sTwig.B, sNub.B, BigHolo], [
        (330, 1f, .93f), (310, .9f, .91f), (288, .83f, .97f), (274, .88f, 1f), (266, .83f, .85f), (258, .91f, 1f), (180, .88f, 1f), (160, .92f, .92f), (60, .88f, 1f), (50, .75f, 1f)
        ], new Rect2(0, 40, 10, 10), "quantum bag");

    public static StarterBag longDistanceBag = new([sScatteredWedge.B, sSeperatedT.B, sScatteredL.LB, sScatteredL.RB, sSlash.B, sLepton.B, sTangle.LB, sTangle.RB, sBowl.B, sStick.B, sWedge.B, sNub.B, sLine.B], new Rect2(0, 50, 10, 10), "quanto bag idk");

    public static StarterBag randomBullshitBag = new([sLPiece.LB, sLPiece.RB, RandomPiece, RandomPiece, RandomPiece, RandomPiece, RandomPiece, RandomPiece, RandomPiece, RandomPiece, RandomPiece, RandomPiece], "fuck");

    public static StarterBag debugBag = new([sSPiece.LB, sDiagonalLine.LB, sLine.B], "debug");

}
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

public static class Data
{

    #region === Piece Shapes ===

    #region === Basic pieces ===

    public static PieceShape sNub = new([(0, 0)], "Nub");
    public static PieceShape sWedge = new([(-.5f, .5f), (.5f, .5f), (.5f, -.5f)], "Wedge");
    public static PieceShape sSlash = new([(-1, -1), (0, 0)], "Slash");

    public static PieceShape sSmallTee = new([(-1, 0), (0, 0), (0, -1), (1, 0)], "Small Tee");
    public static PieceShape sBox = new([(-.5f, -.5f), (-.5f, .5f), (.5f, .5f), (.5f, -.5f)], "Box");
    public static DualPieceShape sZPiece = new([(-1, -1), (0, -1), (0, 0), (1, 0)], "Zee");
    public static DualPieceShape sLPiece = new([(-1, -1), (-1, 0), (0, 0), (1, 0)], "Elle");
    public static PieceShape sRectangle = new([(-1, -1), (-1, 0), (0, -1), (0, 0), (1, -1), (1, 0)], "Rectangle");
    public static DualPieceShape sHook = new([(-1, -1), (0, 0), (1, 0)], "Hook");

    #endregion
    #region === Long Pieces

    public static PieceShape sLine = new([(-1.5f, .5f), (-.5f, .5f), (.5f, .5f), (1.5f, .5f)], "Line");
    public static PieceShape sStick = new([(-1, 0), (0, 0), (1, 0)], "Stick");
    public static PieceShape sTwig = new([(-.5f, .5f), (.5f, .5f)], "Twig");

    public static PieceShape sStump = new([(-1, 0), (0, -1), (0, 0), (1, -1), (1, 0), (2, 0)], "Stump");
    public static DualPieceShape sHatchet = new([(-1, -1), (0, -1), (-1, 0), (0, 0), (1, 0), (2, 0)], "Hatchet");
    public static DualPieceShape sDiagonalLine = new([(-1.5f, -.5f), (-.5f, -.5f), (.5f, .5f), (1.5f, .5f)], "Diagonal line");
    public static DualPieceShape sTwinWedges = new([(-1.5f, -.5f), (-1.5f, .5f), (-.5f, .5f), (.5f, -.5f), (1.5f, -.5f), (1.5f, .5f)], "Twin Wedges");
    public static DualPieceShape sCane = new([(-2, 0), (-1, -1), (0, 0), (1, 0)], "Cane");
    public static DualPieceShape sPick = new([(-1, 0), (0, -1), (0, 0), (1, 0), (2, 0)], "Pick");
    public static PieceShape sPin = new([(0, 0), (2, 0), (3, 0)], "Pin");

    public static DualPieceShape sBranch = new([(-2, 0), (-1, 0), (-1, -1), (0, 0), (1, 0), (1, 1)], "Branch");
    public static DualPieceShape sSpikes = new([(-2, 0), (-1, -1), (-1, 0), (0, 0), (1, -1), (1, 0)], "Spikes");
    public static DualPieceShape sWrench = new([(-2, -1), (-1, -1), (-1, 0), (0, 0), (1, 0)], "Wrench");

    public static PieceShape sLongLine = new([(-2, 0), (-1, 0), (0, 0), (1, 0), (2, 0)], "Long Line", 5);
    public static PieceShape sLongerLine = new([(-3, 0), (-2, 0), (-1, 0), (0, 0), (1, 0), (2, 0)], "Longer Line", 4);
    public static PieceShape sLongestLine = new([(-3, 0), (-2, 0), (-1, 0), (0, 0), (1, 0), (2, 0), (3, 0)], "Longest Line", 3);
    public static PieceShape sLongesterLine = new([(-4, 0), (-3, 0), (-2, 0), (-1, 0), (0, 0), (1, 0), (2, 0), (3, 0)], "Longester Line", 2);
    public static PieceShape sTrueLongestLine = new([(-4, 0), (-3, 0), (-2, 0), (-1, 0), (0, 0), (1, 0), (2, 0), (3, 0), (4, 0)], "True Longest Line", 1);


    #endregion
    #region === Variants of Other Pieces ===
    
    public static PieceShape sLongBigTee = new([(-1, -1), (-1, 0), (-1, 1), (0, 0), (1, 0), (2, 0)], "Long Big Tee");
    public static DualPieceShape sLongL = new([(-1, -1), (-1, 0), (0, 0), (1, 0), (2, 0)], "Long Elle");
    public static DualPieceShape sLongHook = new([(-1.5f, -0.5f), (-.5f, .5f), (.5f, .5f), (1.5f, .5f)], "Long Hook");
    public static DualPieceShape sLineHook = new([(-2, 0), (-1, 0), (0, 0), (1, 0), (2, -1)], "Line Hook");
    public static DualPieceShape sHookedWedge = new([(-2, 1), (-1, 0), (0, 0), (0, -1)], "Hooked Wedge");
    public static DualPieceShape sHookedTeeA = new([(-1, 0), (0, 0), (0, -1), (1, 0), (2, -1)], "Hooked Tee A");
    public static DualPieceShape sHookedTeeB = new([(-2, 1), (-1, 0), (0, 0), (0, -1), (1, 0)], "Hooked Tee B");
    public static DualPieceShape sLongZee = new([(-2, -1), (-1, -1), (-1, 0), (0, -1), (0, 0), (1, 0),], "Long Zee");
    public static DualPieceShape sTwinBoxes = new([(-2, -1), (-2, 0), (-1, -1), (-1, 0), (0, 0), (0, 1), (1, 0), (1, 1)], "Twin Boxes");
    public static DualPieceShape sSplitHook = new([(-2, -1), (0, 0), (1, 0)], "Split Hook");
    public static DualPieceShape sSplitL = new([(-2, -1), (-2, 0), (0, 0), (1, 0)], "Split Elle");

    public static PieceShape sRoundedCorner = new([(-1, 1), (0, 1), (1, 0), (1, -1)], "Rounded Corner");
    public static PieceShape sEscalator = new([(-1, 1), (0, 1), (0, 0), (1, 0), (1, -1)], "Escalator");

    public static DualPieceShape sLongPick = new([(-2, 0), (-1, 0), (-1, -1), (0, 0), (1, 0), (2, 0)], "Long Pick");
    public static DualPieceShape sCurvedCorner = new([(-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0), (1, -1)], "Curved Corner");
    public static DualPieceShape sHookedBowl = new([(-2, 0), (-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0)], "Hooked Bowl");
    public static DualPieceShape sWeirdTee = new([(-1, 0), (-1, 1), (0, 0), (0, -1), (1, 0)], "Weird Tee");
    public static DualPieceShape sLTee = new([(-1, 0), (-1, 1), (0, -1), (0, 0), (1, 0)], "Elle Tee");

    #endregion
    #region === Split Pieces ===

    public static PieceShape sDipole = new([(-1, -1), (-1, 0), (1, -1), (1, 0)], "Dipole");
    public static PieceShape sFarDipole = new([(-1.5f, -.5f), (-1.5f, .5f), (1.5f, -.5f), (1.5f, .5f)], "Far Dipole");
    public static DualPieceShape sRaisedDipole = new([(-1, -1), (-1, 0), (1, 0), (1, 1)], "Raised Dipole");
    public static PieceShape sTripole = new([(-2, 0), (0, 0), (0, -1), (2, 0)], "Tripole");
    public static DualPieceShape sPivot = new([(-2, 0), (0, -1), (0, 0), (2, -1)], "Pivot");
    public static PieceShape sTripleHole = new([(-2, 0), (0, 0), (2, 0)], "Triple Hole");
    public static DualPieceShape sStep = new([(-1, -1), (-1, 0), (1, 0)], "Step");


    public static PieceShape sSplitLine = new([(-2, 0), (-1, 0), (1, 0), (2, 0)], "Split Line");
    public static PieceShape sSplitSlash = new([(-1, -1), (1, 1)], "Split Slash");

    public static PieceShape sCaret = new([(-1, 0), (0, -1), (1, 0)], "Caret");
    public static DualPieceShape sTwist = new([(-1.5f, .5f), (-.5f, -.5f), (.5f, .5f), (1.5f, -.5f)], "Twist");
    public static DualPieceShape sMonkeyWrench = new([(-1.5f, -.5f), (-1.5f, .5f), (-.5f, .5f), (.5f, -.5f), (1.5f, -.5f)], "Monkey Wrench");
    public static DualPieceShape sWand = new([(-1.5f, -.5f), (-.5f, .5f), (.5f, -.5f), (.5f, .5f), (1.5f, -.5f)], "Wand");

    public static DualPieceShape sSplitComma = new([(-1.5f, .5f), (-.5f, -.5f), (-.5f, .5f), (1.5f, -.5f), (1.5f, .5f)], "Split Comma");

    public static PieceShape sScatteredWedge = new([(-2, 0), (0, 0), (0, -2)], "Scattered Wedge");
    public static DualPieceShape sWeirdLine = new([(-1.5f, -1.5f), (-1.5f, .5f), (-.5f, .5f), (.5f, .5f), (1.5f, .5f)], "Weird line");
    public static PieceShape sLepton = new([(-2, 0), (0, 0)], "Lepton");
    public static DualPieceShape sRaisedLepton = new([(-1, -1), (1, 0)], "Raised Lepton");
    public static DualPieceShape sTangle = new([(-1.5f, .5f), (1.5f, -.5f)], "Tangle");
    public static DualPieceShape sLongTangle = new([(-2, 0), (2, -1)], "Long Tangle");

    public static PieceShape sPhoton = new([(0, 0), (2, 0), (3, 0), (4, 0)], "Photon");
    public static PieceShape sElectronPair = new([(-2, 0), (1, 0)], "Electron Pair");
    public static PieceShape sQuantumLoop = new([(-2, 0), (2, 0)], "Quantum Loop");
    public static PieceShape sDistantStars = new([(-2, 0), (3, 0)], "Distant Stars");

    public static PieceShape sTrifecta = new([(-1, -1), (0, 1), (1, -1)], "Trifecta");
    public static PieceShape sSeperatedT = new([(-1, 0), (0, -2), (0, 0), (1, 0)], "Split Tee");
    public static DualPieceShape sScatteredL = new([(-1, -2), (-1, 0), (0, 0), (1, 0)], "Split Elle");


    #endregion
    #region === Rectangular and Medium-Sized Pieces ===

    public static PieceShape sBigBox = new([(-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 0), (0, 1), (1, -1), (1, 0), (1, 1)], "Big Box");
    public static PieceShape sBigTee = new([(-1, -1), (0, -1), (1, -1), (0, 0), (0, 1)], "Big Tee");
    public static PieceShape sCorner = new([(-1, 1), (0, 1), (1, 1), (1, 0), (1, -1)], "Corner");
    public static PieceShape sStairs = new([(-1, 1), (0, 0), (0, 1), (1, -1), (1, 0), (1, 1)], "Stairs");
    public static DualPieceShape sComma = new([(-1.5f, .5f), (-.5f, -.5f), (-.5f, .5f), (.5f, .5f), (.5f, -.5f)], "Comma");

    public static PieceShape sBowl = new([(-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0)], "Bowl");
    public static DualPieceShape sChippedBowl = new([(-1, 0), (0, 1), (1, 1), (1, 0)], "Chipped Bowl");
    public static PieceShape sBasin = new([(-1.5f, -.5f), (-1.5f, .5f), (-.5f, .5f), (0.5f, .5f), (1.5f, .5f), (1.5f, -.5f)], "Basin");
    public static PieceShape sRing = new([(-1.5f, .5f), (-.5f, -.5f), (.5f, -.5f), (1.5f, .5f)], "Ring");

    public static DualPieceShape sBoxes = new([(-1, -1), (-1, 0), (0, -1), (0, 0), (0, 1), (1, 0), (1, 1)], "Boxes"); //TODO RENAME

    public static DualPieceShape sSPiece = new([(-1, -1), (-1, 0), (0, 0), (1, 0), (1, 1)], "Sssnake");
    public static PieceShape sArrow = new([(-1, -1), (-1, 0), (0, 0), (0, 1), (1, -1), (1, 0)], "Arrow");
    public static PieceShape sGlider = new([(-1, 0), (0, -1), (0, 0), (1, 1)], "Glider");
    public static PieceShape sLongSlash = new([(-1, -1), (0, 0), (1, 1)], "Long Slash");
    public static PieceShape sHollowCore = new([(-2, 0), (-1, -1), (-1, 1), (0, 0)], "Hollow Core");
    public static PieceShape sDiamond = new([(-1, 0), (0, -1), (0, 0), (0, 1), (1, 0)], "Diamond");

    #endregion
    #region === Weirdos ===

    public static PieceShape sNeedle = new([(-2, 0), (-1, -1), (-1, 1), (0, 0), (1, 0)], "Needle");
    public static PieceShape sSword = new([(-1, 0), (0, -1), (0, 0), (0, 1), (1, 0), (2, 0)], "Sword");

    #endregion
    #region === Fat Fucks ===

    public static PieceShape sEllipse = new([(-1.5f, .5f), (-.5f, -.5f), (-.5f, 0.5f), (-.5f, 1.5f), (.5f, -.5f), (.5f, 0.5f), (.5f, 1.5f), (1.5f, 0.5f)], "Ellipse");
    public static PieceShape sHammer = new([(-1, 0), (0, 0), (1, -1), (1, 0), (1, 1), (2, -1), (2, 0), (2, 1)], "Hammer");

    public static PieceShape sFatFuck = new([(-1.5f, -1.5f), (-1.5f, -.5f), (-1.5f, .5f), (-1.5f, 1.5f), (-.5f, -1.5f), (-.5f, -.5f), (-.5f, .5f), (-.5f, 1.5f), (.5f, -1.5f), (.5f, -.5f), (.5f, .5f), (.5f, 1.5f), (1.5f, -1.5f), (1.5f, -.5f), (1.5f, .5f), (1.5f, 1.5f)], "fat fuck", 2);
    public static PieceShape sBrick = new([(-1.5f, -.5f), (-1.5f, .5f), (-1.5f, 1.5f), (-.5f, -.5f), (-.5f, .5f), (-.5f, 1.5f), (.5f, -.5f), (.5f, .5f), (.5f, 1.5f), (1.5f, -.5f), (1.5f, .5f), (1.5f, 1.5f)], "Brick", 4);
    public static PieceShape sMegalith = new([(-2.5f, -.5f), (-2.5f, .5f), (-1.5f, -.5f), (-1.5f, .5f), (-.5f, -.5f), (-.5f, .5f), (.5f, -.5f), (.5f, .5f), (1.5f, -.5f), (1.5f, .5f), (2.5f, -.5f), (2.5f, .5f)], "Megalith", 5);
    public static PieceShape sObelisk = new([(-1.5f, -.5f), (-1.5f, .5f), (-.5f, -.5f), (-.5f, .5f), (.5f, -.5f), (.5f, .5f), (1.5f, -.5f), (1.5f, .5f)], "Obelisk");
    
    #endregion
    #endregion

    #region === Random Loot Pools ===

    public static ObjectPool<BlockType> AllBlocksPool = new([BlockType.Basic], [BlockType.Reinforced, BlockType.Hologram, BlockType.Diamond], [BlockType.Cursed, BlockType.Laser], 12, 3, 1);
    
    //why is this here?
    public static ObjectPool<IHasShape> DebugShapePool = new(sBox);

    #endregion

    #region === Unique Pieces ===

    public static SimplePieceDefinition LaserLine = new(sStick, BlockType.Laser);
    public static SimplePieceDefinition BoomBox = new(sBox, BlockType.Bomb);
    public static SimplePieceDefinition CursedNub = new(sNub, BlockType.Cursed);

    public static PooledPieceDefinition RandomPiece = new(AllBlocksPool, PieceShape.AllShapes);
    #endregion

    #region === Bags ===

    
    public static StarterBag magnetBag = new([sBigTee.B, sHatchet.LB, sHatchet.RB, sDipole.B, sHook.LB, sHook.RB, sLine.B, sTwig.B, sWedge.B],
           [(334, .62f, .97f), (294, .77f, .973f), (280, .94f, .94f), (258, .65f, .98f), (163, .56f, .97f), (124, .66f, .96f), (348, .75f, .9f), (36, .73f, 1f), (240, .73f, .95f)],
               "magnet bag");

    public static StarterBag ElectricBag = new([sCaret.B, sGlider.B, sStep.LB, sStep.RB, sStump.B, sSlash.B, sSmallTee.B, sLine.B, sTwig.B, sNub.B], [
        (330, 1f, .93f), (310, .9f, .91f), (288, .83f, .97f), (274, .88f, 1f), (266, .83f, .85f), (258, .91f, 1f), (180, .88f, 1f), (160, .92f, .92f), (60, .88f, 1f), (50, .75f, 1f)
        ], new Rect2(0, 40, 10, 10), "quantum bag");

    public static StarterBag longDistanceBag = new([sScatteredWedge.B, sSeperatedT.B, sScatteredL.LB, sScatteredL.RB, sSlash.B, sLepton.B, sTangle.LB, sTangle.RB, sBowl.B, sStick.B, sWedge.B, sNub.B, sLine.B], new Rect2(0, 50, 10, 10), "quanto bag idk");

    public static StarterBag randomBullshitBag = new([sTwig.B, sWedge.B, sLine.B, RandomPiece, RandomPiece, RandomPiece, RandomPiece, RandomPiece, RandomPiece, RandomPiece, RandomPiece], "fuck");
    
    public static StarterBag debugBag = new([sMegalith.B, CursedNub], "debug");

    #endregion
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

public static class Data
{
    public static PieceShape sLine = new(new int[,] { { 1 }, { 2 }, { 3 }, { 4 } }, 2, 0, "Line");

    public static PieceShape sStick = new(new int[,] { { 1 }, { 2 }, { 3 } }, 1, 0, "Stick");
    public static PieceShape sTwig = new(new int[,] { { 1 }, { 2 } }, 0, 0, "Twig");
    public static PieceShape sNub = new(new int[,] { { 1 } }, 0, 0, "Nub");
    public static PieceShape sWedge = new(new int[,] { { 1, 0 }, { 2, 3 } }, 1, 0, "Wedge");
    public static PieceShape sSlash = new(new int[,] { { 1, 0 }, { 0, 2 } }, 0, 0, "Slash");

    public static PieceShape sSmallTee = new(new int[,] { { 0, 1 }, { 2, 2 }, { 0, 3 } }, 1, 1, "Small Tee");
    public static PieceShape sSquare = new(new int[,] { { 1, 2 }, { 3, 4 } }, 1, 1, "Square");
    public static PieceShape sLZPiece = new(new int[,] { { 0, 1, 2 }, { 3, 4, 0 } }, 1, 1, "Left Zee");
    public static PieceShape sRZPiece = new(new int[,] { { 1, 2, 0 }, { 0, 3, 4 } }, 1, 1, "Right Zee");
    public static PieceShape sLLPiece = new(new int[,] { { 1, 2, 3 }, { 4, 0, 0 } }, 1, 1, "Left Elle");
    public static PieceShape sRLPiece = new(new int[,] { { 1, 0, 0 }, { 2, 3, 4 } }, 1, 1, "Right Elle");

    public static PieceShape sBigTee = new(new int[,] { { 1, 2, 1 }, { 0, 3, 0 }, { 0, 4, 0 } }, 1, 1, "Big Tee");
    public static PieceShape sLHatchet = new(new int[,] { { 1, 2 }, { 1, 2 }, { 3, 0 }, { 3, 0 } }, 2, 0, "Left Hatchet");
    public static PieceShape sRHatchet = new(new int[,] { { 1, 0 }, { 1, 0 }, { 2, 3 }, { 2, 3 } }, 1, 0, "Right Hatchet");
    public static PieceShape sDipole = new(new int[,] { { 1, 2 }, { 0, 0 }, { 3, 4 } }, 1, 1, "Dipole");
    public static PieceShape sLHook = new(new int[,] { { 0, 1 }, { 2, 0 }, { 3, 0 } }, 1, 0, "Left Hook");
    public static PieceShape sRHook = new(new int[,] { { 1, 0 }, { 2, 0 }, { 0, 3 } }, 1, 0, "Right Hook");

    public static PieceShape sCorner = new(new int[,] { { 1, 1, 2 }, { 3, 0, 0 }, { 4, 0, 0 } }, 0, 2, "Corner");
    public static PieceShape sRectangle = new(new int[,] { { 1, 1 }, { 2, 2 }, { 3, 3 } }, 1, 1, "Rectangle");
    public static PieceShape sLPick = new(new int[,] { { 1, 0 }, { 2, 3 }, { 4, 0 }, { 4, 0 } }, 1, 1, "Left Pick");
    public static PieceShape sRPick = new(new int[,] { { 1, 0 }, { 1, 0 }, { 2, 3 }, { 4, 0 } }, 2, 1, "Right Pick");

    public static PieceShape sCaret = new(new int[,] { { 0, 1 }, { 2, 0 }, { 0, 3 } }, 1, 1, "Caret");
    public static PieceShape sGlider = new(new int[,] { { 0, 1, 0 }, { 0, 2, 3 }, { 4, 0, 0 } }, 1, 1, "Glider");
    public static PieceShape sLStep = new(new int[,] { { 1, 2 }, { 0, 0 }, { 3, 0 } }, 1, 0, "Left Step");
    public static PieceShape sRStep = new(new int[,] { { 3, 0 }, { 0, 0 }, { 1, 2 } }, 1, 0, "Right Step");
    public static PieceShape sStump = new(new int[,] { { 1, 0 }, { 2, 3 }, { 2, 3 }, { 4, 0 } }, 1, 1, "Stump");

    public static PieceShape sScatteredWedge = new(new int[,] { { 4, 0, 1 }, { 0, 0, 0 }, { 2, 0, 0 } }, 1, 1, "temp");
    public static PieceShape sTripleHole = new(new int[,] { { 1 }, { 0 }, { 4 }, { 0 }, { 2 } }, 2, 0, "temp");
    public static PieceShape sLWeirdLine = new(new int[,] { { 2, 0, 1 }, { 2, 0, 0 }, { 3, 0, 0 }, { 4, 0, 0 } }, 1, 0, "temp");
    public static PieceShape sRWeirdLine = new(new int[,] { { 4, 0, 0 }, { 3, 0, 0 }, { 2, 0, 0 }, { 2, 0, 1 } }, 2, 0, "temp");
    public static PieceShape sLepton = new(new int[,] { { 1 }, { 0 }, { 3 } }, 1, 0, "Lepton");
    public static PieceShape sLTangle = new(new int[,] { { 0, 1 }, { 0, 0 }, { 0, 0 }, { 3, 0 } }, 1, 1, "Left Tangle");
    public static PieceShape sRTangle = new(new int[,] { { 1, 0 }, { 0, 0 }, { 0, 0 }, { 0, 3 } }, 1, 1, "Right Tangle");
    public static PieceShape sBowl = new(new int[,] { { 1, 1 }, { 2, 0 }, { 3, 3 } }, 1, 1, "Bowl");

    public static PieceShape sTrifecta = new(new int[,] { { 1, 0, 0 }, { 0, 0, 4 }, { 2, 0, 0 } }, 1, 1, "Trifecta");
    public static PieceShape sSeperatedT = new(new int[,] { { 1, 0, 0 }, { 3, 0, 4 }, { 2, 0, 0 } }, 1, 1, "temp");
    public static PieceShape sLScatteredL = new(new int[,] { { 1, 0, 4 }, { 1, 0, 0 }, { 2, 0, 0 } }, 1, 0, "temp");
    public static PieceShape sRScatteredR = new(new int[,] { { 1, 0, 0 }, { 2, 0, 0 }, { 2, 0, 4 } }, 1, 0, "temp");

}
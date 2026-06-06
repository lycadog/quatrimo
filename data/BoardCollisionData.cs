using Godot;
using System;
using System.Collections.Generic;

public class BoardCollisionData
{
    public int SlamYOffset;

    public Vector2I BoardPos;

    Vector2 RealPosition
    {
        get => RootPosition + LocalPosition;
    }
    Vector2 LocalPosition;
    Vector2 RootPosition;

    public bool LeftMoveValid;
    public bool RightMoveValid;
    public bool DownMoveValid;
    public bool UpMoveValid;

    public int CurrentRotation; //varies from 0-3, looping

    public bool Solid = true;

    public static Vector2I[] RotationOffsets = [new(0, 0), new(-1, 0), new(1, 0), new(0, 1), new(-1, -1), new(1, -1), new(-2, 0), new(0, -2)];

    public bool[] ValidLeftRotations = new bool[RotationOffsets.Length];
    public bool[] ValidRightRotations = new bool[RotationOffsets.Length];

    public void UpdateFallingBlock(Vector2 localPosition, Vector2 rootPosition)
    {
        LocalPosition = localPosition;
        RootPosition = rootPosition;

        //recalculate board positiom from our position relative to our root
        BoardPos = new((int)((RootPosition.X + LocalPosition.X) / 10), -(int)((RootPosition.Y + LocalPosition.Y) / 10));

        LeftMoveValid = BoardAccessor.IsValidMoveTo(BoardPos + new Vector2I(-1, 0), Solid);
        RightMoveValid = BoardAccessor.IsValidMoveTo(BoardPos + new Vector2I(1, 0), Solid);
        DownMoveValid = BoardAccessor.IsValidMoveTo(BoardPos + new Vector2I(0, -1), Solid);
        UpMoveValid = BoardAccessor.IsValidMoveTo(BoardPos + new Vector2I(0, 1), Solid);

        ValidLeftRotations = ProcessRotation(-1);
        ValidRightRotations = ProcessRotation(1);

        //get new slam offset
        UpdateSlamOffset();
    }

    public void UpdateFallingEnemyBlock(Vector2 blockPosition)
    {
        BoardPos = new((int)(blockPosition.X / 10), -(int)(blockPosition.Y / 10));
        DownMoveValid = BoardAccessor.IsValidMoveTo(BoardPos + new Vector2I(0, -1), Solid);

        UpdateSlamOffset();
    }

    public void UpdateSlamOffset()
    {
        SlamYOffset = 0;
        for (int y = BoardPos.Y - 1; true; y--)
        {
            Vector2I loweredPosition = new(BoardPos.X, y);

            if (!BoardAccessor.IsValidMoveTo(loweredPosition, Solid))
            {
                //if we can't move here:
                //break loop. this way we will use the previous loop's value
                break;
            }
            SlamYOffset++;
        }
    }

    bool[] ProcessRotation(int direction)
    {
        Vector2 RotatedPos = Utils.GetRotatedVector(LocalPosition, direction);
        RotatedPos += new Vector2(RootPosition.X, RootPosition.Y);

        Vector2I RotatedBoardPos = new((int)(RotatedPos.X / 10), -(int)(RotatedPos.Y / 10));

        bool[] finalValues = new bool[RotationOffsets.Length];

        for (int i = 0; i < RotationOffsets.Length; i++)
        {

            finalValues[i] = BoardAccessor.IsValidMoveTo(
                RotatedBoardPos + new Vector2I(RotationOffsets[i].X * direction, RotationOffsets[i].Y),
                Solid);
            //todo: tune the offsets more. we may need to invert them again every rotation
        }

        return finalValues;

    }



    /// <summary>
    /// Updates a piece using the collision data of its blocks. ONLY use AFTER updating blocks' collision data!
    /// </summary>
    /// <param name="blocks"></param>
    /// <param name="PiecePosition"></param>
    public void UpdatePiece(List<Block> blocks, Vector2 PiecePosition)
    {
        ResetAllValues();

        foreach(var block in blocks)
        {
            //if a single block collides somewhere, we also collide there!
            CopyOverFalseValues(block.CollisionData);

            //take the lowest y offset and use that as our own
            SlamYOffset = Math.Min(block.CollisionData.SlamYOffset, SlamYOffset);
        }

        foreach(var block in blocks)
        {
            block.CollisionData.SlamYOffset = SlamYOffset;
        }
    }


    void CopyOverFalseValues(BoardCollisionData data)
    {
        if (!data.LeftMoveValid)
        {
            LeftMoveValid = false;
        }

        if (!data.RightMoveValid)
        {
            RightMoveValid = false;
        }

        if (!data.DownMoveValid)
        {
            DownMoveValid = false;
        }

        if (!data.UpMoveValid)
        {
            UpMoveValid = false;
        }

        for(int i = 0; i < RotationOffsets.Length; i++)
        {
            if (!data.ValidLeftRotations[i])
            {
                ValidLeftRotations[i] = false;
            }

            if (!data.ValidRightRotations[i])
            {
                ValidRightRotations[i] = false;
            }

        }

    }
    void ResetAllValues()
    {
        LeftMoveValid = true;
        RightMoveValid = true;
        DownMoveValid = true;
        UpMoveValid = true;

        SlamYOffset = 1000;

        for(int i = 0; i < RotationOffsets.Length; i++)
        {
            ValidLeftRotations[i] = true;
            ValidRightRotations[i] = true;
        }
    }


    

}
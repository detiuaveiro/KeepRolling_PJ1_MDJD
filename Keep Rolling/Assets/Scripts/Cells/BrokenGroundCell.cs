using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenGroundCell : Cell
{
    public BrokenGroundCell(int x, int y, int height) : base(x, y, height, false)
    {

    }

    override
    public bool CanPlaceOnTop(PieceType pieceType)
    {
        return pieceType==PieceType.FixGround;
    }

    public override string ToString()
    {
        return $"BrokenGroundCell x:{x} y:{y}, height:{height}";
    }
}

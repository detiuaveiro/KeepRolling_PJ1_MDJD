using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceRampCell : GroundCell
{
    public PlaceRampCell(int x, int y, int height) : base(x, y, height)
    {

    }

    public override bool CanPlaceOnTop(PieceType pieceType)
    {
        return pieceType == PieceType.Ramp;
    }


    public override string ToString()
    {
        return $"PlaceRampCell x:{x} y:{y}, height:{height}";
    }
}

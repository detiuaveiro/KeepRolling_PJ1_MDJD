using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCell : Cell
{
    public GroundCell(int x, int y, int height): base(x, y, height, true)
    {
        possible_positions.Add(Tuple.Create(x + 1, y, height));
        possible_positions.Add(Tuple.Create(x - 1, y, height));
        possible_positions.Add(Tuple.Create(x, y + 1, height));
        possible_positions.Add(Tuple.Create(x, y - 1, height));
    }

    override
    public bool CanPlaceOnTop(PieceType pieceType)
    {
        return true;
    }

    public override string ToString()
    {
        return $"GroundCell x:{x} y:{y}, height:{height}";
    }
}

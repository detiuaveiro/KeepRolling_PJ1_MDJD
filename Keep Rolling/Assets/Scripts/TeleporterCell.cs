using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterCell : Cell
{
    public TeleporterCell(int x, int y, int height) : base(x, y, height, true)
    {
        //insert something
    }

    public override string ToString()
    {
        return $"TeleporterCell x:{x} y:{y}, height:{height}";
    }
}

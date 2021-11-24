using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creates Cells depending on the type of tyle it is
public class CellFactory
{
    public static Cell CreateCell(int x, int y, int height, string tileName)
    {
        switch (tileName) {

            case string tile when tile.Contains("ground"):
                return new Cell(x, y, height, true);

            case string tile when tile.Contains("ramp"):
                return new Cell(x, y, height, true);
            
            // not walkable
            default:
                return new Cell(x, y, height, false);
        }
    }
}

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
                return new GroundCell(x, y, height);

            case string tile when tile.Contains("ramp"):
                return new TeleporterCell(x, y, height);
            
            // not walkable
            default:
                return new Cell(x, y, height, false);
        }
    }
    public static Cell CreateCellFromCellName(int x, int y, int height, string cellName)
    {
        switch (cellName)
        {

            case "Ground":
                return new GroundCell(x, y, height);

            case "BrokenGround":
                return new BrokenGroundCell(x, y, height);

            case "PlaceRamp":
                return new PlaceRampCell(x, y, height);

            // not walkable
            default:
                return new Cell(x, y, height, false);
        }
    }
}

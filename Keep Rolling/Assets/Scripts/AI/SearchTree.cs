using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SearchTree 
{
    public static bool CanWalkToCell(Cell src, Cell dest)
    {
        if (!(src is null) && !(dest is null) && src.IsWalkable() && dest.IsWalkable())
        {
            if (src is GroundCell)
            {
                if (dest is GroundCell)
                {
                    if (src.getHeight() != dest.getHeight())
                    {
                        return false;
                    }
                    Vector2 srcPos = src.getVisualPosition();
                    Vector2 destPos = dest.getVisualPosition();
                    int result = (int)(Math.Abs(srcPos.x - destPos.x) + Math.Abs(srcPos.y - destPos.y));
                    return result == 1;
                }
                else if (dest is RampCell)
                {
                    Vector3 srcHeightPos = src.getVisualHeightPosition();
                    foreach (Vector3 pos in dest.getPossiblePositions())
                    {
                        if (pos == srcHeightPos)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            else if (src is RampCell)
            {
                Vector3 destHeightPos = dest.getVisualHeightPosition();
                foreach (Vector3 pos in src.getPossiblePositions())
                {
                    if (pos == destHeightPos)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        return false;
    }

    private void test() {
        Cell cell1 = new GroundCell(0, 0, 0);
        Cell cell2 = new GroundCell(0, 1, 0);
        Cell cell3 = new GroundCell(1, 1, 0);
        Cell cell4 = new GroundCell(1, 3, 1);
        Cell cell5 = new RampCell(1, 2, 1, "Up");
        Debug.Log(CanWalkToCell(cell1, cell2)); // True
        Debug.Log(CanWalkToCell(cell1, cell3)); // False
        Debug.Log(CanWalkToCell(cell1, cell4)); // False
        Debug.Log(CanWalkToCell(cell1, cell5)); // True
        Debug.Log(CanWalkToCell(cell5, cell4)); // True
    }

}

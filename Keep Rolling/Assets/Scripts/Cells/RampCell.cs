using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampCell : TeleporterCell
{
    protected string direction;
    protected List<Vector3> possible_positions;
    public RampCell(int x, int y, int height, string direction) : base(x, y, height)
    {
        this.direction = direction;
        this.possible_positions = CreatePossiblePositions(direction);
    }

    private List<Vector3> CreatePossiblePositions(string direction) {
        List<Vector3> ret = new List<Vector3>();
        switch (direction) {
            case "Up":
                ret.Add(new Vector3(visual_x, visual_y - 1, height - 1));
                ret.Add(new Vector3(visual_x, visual_y + 1, height));
                break;
            default:
                break;
        
        
        }





        return ret;
    }

    public override List<Vector3> getPossiblePositions()
    {
        return possible_positions;
    }

    public override string ToString()
    {
        return $"RampCell x:{x} y:{y}, height:{height}";
    }
}

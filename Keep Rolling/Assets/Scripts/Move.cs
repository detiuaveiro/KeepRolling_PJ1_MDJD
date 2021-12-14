using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Move : Command
{
    public Cell destination;
    public Transform transform;

    public Move(Cell destCell, Transform trans)
    {
        this.destination = destCell;
        this.transform = trans;
    }
    public override void Execute()
    {
        if (this.destination != null)
        {
            Vector3 coords = MapManager.instance.tilemaps[destination.getHeight()].GetCellCenterWorld(new Vector3Int(destination.getX(), destination.getY(), 0));
            ChairMovementController.instance.nextPosition = coords;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Move : Command
{
    public Cell destination;

    public Move(Cell destCell)
    {
        this.destination = destCell;
    }
    public override void Execute()
    {
        if (this.destination != null)
        {
            Vector3 coords = MapManager.instance.tilemaps[destination.getHeight()].GetCellCenterWorld(new Vector3Int(destination.getVisualX(), destination.getVisualY(), 0));
            ChairMovementController.instance.nextPosition = coords;
            ChairMovementController.instance.heightLevel = destination.getHeight();
            ChairMovementController.instance.cellDestination = this.destination;
        }
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }
}

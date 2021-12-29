using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int x = -1;
    public int y = -1;

    // Start is called before the first frame update
    void Start()
    {
        ChairMovementController.instance.commandQueue.Enqueue(new Move(MapManager.instance.cell_matrix.GetCell(x,y)));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        float off = Random.Range(0f, 1f);
        int new_x = x;
        int new_y = y;
        if (off < 0.25)
        {
            new_x -= 1;
        }
        else if (off < 0.50)
        {
            new_x += 1;
        }
        else if (off < 0.75)
        {
            new_y -= 1;
        }
        else
        {
            new_y += 1;
        }

        Cell cell = MapManager.instance.cell_matrix.GetCell(new_x, new_y);
        if (!(cell is null) && cell.getHeight() < 1)
        {
            ChairMovementController.instance.commandQueue.Enqueue(new Move(cell));
            x = new_x;
            y = new_y;
        }
        */
    }
}

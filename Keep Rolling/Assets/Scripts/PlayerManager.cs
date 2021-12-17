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
        float x_off = Random.Range(0f, 1f);
        Debug.Log(x_off);
        int new_x = x;
        if (x_off < 0.40)
        {
            new_x -= 1;
        }
        else if (x_off < 0.80)
        {
            new_x += 1;
        }

        float y_off = Random.Range(0f, 1f);
        int new_y = y;
        if (y_off < 0.40)
        {
            new_y -= 1;
        }
        else if (y_off < 0.80)
        {
            new_y += 1;
        }

        Cell cell = MapManager.instance.cell_matrix.GetCell(new_x, new_y);
        Debug.Log(new_x);
        Debug.Log(new_y);
        Debug.Log(cell);
        if (!(cell is null) && cell.getHeight() < 1)
        {
            ChairMovementController.instance.commandQueue.Enqueue(new Move(cell));
            x = new_x;
            y = new_y;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleSelected : MonoBehaviour
{
    public Tile tile;
    public Grid grid;
    private List<Cell> cells;
    private void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        cells = MapManager.instance.cells;
        Debug.Log("Got " + cells.Count);
    }
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);

        foreach (var cell in cells)
        {
            if (Vector2.Distance(cell.getPosition(), transform.position) < 0.5)
            {
                transform.position = cell.getPosition();
                Debug.Log("Snapping to: " + cell);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);
        }
    }
}

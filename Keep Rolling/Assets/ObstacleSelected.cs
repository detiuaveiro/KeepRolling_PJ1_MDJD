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
            Vector3Int cellPos = new Vector3Int((int)cell.getPosition().x,(int)cell.getPosition().y,0);
            Vector3 place = MapManager.instance.tilemaps[cell.getHeight()].GetCellCenterWorld(cellPos);
            if (Vector3.Distance(place, transform.position) < 0.25)
            {
                transform.position = place;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);
        }
    }
}

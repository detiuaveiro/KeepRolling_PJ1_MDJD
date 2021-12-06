using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleSelected : MonoBehaviour
{
    public Tile tile;
    public Grid grid;
    private List<Cell> cells;
    private Cell lastSnappedCell;
    public Piece piece;
    private void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        cells = MapManager.instance.cell_matrix.GetAllCells();
    }
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);

        lastSnappedCell = null;

        foreach (var cell in cells)
        {
            Vector3Int cellPos = new Vector3Int((int)cell.getPosition().x,(int)cell.getPosition().y,0);
            Vector3 place = MapManager.instance.tilemaps[cell.getHeight()].GetCellCenterWorld(cellPos);
            if (Vector3.Distance(place, transform.position) < 0.25)
            {
                transform.position = place;
                lastSnappedCell = cell;
                //Debug.Log(cell);
            }
        }

        if (lastSnappedCell != null)
        {
            if (MapManager.instance.CanPlaceTile(lastSnappedCell, piece))
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.2f, 1, 0.2f, 0.75f);
            } else
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.2f, 0.2f, 0.75f);
            }
            if (Input.GetButton("Fire1"))
            {
                if (MapManager.instance.PlaceTile(lastSnappedCell, piece)) {
                    Debug.Log("placing");
                    Destroy(this.gameObject);
                }
            }
        } else
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.75f);
        }
        cells = MapManager.instance.cell_matrix.GetAllCells();
    }
}

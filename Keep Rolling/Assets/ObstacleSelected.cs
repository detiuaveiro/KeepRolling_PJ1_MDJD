using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleSelected : MonoBehaviour
{
    public Tile tile;
    public Grid grid;
    private void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
    }
    void Update()
    {
        transform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);
            Debug.Log(coordinate);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager
{
    private static MapManager instance = null;
    public List<Tilemap> tilemaps;
    public List<Cell> cells;
    private MapManager(List<Tilemap> tilemaps) {
        this.tilemaps = tilemaps;
        CreateCellGrid();
    }

    public static MapManager GetInstance() {
        if (instance == null) {
            //maybe error
            return null;
        }
        return instance;
    }
    public static void CreateInstance(List<Tilemap> tilemaps) {
        instance = new MapManager(tilemaps);
    }

    private void CreateCellGrid() {
        this.cells = new List<Cell>();
        for (int height = 0; height < tilemaps.Count; height++)
        {
            var tilemap = tilemaps[height];
            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                //useful function
                //Vector3 place = tilemap.CellToWorld(localPlace);
                if (tilemap.HasTile(localPlace))
                {
                    TileBase tilebase = tilemap.GetTile(localPlace);
                    Cell cell = CellFactory.CreateCell(pos.x, pos.y, height, tilebase.name);
                    cells.Add(cell);
                }
            }
        }
        /*
        foreach (var cell in cells)
            Debug.Log(cell);
        */
    }
}

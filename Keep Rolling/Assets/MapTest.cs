using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapTest : MonoBehaviour
{
    public static MapTest instance;
    public List<Tilemap> tilemaps;
    public List<Cell> cells;
    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        /*Tilemap tilemap = GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        Debug.Log( bounds.size);
        Debug.Log(allTiles[0]);
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                }
                else
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }*/

        cells = new List<Cell>();
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
        foreach (var cell in cells)
            Debug.Log(cell);



    }
}

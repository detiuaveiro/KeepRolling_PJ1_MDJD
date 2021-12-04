using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
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
        CreateCellGrid();

    }
    private void CreateCellGrid()
    {
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
        //CanPlaceTile(cells[0], null);
        /*
        foreach (var cell in cells)
            Debug.Log(cell);
        */
    }

    private bool CanPlaceTileHere(int x, int y, int height)
    {
        foreach (var cell in cells)
        {
            if (cell.getX() == x && cell.getY() == y && cell.getHeight() == height)
            {
                return false;
            }
        }
        return true;
    }

    public bool CanPlaceTile(Cell cell, Piece piece) {

        if (!cell.CanPlaceOnTop(piece.type))
            return false;

        //TODO: keep adding PieceTypes
        switch (piece.type) {
            case PieceType.Ramp:
                return CanPlaceTileHere(cell.getX(), cell.getY(), cell.getHeight()+1);
            default:
                return false;
        }
    }

    public bool PlaceTile(Cell cell, Piece piece) {
        
        if (!CanPlaceTile(cell, piece))
            return false;

        //TODO: keep adding PieceTypes
        switch (piece.type)
        {
            case PieceType.Ramp:
                tilemaps[cell.getHeight() + 1].SetTile(new Vector3Int(cell.getX(),cell.getY(),0), piece.tile);
                cells.Add(new TeleporterCell(cell.getX(), cell.getY(),cell.getHeight()));
                break;
            default:
                return false;
        }
        return true;
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    public List<Tilemap> tilemaps;
    //public List<Cell> cells;
    public CellMatrix cell_matrix;
    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        instance = this;

        CreateCellGrid();

    }
    private void CreateCellGrid()
    {
        var cell_list = new List<Cell>();
        int min_y = 1000, min_x = 1000, max_y = -1000, max_x = -1000;

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
                    cell_list.Add(cell);
                    min_x = Mathf.Min(min_x, pos.x);
                    min_y = Mathf.Min(min_y, pos.y);
                    max_x = Mathf.Max(max_x, pos.x);
                    max_y = Mathf.Max(max_y, pos.y);
                }
            }
        }

        cell_matrix = new CellMatrix(min_x, min_y, max_x, max_y);
        Debug.Log(min_x);
        Debug.Log(min_y);
        Debug.Log(max_x);
        Debug.Log(max_y);

        //CanPlaceTile(cells[0], null);

        foreach (Cell cell in cell_list)
        {
            cell_matrix.AddCell(cell);
            //Debug.Log(cell);
        }
        
    }

    private bool CanPlaceTileHere(int x, int y, int height)
    {
        return (!cell_matrix.HasCell(x, y) || cell_matrix.GetCell(x, y).getHeight() < height);

        /*
        foreach (var cell in cells)
        {
            if (cell.getX() == x + height && cell.getY() == y + height && cell.getHeight() == height)
            {
                return false;
            }
        }
        
        return true;
        */
    }

    public bool CanPlaceTile(Cell cell, Piece piece, int height) {

        if (!cell.CanPlaceOnTop(piece.type))
            return false;

        //TODO: keep adding PieceTypes
        switch (piece.type) {
            case PieceType.Ramp:
                return CanPlaceTileHere(cell.getVisualX(), cell.getVisualY(), cell.getHeight()+height);
            default:
                return false;
        }
    }

    public bool PlaceTile(Cell cell, Piece piece, int height) {
        
        if (!CanPlaceTile(cell, piece, height))
            return false;

        //TODO: keep adding PieceTypes
        switch (piece.type)
        {
            case PieceType.Ramp:
                tilemaps[cell.getHeight() + height].SetTile(new Vector3Int(cell.getX()+height,cell.getY()+height,0), piece.tile);
                var new_cell = new TeleporterCell(cell.getX()+height, cell.getY()+height, cell.getHeight()+height);
                cell_matrix.AddCell(new_cell);
                break;
            default:
                return false;
        }
        return true;
    }

    public Tuple<Cell, int> GetLastSnappedCell(Vector3 mousePos, Piece piece) {

        int offset = 0;
        Vector3Int cellPos1 = tilemaps[0].WorldToCell(mousePos);
        Cell lastSnappedCell = cell_matrix.GetCell(cellPos1.x, cellPos1.y);

        //Debug.Log(lastSnappedCell);
        if (lastSnappedCell is null || !CanPlaceTile(lastSnappedCell, piece, offset)) {
            offset++;
            lastSnappedCell = null;
            while (offset  < tilemaps.Count)
            {
                Debug.Log("entrou");
                cellPos1.x -= 1;
                cellPos1.y -= 1;

                Cell temp = cell_matrix.GetCell(cellPos1.x, cellPos1.y);
                if (!(temp is null))
                {
                    if (CanPlaceTile(temp, piece, offset))
                    {
                        lastSnappedCell = temp;
                        break;
                    }
                    /*
                    if (!temp.CanPlaceOnTop(piece.type))
                    {
                        break;
                    }
                    */
                }
                //Debug.Log(lastSnappedCell);
                offset++;
            }
            Debug.Log("saiu");
        }
        return Tuple.Create(lastSnappedCell, offset);
    }



}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.Events;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    public List<Tilemap> tilemaps;
    //public List<Cell> cells;
    public CellMatrix cell_matrix;

    public Vector3Int startPosition;
    public Vector3Int endPosition;
    public bool solve = false;

    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        LoadScene();
    }

    private void LoadScene()
    {
        List<Cell> cellList = MapLoader.loadLevel(GameManager.instance.GetCurrentLevel(),tilemaps);
        CreateCellGridFromList(cellList);
        ChairMovementController.instance.transform.position = tilemaps[startPosition.z].CellToWorld(startPosition);
        Cell endCell = cell_matrix.GetCell(endPosition.x, endPosition.y);
        tilemaps[endPosition.z].SetColor(new Vector3Int(endCell.getVisualX(),endCell.getVisualY(),0), new Color(1.0f,0f,0f,1));
        JumpingPin.instance.ground = tilemaps[startPosition.z].CellToWorld(endPosition);
        JumpingPin.instance.transform.position = tilemaps[startPosition.z].CellToWorld(endPosition);
        JumpingPin.instance.velocity = 0;
    }

    void Update()
    {
        if (solve) {
            solve = false;
            StartCoroutine(SolveLevel());
        }
    }

    private IEnumerator SolveLevel() {
        Debug.Log(startPosition);
        Debug.Log(endPosition);
        //startPosition = new Vector3Int(2, 6, 0);
        //endPosition = new Vector3Int(2, 6, 0);
        /*
        foreach (Cell cell in cell_matrix.GetAllCells())
        {
            Debug.Log($"blablabla {cell}");
        }*/
        SearchTree searchTree = new SearchTree(startPosition, endPosition, cell_matrix);
        StartCoroutine(searchTree.search());
        while (true) {
            if (!(searchTree.solution is null)) {
                break;
            }
            yield return null;
        }
        Debug.Log(searchTree.solution);
        Debug.Log(searchTree.nodes_explored);
        Debug.Log(searchTree.open_nodes.Count);

        foreach (Command move in searchTree.GetCommandSolution())
        {
            Debug.Log(((Move)move).destination);
            ChairMovementController.instance.commandQueue.Enqueue((Move)move);
        }
        yield return null;

    }


    private void CreateCellGridFromList(List<Cell> cellList) 
    {
        int min_y = 1000, min_x = 1000, max_y = -1000, max_x = -1000;

        foreach(Cell cell in cellList)
        {
            min_x = Mathf.Min(min_x, cell.getVisualX());
            min_y = Mathf.Min(min_y, cell.getVisualY());
            max_x = Mathf.Max(max_x, cell.getVisualX());
            max_y = Mathf.Max(max_y, cell.getVisualY());
        }

        cell_matrix = new CellMatrix(min_x, min_y, max_x, max_y);
        Debug.Log(min_x);
        Debug.Log(min_y);
        Debug.Log(max_x);
        Debug.Log(max_y);

        foreach(Cell cell in cellList)
        {
            cell_matrix.AddCell(cell);
            Debug.Log(cell);
        }



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
        {
            return false;
        }

        //TODO: keep adding PieceTypes
        switch (piece.type) {
            case PieceType.Ramp:
                return CanPlaceTileHere(cell.getVisualX(), cell.getVisualY(), cell.getHeight()+1+height);
            case PieceType.FixGround:
                return true;
            default:
                return false;
        }
    }

    public bool PlaceTile(Cell cell, Sprite sprite, int height, Piece piece, Direction direction) {
        
        if (!CanPlaceTile(cell, piece, height) || !LevelManager.instance.BuyPiece(piece))
            return false;

        //TODO: keep adding PieceTypes
        IsometricRuleTile newTile = new IsometricRuleTile();
        Cell new_cell = null;
        switch (piece.type)
        {
            case PieceType.Ramp:
                newTile.m_DefaultSprite = sprite;
                tilemaps[cell.getHeight() + height + 1].SetTile(new Vector3Int(cell.getX()+height,cell.getY()+height,0), newTile);
                new_cell = new RampCell(cell.getX()+1+height, cell.getY()+1+height, cell.getHeight()+height + 1,direction);
                cell_matrix.AddCell(new_cell);
                break;
            case PieceType.FixGround:
                newTile = piece.tile;
                tilemaps[cell.getHeight() + height].SetTile(new Vector3Int(cell.getVisualX() + height, cell.getVisualY() + height, 0), newTile);
                new_cell = new GroundCell(cell.getX() + height, cell.getY() + height, cell.getHeight() + height);
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
        /*
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
                    *//*
                    if (!temp.CanPlaceOnTop(piece.type))
                    {
                        break;
                    }
                    */
                /*}
                //Debug.Log(lastSnappedCell);
                offset++;
            }
            Debug.Log("saiu");
        }*/
        return Tuple.Create(lastSnappedCell, offset);
    }



}

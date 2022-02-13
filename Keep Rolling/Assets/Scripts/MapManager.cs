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
    public GameObject arrival_pin_prefab;
    public GameObject start_pin_prefab;

    public Vector3Int startPosition;
    public Vector3Int endPosition;
    public bool solve = false;

    private List<PlaceObject> placeObjectLog;
    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        LoadScene();
        placeObjectLog = new List<PlaceObject>();
    }

    private void LoadScene()
    {
        List<Cell> cellList = MapLoader.loadLevel(GameManager.instance.GetCurrentLevel(),tilemaps);
        CreateCellGridFromList(cellList);
        ChairMovementController.instance.transform.position = tilemaps[startPosition.z].CellToWorld(startPosition);
        Cell endCell = cell_matrix.GetCell(endPosition.x, endPosition.y);
        //tilemaps[endPosition.z].SetColor(new Vector3Int(endCell.getVisualX(),endCell.getVisualY(),0), new Color(1.0f,0f,0f,1));
        var obj = Instantiate(arrival_pin_prefab, transform);
        var pos = tilemaps[endPosition.z].GetCellCenterWorld(new Vector3Int(endCell.getVisualX(), endCell.getVisualY(), 0));
        obj.transform.position = pos;
    }

    void Update()
    {
        if (solve) {
            solve = false;
            StartCoroutine(SolveLevel());
        }
    }

    public void UndoObjectPlace()
    {
        if (placeObjectLog.Count > 0)
        {
            PlaceObject command = placeObjectLog[placeObjectLog.Count - 1];
            placeObjectLog.Remove(command);
            command.Undo();
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

    private bool CanPlaceTileHere(int x, int y, int height)
    {
        return (!cell_matrix.HasCell(x, y) || cell_matrix.GetCell(x, y).getHeight() < height);
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
        switch (piece.type)
        {
            case PieceType.Ramp:
                PlaceObject command = new PlaceObject(sprite,cell,height,direction, piece);
                command.Execute();
                placeObjectLog.Add(command);
                break;
            case PieceType.FixGround:
                PlaceObject command2 = new PlaceObject(sprite, cell, height, direction, piece);
                command2.Execute();
                placeObjectLog.Add(command2);
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

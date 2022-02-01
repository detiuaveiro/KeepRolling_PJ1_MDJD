using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairMovementController : MonoBehaviour
{
    public static ChairMovementController instance;

    private List<Command> commandLog;
    public Queue<Command> commandQueue;

    public SpriteRenderer ChairSprite;

    private bool moving;
    public Vector3 nextPosition;
    public Cell cellDestination;
    private Cell currentCell;
    private List<Cell> cellsToRestoreTransparency;
    private bool level_ended;
    private float speed=1; 

    public int heightLevel;

    public Animator animator;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        commandLog = new List<Command>();
        commandQueue = new Queue<Command>();
        moving = false;
        level_ended = false;
        cellsToRestoreTransparency = new List<Cell>();
    }

    public void RestartMovement() {
        commandLog = new List<Command>();
        commandQueue = new Queue<Command>();
        moving = false;
        level_ended = false;
        cellsToRestoreTransparency = new List<Cell>();
        changeRenderingLayer();
    }

    void changeRenderingLayer()
    {
        switch (heightLevel)
        { 
            case 0:
                this.ChairSprite.sortingLayerName = "AgentOnGround";
                break;
            case 1:
                this.ChairSprite.sortingLayerName = "AgentLevel1";
                break;
            default:
                break;
        }
    }

    void applyTransparencyToCells()
    {
        if (cellsToRestoreTransparency.Count > 0)
        {
            for (int i = 0; i < cellsToRestoreTransparency.Count; i++)
            {
                Color color2 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                MapManager.instance.tilemaps[cellsToRestoreTransparency[i].getHeight()].SetColor(new Vector3Int(cellsToRestoreTransparency[i].getX(), cellsToRestoreTransparency[i].getY(), 0), color2);
                
            }
            cellsToRestoreTransparency = new List<Cell>();
        }
        Cell cellY = MapManager.instance.cell_matrix.GetCell(cellDestination.getX(), cellDestination.getY() - 1);
        if (cellY != null && cellY.getHeight() > cellDestination.getHeight())
        {
            Color color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
            cellsToRestoreTransparency.Add(cellY);
            MapManager.instance.tilemaps[cellY.getHeight()].SetColor(new Vector3Int(cellY.getX(), cellY.getY(), 0), color);
            Cell cell2 = MapManager.instance.cell_matrix.GetCell(cellDestination.getX() + 1, cellDestination.getY() - 1);
            Cell cell3 = MapManager.instance.cell_matrix.GetCell(cellDestination.getX() - 1, cellDestination.getY() - 1);
            if (cell2 != null && cell2.getHeight() > cellDestination.getHeight())
            {
                MapManager.instance.tilemaps[cell2.getHeight()].SetColor(new Vector3Int(cell2.getX(), cell2.getY(), 0), color);
                cellsToRestoreTransparency.Add(cell2);
            }
            if (cell3 != null && cell3.getHeight() > cellDestination.getHeight())
            {
                MapManager.instance.tilemaps[cell3.getHeight()].SetColor(new Vector3Int(cell3.getX(), cell3.getY(), 0), color);
                cellsToRestoreTransparency.Add(cell3);
            }
        }
        Cell cellX = MapManager.instance.cell_matrix.GetCell(cellDestination.getX() - 1, cellDestination.getY());
        if (cellX != null && cellX.getHeight() > cellDestination.getHeight())
        {
            Color color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
            cellsToRestoreTransparency.Add(cellX);
            MapManager.instance.tilemaps[cellX.getHeight()].SetColor(new Vector3Int(cellX.getX(), cellX.getY(), 0), color);
            Cell cell2 = MapManager.instance.cell_matrix.GetCell(cellDestination.getX() - 1, cellDestination.getY() - 1);
            Cell cell3 = MapManager.instance.cell_matrix.GetCell(cellDestination.getX() - 1, cellDestination.getY() + 1);
            if (cell2 != null && cell2.getHeight() > cellDestination.getHeight())
            {
                MapManager.instance.tilemaps[cell2.getHeight()].SetColor(new Vector3Int(cell2.getX(), cell2.getY(), 0), color);
                cellsToRestoreTransparency.Add(cell2);
            }
            if (cell3 != null && cell3.getHeight() > cellDestination.getHeight())
            {
                MapManager.instance.tilemaps[cell3.getHeight()].SetColor(new Vector3Int(cell3.getX(), cell3.getY(), 0), color);
                cellsToRestoreTransparency.Add(cell3);
            }
        }
        Cell cellXY = MapManager.instance.cell_matrix.GetCell(cellDestination.getX() - 1, cellDestination.getY()-1);
        if (cellXY != null && cellXY.getHeight() > cellDestination.getHeight())
        {
            Color color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
            cellsToRestoreTransparency.Add(cellXY);
            MapManager.instance.tilemaps[cellXY.getHeight()].SetColor(new Vector3Int(cellXY.getX(), cellXY.getY(), 0), color);
        }
    }
            // Update is called once per frame
            
    void Update()
    {
        if (!moving && commandQueue.Count > 0)
        {
            Command command = commandQueue.Dequeue();
            commandLog.Add(command);
            command.Execute();

            changeRenderingLayer();
            applyTransparencyToCells();
            animator.SetBool("Idle", false);

            Cell cell;
            if (currentCell != null)
            {
                cell = currentCell;
            } else
            {
                Vector3Int cellPos = MapManager.instance.tilemaps[heightLevel].WorldToCell(transform.position);
                cell = MapManager.instance.cell_matrix.GetCell(cellPos.x, cellPos.y);
            }
            if (cell.getHeight() == cellDestination.getHeight())
            {
                if (cell.getX() - cellDestination.getX() > 0)
                {
                    animator.SetBool("GoinUp", false);
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else if (cell.getX() - cellDestination.getX() < 0)
                {
                    animator.SetBool("GoinUp", true);
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
                }
                else if (cell.getY() - cellDestination.getY() > 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
                    animator.SetBool("GoinUp", false);
                }
                else
                {
                    animator.SetBool("GoinUp", true);
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }

            moving = true;
        }
        //No more movement
        else if (!moving && commandQueue.Count == 0 && commandLog.Count > 0 && !level_ended)
        {
            LevelManager.instance.OnMovementComplete();
            level_ended =true;
        }

        if (moving && nextPosition != null)
        {
            if (Vector3.Distance(this.transform.position, new Vector2(nextPosition.x, nextPosition.y)) > 0.001f)
            {
                this.transform.position = Vector2.MoveTowards(transform.position, new Vector2(nextPosition.x, nextPosition.y), speed * Time.deltaTime);
            } else
            {
                currentCell = cellDestination;
                moving = false;
                animator.SetBool("Idle", true);
            }
        }
        
    }

    public void SetSpeed(float value) {
        speed = value;
    }
}

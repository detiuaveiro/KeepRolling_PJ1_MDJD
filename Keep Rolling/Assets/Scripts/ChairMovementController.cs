using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairMovementController : MonoBehaviour
{
    public static ChairMovementController instance;

    private List<Command> commandLog = new List<Command>();
    public Queue<Command> commandQueue = new Queue<Command>();

    public SpriteRenderer ChairSprite;

    private bool moving = false;
    public Vector3 nextPosition;
    public int heightLevel;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }

    void Start()
    {
        //StartCoroutine(ExampleCoroutine());
    }
    // Example
    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(2);
        Move move = new(MapManager.instance.cell_matrix.GetCell(-1, -2));
        commandQueue.Enqueue(move);
        Move move2 = new(MapManager.instance.cell_matrix.GetCell(-1, -3));
        commandQueue.Enqueue(move2);
        Move move3 = new(MapManager.instance.cell_matrix.GetCell(-2, -3));
        commandQueue.Enqueue(move3);
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

    // Update is called once per frame
    void Update()
    {
        if (!moving && commandQueue.Count > 0)
        {
            Command command = commandQueue.Dequeue();
            commandLog.Add(command);
            command.Execute();

            changeRenderingLayer();
    
            moving = true;
        }

        if (moving && nextPosition != null)
        {
            if (Vector3.Distance(this.transform.position,nextPosition) > 0.001f)
            {
                this.transform.position = Vector2.MoveTowards(transform.position, new Vector2(nextPosition.x, nextPosition.y), 1.0f * Time.deltaTime);
            } else
            {
                moving = false;
            }
        }
    }
}

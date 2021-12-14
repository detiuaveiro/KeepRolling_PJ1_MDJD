using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairMovementController : MonoBehaviour
{
    public static ChairMovementController instance;

    private List<Command> commandLog = new List<Command>();
    private Queue<Command> commandQueue = new Queue<Command>();

    private bool moving = false;
    public Vector3 nextPosition;
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
        StartCoroutine(ExampleCoroutine());
    }
    // Example
    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(2);
        Move move = new(MapManager.instance.cell_matrix.GetCell(-1, -2), this.gameObject.transform);
        commandQueue.Enqueue(move);
        Move move2 = new(MapManager.instance.cell_matrix.GetCell(-1, -3), this.gameObject.transform);
        commandQueue.Enqueue(move2);
        Move move3 = new(MapManager.instance.cell_matrix.GetCell(-2, -3), this.gameObject.transform);
        commandQueue.Enqueue(move3);
    }
    // Update is called once per frame
    void Update()
    {
        if (!moving && commandQueue.Count > 0)
        {
            Command command = commandQueue.Dequeue();
            commandLog.Add(command);
            command.Execute();
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

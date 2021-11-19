using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedMovement : MonoBehaviour
{
    public Transform destination;
    public Transform midPoint;
    private bool canGo = false;
    private bool atMidPoint = false;
    void Start()
    {
        StartCoroutine(ExampleCoroutine());
    }
    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(2);
        canGo = true;
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
    // Update is called once per frame
    void Update()
    {
        if (canGo)
        {
            if (atMidPoint)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(destination.position.x, destination.position.y), 1.0f * Time.deltaTime);
            } else
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(midPoint.position.x, midPoint.position.y), 1.0f * Time.deltaTime);
                if (Vector2.Distance(transform.position , midPoint.position) < 0.5)
                {
                    atMidPoint = true;
                }
            }
        }
    }
}

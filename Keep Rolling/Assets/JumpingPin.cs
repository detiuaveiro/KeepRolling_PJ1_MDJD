using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPin : MonoBehaviour
{
    public static JumpingPin instance;
    public Vector3 ground;
    public float velocity = 0;
    public float gravity=1;
    public float velocity_jump = 10;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }
        ground = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(ground, transform.position) < 0.7f && velocity <= 0)
        {
            velocity = velocity_jump;
        }
        else {
            velocity -= gravity*Time.deltaTime;
        }
        transform.position = new Vector3(transform.position.x,transform.position.y + velocity*Time.deltaTime, transform.position.z);
    }

}

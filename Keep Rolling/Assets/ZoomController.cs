using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomController : MonoBehaviour
{
    Camera cameraC;
    // Start is called before the first frame update
    void Start()
    {
        cameraC = this.gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraC.orthographicSize -= Input.GetAxis("Mouse ScrollWheel");
    }
}

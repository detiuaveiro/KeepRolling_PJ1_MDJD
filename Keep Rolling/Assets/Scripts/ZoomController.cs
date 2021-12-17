using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomController : MonoBehaviour
{
    public float maxSize;
    public float minSize;
    Camera cameraC;
    // Start is called before the first frame update
    void Start()
    {
        cameraC = this.gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float newSize = cameraC.orthographicSize - Input.GetAxis("Mouse ScrollWheel");
        if (newSize > minSize && newSize < maxSize)
        {
            cameraC.orthographicSize = newSize;
        }
    }
}

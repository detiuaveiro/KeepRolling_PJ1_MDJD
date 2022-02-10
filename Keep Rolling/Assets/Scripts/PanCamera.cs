using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCamera : MonoBehaviour
{
    private Vector3 central_position;
    private Vector3 lastPosition;
    public float mouseSensitivity;
    public float maxDistanceX;    
    public float maxDistanceY;

    void Start()
    {
        central_position = transform.position;
    }


    void Update()
    {
        
        if (Input.GetMouseButtonDown(1))
        {
            lastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Confined;
            var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastPosition;
            Vector3 tr = transform.position;
            tr += new Vector3(-delta.x * mouseSensitivity, -delta.y * mouseSensitivity, 0);
            if (Mathf.Abs(tr.x - central_position.x) < maxDistanceX && Mathf.Abs(tr.y - central_position.y) < maxDistanceY)
            {
                Debug.Log(Vector3.Distance(tr, central_position));
                transform.Translate(-delta.x * mouseSensitivity, -delta.y * mouseSensitivity, 0);
            }
            lastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

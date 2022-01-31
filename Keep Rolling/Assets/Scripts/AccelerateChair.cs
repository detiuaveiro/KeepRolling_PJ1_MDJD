using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccelerateChair : MonoBehaviour
{
    public float[] speed_list;
    public Text text;
    private int index;

    private void Start()
    {
        index = 0;
        text.text = $"{speed_list[index]}x";
    }

    public void Accelerate() {
        index = (index + 1) % speed_list.Length;
        text.text = $"{speed_list[index]}x";
        ChairMovementController.instance.SetSpeed(speed_list[index]);
    }
}

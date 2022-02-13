using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryTellerManager : MonoBehaviour
{
    public Text textBox;
    public string[] textLines;
    private int currentTextLine;

    public static StoryTellerManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }
    public void DisplayStoryTeller()
    {
        currentTextLine = 0;
        textBox.text = textLines[currentTextLine];
        this.gameObject.SetActive(true);
    }

    public void NextTextLine()
    {
        currentTextLine++;
        if (textLines.Length > currentTextLine)
        {
            textBox.text = textLines[currentTextLine];
        } else
        {
            this.gameObject.SetActive(false);
        }
    }
}

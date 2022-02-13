using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryTellerManager : MonoBehaviour
{
    public Text textBox;
    public string[] textLines;
    private int currentTextLine;
    public Image image;

    public Sprite[] rapazSprites;
    public Sprite[] raparigaSprites;
    public Sprite[] nbSprites;

    private Sprite[] spritesToImage;

    public static StoryTellerManager instance;

    private float frameRate = 0.2f;
    private float timer;
    private int currentFrame;

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

        switch (GameManager.instance.selectedPerson)
        {
            case "rapaz":
                {
                    spritesToImage = rapazSprites;
                    break;
                }
            case "rapariga":
                {
                    spritesToImage = raparigaSprites;
                    break;
                }
            case "nb":
                {
                    spritesToImage = nbSprites;
                    break;
                }
        }
        image.sprite = spritesToImage[2];
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
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % spritesToImage.Length;
            image.sprite = spritesToImage[currentFrame];
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLooper : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite[] sprites;
    private float frameRate = 0.1f;
    private float timer;
    private int currentFrame;
    // Start is called before the first frame update
    void Start()
    {
        sr = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % sprites.Length;
            sr.sprite = sprites[currentFrame];
        }
    }
}

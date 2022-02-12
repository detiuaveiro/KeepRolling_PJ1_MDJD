using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionManager : MonoBehaviour
{
    public static ReactionManager instance;
    public SpriteRenderer image;
    public ReactionEnum nextReaction;
    public List<Sprite> spriteList;
    public float timeLeft = -1;

    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        image = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (timeLeft != -1) {
            if (spriteList[(int)nextReaction] != image.sprite) {
                image.sprite = spriteList[(int)nextReaction];
            }
            timeLeft -= Time.deltaTime;
        }
        if (timeLeft <= 0 && timeLeft != -1) {
            timeLeft = -1;
            image.sprite = null;
        }
    }
}

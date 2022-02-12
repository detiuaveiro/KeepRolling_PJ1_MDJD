using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSelect : MonoBehaviour
{

    public static VisualSelect instance;
    public Sprite selectedSprite;
    public Sprite notSelectedSprite;

    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        //load character select
    }

    public void ChangeCharacter()
    {
    
    }
    public void ChangeGender()
    {

    }
}

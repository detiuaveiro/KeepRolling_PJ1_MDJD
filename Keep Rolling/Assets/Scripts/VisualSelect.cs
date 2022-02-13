using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualSelect : MonoBehaviour
{

    public static VisualSelect instance;
    public Sprite selectedSprite;
    public Sprite notSelectedSprite;
    public string character_selected;
    public string chair_selected;
    public Dictionary<string,Sprite> sprites;
    public Image image;

    [System.Serializable]
    public class AppearanceData {
        public List<string> all;  
    }


    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        sprites = new Dictionary<string, Sprite>();
        TextAsset jsonFile = Resources.Load<TextAsset>("Visual/appearance");
        AppearanceData appearanceData = JsonUtility.FromJson<AppearanceData>(jsonFile.text);
        foreach (string spriteName in appearanceData.all) {
            Sprite sprite = Resources.Load<Sprite>($"Visual/{spriteName}");
            sprites.Add(spriteName, sprite);
            Debug.Log(spriteName);
        }
        foreach (string s in sprites.Keys) {
            Debug.Log(s);
        }
        character_selected = "rapaz";
        chair_selected = "azul";
        image.sprite = sprites[$"{character_selected}-{chair_selected}"];
        //load character select
    }

    public void ChangeCharacter(string id)
    {
        character_selected = id;
        image.sprite = sprites[$"{character_selected}-{chair_selected}"];
    }
    public void ChangeChair(string id)
    {
        chair_selected = id;
        image.sprite = sprites[$"{character_selected}-{chair_selected}"];
    }
}

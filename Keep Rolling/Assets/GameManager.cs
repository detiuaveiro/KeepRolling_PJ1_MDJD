using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentLevel;
    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("destroying game manage");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    public void StartLevel(int level)
    {
        currentLevel = level;
        SceneManager.LoadScene("InGame");
    }
}

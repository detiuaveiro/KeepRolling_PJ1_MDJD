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
            Destroy(this.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
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

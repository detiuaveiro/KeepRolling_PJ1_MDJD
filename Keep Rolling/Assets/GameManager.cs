using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentLevel;

    public Dictionary<string, float> levelHighScores;
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

    void Start()
    {
        levelHighScores = new();
        SaveManager.Save save = SaveManager.LoadSaveGame();
        levelHighScores = save.scores;
    }

    public void SetScoreForCurrentLevel(float score)
    {
        if (levelHighScores.ContainsKey("Level" + currentLevel))
        {
            if (levelHighScores["Level" + currentLevel] < score)
            {
                levelHighScores["Level" + currentLevel] = score;
                SaveManager.SaveGame("s", "s", levelHighScores);
            }
        } else
        {
            levelHighScores.Add("Level" + currentLevel, score);
            SaveManager.SaveGame("s", "s", levelHighScores);
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

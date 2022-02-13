using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentLevel;

    public string selectedChair;
    public string selectedPerson;
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
        selectedChair = save.selectedChair;
        selectedPerson = save.selectedPerson;
    }

    public void SetScoreForCurrentLevel(float score)
    {
        if (levelHighScores.ContainsKey("Level" + currentLevel))
        {
            if (levelHighScores["Level" + currentLevel] < score)
            {
                levelHighScores["Level" + currentLevel] = score;
                SaveManager.SaveGame(selectedChair, selectedPerson, levelHighScores);
            }
        } else
        {
            levelHighScores.Add("Level" + currentLevel, score);
            SaveManager.SaveGame(selectedChair, selectedPerson, levelHighScores);
        }
    }

    public void SetVisualSelection(string character, string chair) {
        selectedChair = chair;
        selectedPerson = character;
        SaveManager.SaveGame(chair, character, levelHighScores);
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

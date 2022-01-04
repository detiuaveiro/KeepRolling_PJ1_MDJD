using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    public GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMenu()
    {
        menu.SetActive(true);
    }

    public void Resume()
    {
        menu.SetActive(false);
    }

    public void Restart()
    {

    }

    public void OpenSettings()
    {

    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

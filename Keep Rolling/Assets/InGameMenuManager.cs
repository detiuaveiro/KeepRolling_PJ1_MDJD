using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject settingsMenu;
    public Dropdown resolutionsDropDown;
    private Resolution[] resolutions;

    public void Start()
    {
        resolutions = Screen.resolutions;
        resolutionsDropDown.ClearOptions();

        int currentResolution = 0;

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }

        resolutionsDropDown.AddOptions(options);
        resolutionsDropDown.value = currentResolution;
        resolutionsDropDown.RefreshShownValue();

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
        SceneManager.LoadScene("InGame");
    }

    public void NextLevel()
    {
        GameManager.instance.currentLevel += 1;
        SceneManager.LoadScene("InGame");
    }

    public void OpenSettings()
    {
        menu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void BackToMenuFromSettings()
    {
        settingsMenu.SetActive(false);
        menu.SetActive(true);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width,res.height,Screen.fullScreen);
    }
}

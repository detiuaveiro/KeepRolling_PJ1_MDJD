using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject settingsMenu;
    public GameObject help;
    public Dropdown resolutionsDropDown;
    private Resolution[] resolutions;

    public AudioMixer audioMixer;
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider soundEffectsVolSlider;
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

        float vol = 0.5f;
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            vol = PlayerPrefs.GetFloat("MasterVolume");
        }
        masterVolSlider.value = vol;
        vol = 1f;
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            vol = PlayerPrefs.GetFloat("MusicVolume");
        }
        musicVolSlider.value = vol;
        vol = 1f;
        if (PlayerPrefs.HasKey("SoundEffectsVolume"))
        {
            vol = PlayerPrefs.GetFloat("SoundEffectsVolume");
        }
        soundEffectsVolSlider.value = vol;
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

    public void OpenHelp()
    {
        help.SetActive(true);
    }

    public void CloseHelp()
    {
        help.SetActive(false);
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

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    public void SetSoundEffectsVolume(float volume)
    {
        audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SoundEffectsVolume", volume);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject Home;
    public GameObject Play;
    public GameObject Settings;
    public GameObject Leaderboard;
    public GameObject ModifyChar;

    private GameObject currentDisplaying;
    private List<Command> NavigateLog;

    private Resolution[] resolutions;
    public Dropdown resolutionsDropDown;

    public AudioMixer audioMixer;
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider soundEffectsVolSlider;
    // Start is called before the first frame update
    void Start()
    {
        NavigateLog = new List<Command>();
        NavigateHome();

        resolutions = Screen.resolutions;
        resolutionsDropDown.ClearOptions();

        int currentResolution = 0;

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
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

    public void NavigateHome()
    {
        Navigate(Home);
    }
    public void NavigateSettings()
    {
        Navigate(Settings);
    }

    public void NavigatePlay()
    {
        Navigate(Play);
    }

    public void NavigateLeaderboard()
    {
        Navigate(Leaderboard);
    }

    public void NavigateModifyChar()
    {
        Navigate(ModifyChar);
    }

    private void Navigate(GameObject destiny)
    {
        if (currentDisplaying != null)
        {
            currentDisplaying.SetActive(false);
        }

        currentDisplaying = destiny;
        currentDisplaying.SetActive(true);
    }

    public void SettingsButton()
    {
        ExecuteCommand(new GoToSettings(this));
    }

    public void PlayButton()
    {
        ExecuteCommand(new GoToPlay(this));
    }

    public void LeaderBoardButton()
    {
        ExecuteCommand(new GoToLeaderboard(this));
    }

    public void ModifyCharButton()
    {
        ExecuteCommand(new GoToModifyChar(this));
    }

    private void ExecuteCommand(Command command)
    {
        NavigateLog.Add(command);
        command.Execute();
    }

    public void LoadLevel(int level)
    {
        GameManager.instance.StartLevel(level);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void UndoCommand()
    {
        Command lastCommand = NavigateLog[NavigateLog.Count - 1];
        NavigateLog.Remove(lastCommand);
        lastCommand.Undo();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
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

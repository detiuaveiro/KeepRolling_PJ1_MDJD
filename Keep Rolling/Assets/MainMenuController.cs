using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}

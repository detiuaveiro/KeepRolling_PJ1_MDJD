using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject Home;
    public GameObject Play;
    public GameObject Settings;
    public GameObject Leaderboard;

    private GameObject currentDisplaying;
    private List<Command> NavigateLog;

    // Start is called before the first frame update
    void Start()
    {
        NavigateLog = new List<Command>();
        NavigateHome();
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

    private void ExecuteCommand(Command command)
    {
        NavigateLog.Add(command);
        command.Execute();
    }

    public void LoadLevel(int level)
    {
        Debug.Log("Will load level: " + level);
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
}

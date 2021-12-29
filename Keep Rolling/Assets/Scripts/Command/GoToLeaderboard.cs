
public class GoToLeaderboard : Command
{
    private MainMenuController instance;
    public GoToLeaderboard(MainMenuController ist)
    {
        this.instance = ist;
    }

    public override void Execute()
    {
        this.instance.NavigateLeaderboard();
    }

    public override void Undo()
    {
        this.instance.NavigateHome();
    }
}
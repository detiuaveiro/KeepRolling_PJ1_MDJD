
public class GoToSettings : Command
{
    private MainMenuController instance;
    public GoToSettings(MainMenuController ist)
    {
        this.instance = ist;
    }

    public override void Execute()
    {
        this.instance.NavigateSettings();
    }

    public override void Undo()
    {
        this.instance.NavigateHome();
    }
}
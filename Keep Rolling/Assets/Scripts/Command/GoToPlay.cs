
public class GoToPlay : Command
{
    private MainMenuController instance;
    public GoToPlay(MainMenuController ist)
    {
        this.instance = ist;
    }

    public override void Execute()
    {
        this.instance.NavigatePlay();
    }

    public override void Undo()
    {
        this.instance.NavigateHome();
    }
}

public class GoToModifyChar : Command
{
    private MainMenuController instance;
    public GoToModifyChar(MainMenuController ist)
    {
        this.instance = ist;
    }

    public override void Execute()
    {
        this.instance.NavigateModifyChar();
    }

    public override void Undo()
    {
        this.instance.NavigateHome();
    }

}

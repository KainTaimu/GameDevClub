namespace Game.Levels;

public partial class MasterScene : Node
{
    [Export]
    private PackedScene? _mainMenu;

    [Export]
    private PackedScene? _mainGame;

    public override void _Ready()
    {
        Callable.From(() =>
        {
            if (OS.IsDebugBuild())
            {
                GetTree().ChangeSceneToPacked(_mainGame);
                return;
            }
            else
            {
                GetTree().ChangeSceneToPacked(_mainMenu);
                return;
            }
        }).CallDeferred();
    }
}

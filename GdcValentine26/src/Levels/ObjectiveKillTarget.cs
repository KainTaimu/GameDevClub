using Game.Core;
using Game.UI;

namespace Game.Levels;

public partial class ObjectiveKillTarget : Node
{
    [Export]
    private Node? KillTarget
    {
        get;
        set
        {
            field?.TreeExiting -= OnTargetExiting;

            field = value;

            field?.TreeExiting += OnTargetExiting;
        }
    }

    [Export]
    private WinScreen _winScreen = null!;

    [Export]
    private PauseController _pauseController = null!;

    private void OnTargetExiting()
    {
        _winScreen.ShowScreen();
        _pauseController.Pause(this);
    }
}

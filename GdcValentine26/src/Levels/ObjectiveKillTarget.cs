using Game.Core;

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
    private string _winScreenScenePath = null!;

    [Export]
    private CanvasLayer _uiLayer = null!;

    [Export]
    private PauseController _pauseController = null!;

    private void OnTargetExiting()
    {
        var winScreen = GD.Load<PackedScene>(_winScreenScenePath).Instantiate();
        _uiLayer.CallDeferred(Node.MethodName.AddChild, winScreen);

        _pauseController.Pause(this);
    }
}

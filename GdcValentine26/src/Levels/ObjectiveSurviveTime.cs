namespace Game.Levels;

public partial class ObjectiveSurviveTime : Node
{
    [Export]
    private double _timeSurvive = 30;

    [Export]
    private PackedScene _nextLevel = null!;

    [Export]
    private Label _objectiveLabel = null!;

    private double _timeRemaining;

    public override void _Ready()
    {
        _timeRemaining = _timeSurvive;
        _objectiveLabel.Text = $"START RUNNING! {_timeRemaining:.3}s";

        GameWorld.Instance.OnPlayerDeath += () => ProcessMode = ProcessModeEnum.Disabled;
    }

    public override void _Process(double delta)
    {
        _objectiveLabel.Text = $"START RUNNING! {(_timeRemaining - delta).ToString("F3")}s";
        if (_timeRemaining > 0)
        {
            _timeRemaining -= delta;
            return;
        }

        Logger.LogInfo("Level 3 -> Level 4");
        GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, _nextLevel);
    }
}

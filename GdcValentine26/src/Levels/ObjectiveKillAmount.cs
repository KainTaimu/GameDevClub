using Game.Core.ECS;

namespace Game.Levels;

public partial class ObjectiveKillAmount : Node
{
    [Export]
    private int _targetKills = 200;

    [Export]
    private PackedScene _nextLevel = null!;

    [ExportCategory("Components")]
    [Export]
    private Label _helpLabel = null!;

    [Export]
    private EntityComponentStore _entities = null!;

    private int Kills
    {
        get;
        set
        {
            field = value;
            _helpLabel.Text = $"Get kills! ({field}/{_targetKills})";
        }
    }

    public override void _Ready()
    {
        _helpLabel.Text = $"Get kills! ({Kills}/{_targetKills})";
        _entities.BeforeEntityUnregistered += (_) =>
        {
            Kills++;

            if (Kills >= _targetKills)
            {
                Logger.LogInfo("Level 2 -> Level 3");
                GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, _nextLevel);
            }
        };
    }
}

using Game.Core.ECS;
using Game.UI;

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

    [Export]
    private WinScreen _winScreen = null!;

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
                _winScreen.ShowScreen();
            }
        };
    }
}

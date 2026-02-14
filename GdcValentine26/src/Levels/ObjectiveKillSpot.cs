using Game.Core.ECS;

namespace Game.Level;

public partial class ObjectiveKillSpot : Area2D
{
    [Export]
    private int _killTarget = 50;

    [Export]
    private int _killRadius = 64;

    [ExportCategory("Components")]
    [Export]
    private CollisionShape2D _collisionShape = null!;

    [Export]
    private Label _helpLabel = null!;

    [Export]
    private EntityComponentStore _entities = null!;

    private int KillCount
    {
        get;
        set
        {
            _helpLabel.Text = $"Kill enemies here! ({value}/{_killTarget})";
            field = value;
        }
    }

    public override void _Ready()
    {
        _helpLabel.Text = $"Kill enemies here! ({KillCount}/{_killTarget})";

        if (_collisionShape.Shape is not CircleShape2D circle)
        {
            Logger.LogError("CollisionShape2D has to be CircleShape2D!");
            return;
        }

        var radius = circle.Radius = _killRadius;

        _entities.BeforeEntityUnregistered += (id) =>
        {
            if (!_entities.GetComponent<PositionComponent>(id, out var pos))
                return;
            if (pos.Position.DistanceTo(GlobalPosition) > radius)
                return;

            KillCount++;
        };
    }

    public override void _Draw()
    {
        base._Draw();
        DrawCircle(GlobalPosition, _killRadius, new Color(255, 0, 0, 0.2f));
    }
}

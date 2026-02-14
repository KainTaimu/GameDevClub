namespace Game.Enemies;

public partial class EnemyMovementController : Node
{
    [Export]
    private EnemyEcsProxyBoss _boss = null!;

    [Export]
    private EnemyStats _stats = null!;

    public override void _Process(double delta)
    {
        var player = GameWorld.Instance.MainPlayer;
        if (player is null)
            return;

        _boss.GlobalPosition +=
            _boss.GlobalPosition.DirectionTo(player.GlobalPosition)
            * _stats.MoveSpeed
            * (float)delta;
    }
}

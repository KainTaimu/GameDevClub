namespace Game.Players;

public partial class PlayerCollisionArea : Area2D
{
    [Export]
    public Player Player;

    public override void _Ready()
    {
        GameWorld.Instance.OnPlayerDeath += () =>
        {
            SetDeferred(Area2D.PropertyName.Monitorable, false);
            SetDeferred(Area2D.PropertyName.Monitoring, false);
        };
    }
}

namespace Game.Obstacles;

public partial class ObstacleInteractionArea : Area2D
{
    [Export]
    public Obstacle Obstacle { get; private set; }

    public override void _Ready()
    {
        GameWorld.Instance.OnPlayerDeath += () =>
        {
            SetDeferred(Area2D.PropertyName.Monitorable, false);
            SetDeferred(Area2D.PropertyName.Monitoring, false);
        };
    }
}

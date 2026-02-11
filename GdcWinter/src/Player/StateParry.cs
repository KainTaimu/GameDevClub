using Game.Weapons;

namespace Game.Players;

public partial class StateParry : State
{
    [Export]
    private Area2D _parryArea;

    private double _cooldown;

    public override void Enter()
    {
        var overlapping = _parryArea.GetOverlappingAreas();
        foreach (var area in overlapping)
        {
            OnProjectileEntered(area);
        }

        MovementController.ChangeState<StateGrounded>();
    }

    public override void Exit() { }

    private void OnProjectileEntered(Area2D area)
    {
        switch (area)
        {
            case ProjectileBullet bullet:
                // Reflect
                // BUG: Can reflect more than once while projectile is in parry area.
                // How to track already reflected bullets?
                bullet.GlobalRotation *= -1;
                bullet.Velocity = 1200;
                bullet.SetDeferred(Area2D.PropertyName.Monitoring, false);
                bullet.SetDeferred(Area2D.PropertyName.Monitorable, false);
                break;
            default:
                break;
        }
    }

    private float GetAngleFromPlayer(Vector2 point)
    {
        var viewport = GetViewport();
        var player = GameWorld.Instance.MainPlayer;
        var playerPos =
            player.GlobalPosition * viewport.GetCamera2D().GetCanvasTransform().AffineInverse();
        var crosshairPos = point * viewport.GetScreenTransform();

        var angle = playerPos.AngleToPoint(crosshairPos);
        return angle;
    }
}

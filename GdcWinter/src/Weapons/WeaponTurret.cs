namespace Game.Weapons;

public partial class WeaponTurret : Weapon
{
    [Export]
    private PackedScene _projectile;

    [Export]
    private Marker2D _barrelEnd;

    [Export]
    private Timer _timer;

    /// <summary>
    /// How far ahead the turret aims to compensate
    /// </summary>
    // BUG: Must manually adjust when scroll speed is changed! Use kinematics to dynamically predict?
    private const int FORWARD_COMPENSATION = 600;

    public override void _Ready()
    {
        _timer.WaitTime = Stats.AttackSpeed;
        _timer.Autostart = true;
        _timer.Start();
        _timer.Timeout += Attack;
    }

    protected override void AfterProjectileHit(Node2D target)
    {
        Logger.LogDebug($"Hit {target.Name}");
    }

    public void Attack()
    {
        var projectile = _projectile.Instantiate<ProjectileBullet>();
        projectile.Initialize(this);
        projectile.SetPosition(Position);
        projectile.SetRotation(GetAngleFromPlayer());
        projectile.TargetsWhat = ProjectileTargetsWhat.Player;
        AddChild(projectile);
    }

    private float GetAngleFromPlayer()
    {
        var _viewport = GetViewport(); // TODO: EXPENSIVE ASF. CACHE VIEWPORT
        var player = GameWorld.Instance.MainPlayer;
        var playerPos =
            player.GlobalPosition * _viewport.GetCamera2D().GetCanvasTransform().AffineInverse();
        playerPos = new Vector2(playerPos.X + FORWARD_COMPENSATION, playerPos.Y);
        var crosshairPos = GlobalPosition * _viewport.GetScreenTransform();

        var angle = playerPos.AngleToPoint(crosshairPos);
        return angle;
    }
}

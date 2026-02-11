namespace Game.Weapons;

public partial class WeaponDrone : Weapon
{
    [Export]
    private PackedScene _projectile;

    [Export]
    private Node _bulletParent;

    [Export]
    private Timer _timer;

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
        projectile.SetGlobalPosition(GlobalPosition);
        projectile.SetRotation(GetAngleFromPlayer());
        projectile.Scale *= 2;
        projectile.TargetsWhat = ProjectileTargetsWhat.Player;

        // Using a Node so bullets don't inherit drone's position
        _bulletParent.AddChild(projectile);

        var rand = (float)Stats.Additional["AttackSpeedRand"];
        _timer.WaitTime = Stats.AttackSpeed + GD.RandRange(-rand, rand);
    }

    private float GetAngleFromPlayer()
    {
        var _viewport = GetViewport(); // TODO: EXPENSIVE ASF. CACHE VIEWPORT
        var player = GameWorld.Instance.MainPlayer;
        var playerPos =
            player.GlobalPosition * _viewport.GetCamera2D().GetCanvasTransform().AffineInverse();
        playerPos = new Vector2(playerPos.X, playerPos.Y);
        var crosshairPos = GlobalPosition * _viewport.GetScreenTransform();

        var angle = playerPos.AngleToPoint(crosshairPos);
        return angle;
    }
}

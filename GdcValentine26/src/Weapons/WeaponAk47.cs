using System.Threading.Tasks;
using Game.Core;
using Game.Core.ECS;
using Game.Enemies;
using Game.Players;
using Game.Projectiles;
using Game.UI;

namespace Game.Weapons;

public partial class WeaponAk47 : BaseWeapon, IMagazine
{
    [Export]
    private PackedScene _projectileScene = null!;

    private Player Player => GameWorld.Instance.MainPlayer!;

    private Crosshair? Crosshair => Crosshair.Instance;

    public int MagazineCapacity
    {
        get => _magazineCapacity;
    }

    public int MagazineCount
    {
        get => _magazineCount;
    }

    private double _fireCooldown;
    private bool _isReloading;

    private int _reloadTimeMs = 1500;
    private float _bloomCoefficient = 0.03f;
    private int _magazineCapacity = 30;
    private int _magazineCount;

    private float _horizontalBaseRecoil = 3f;
    private float _horizontalRecoilRandom = 1f;
    private float _verticalBaseRecoil = 3f;
    private float _verticalRecoilRandom = 0.1f;

    public override void _Ready()
    {
        _magazineCapacity = (int)Stats.Additional["MagazineCapacity"];
        _magazineCount = _magazineCapacity;
        _reloadTimeMs = (int)Stats.Additional["ReloadTimeMs"];
        _bloomCoefficient = (float)Stats.Additional["BloomCoefficient"];

        _horizontalBaseRecoil = (float)Stats.Additional["HorizontalBaseRecoil"];
        _horizontalRecoilRandom = (float)Stats.Additional["HorizontalRecoilRandom"];
        _verticalBaseRecoil = (float)Stats.Additional["VerticalBaseRecoil"];
        _verticalRecoilRandom = (float)Stats.Additional["VerticalRecoilRandom"];
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed(InputMapNames.GunReload))
            Reload();

        if (Input.IsActionPressed(InputMapNames.PrimaryAttack))
            Shoot();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_fireCooldown <= 0)
            return;
        _fireCooldown -= delta;
    }

    private void Shoot()
    {
        if (_isReloading)
            return;

        if (_magazineCount <= 0)
        {
            Reload();
            return;
        }

        if (_fireCooldown > 0)
            return;

        Attack();
        _fireCooldown = Stats.AttackSpeedSec;
    }

    protected override void Attack()
    {
        if (_magazineCount <= 0 || _isReloading)
            return;

        _magazineCount--;

        var playerVector = Player.GetCanvasTransform() * Player.Position;

        Vector2 mouseVector;
        if (Crosshair is not null)
        {
            mouseVector =
                Crosshair.CrosshairSprite.GetCanvasTransform()
                * Crosshair.CrosshairSprite.GlobalPosition;
        }
        else
        {
            mouseVector = Player.GetGlobalMousePosition();
        }
        var rotation = playerVector.AngleToPoint(mouseVector);

        var bloom = (float)GD.Randfn(rotation, _bloomCoefficient);

        var projectile = _projectileScene.Instantiate<ProjectileAk47Bullet>();
        projectile.Initialize(this, EntityType.Enemy | EntityType.Breakable);

        projectile.SetScale(Vector2.One * Stats.ProjectileScaleMultiplier);
        projectile.SetPosition(Player.Position);
        projectile.SetRotation(bloom);
        projectile.ProjectileSpeed = Stats.ProjectileSpeed;
        projectile.PierceLimit = Stats.PierceLimit;

        projectile.OnTargetHit += HandleHit;
        AddChild(projectile);

        ApplyCursorRecoil();
    }

    public void Reload()
    {
        if (_isReloading)
            return;

        if (_magazineCount == _magazineCapacity)
            return;

        _isReloading = true;
        _ = ReloadTask();
    }

    private async Task ReloadTask()
    {
        await Task.Delay(_reloadTimeMs);
        _isReloading = false;
        _magazineCount = _magazineCapacity;
    }

    private void ApplyCursorRecoil()
    {
        if (Crosshair is null)
            return;

        var recoilX = _horizontalBaseRecoil * (float)GD.Randfn(0, _horizontalRecoilRandom);
        var recoilY = _verticalBaseRecoil * (float)GD.Randfn(1, _verticalRecoilRandom);
        recoilY = Math.Clamp(recoilY, 2, float.MaxValue);

        var recoil = new Vector2(recoilX, -recoilY);
        Crosshair.Recoil.ApplyImpulse(recoil, 1f);
    }

    protected override void HandleHit(Node target)
    {
        if (target is not EnemyECSProxy enemy)
            return;

        var enemyId = enemy.Id;
        var componentStore = GameWorld.Instance.EntityComponentStore;

        if (!componentStore.GetComponent<PositionComponent>(enemyId, out var pos))
            return;

        var direction = Player.GlobalPosition.DirectionTo(pos.Position);
        var pushVector = direction * 10;

        componentStore.UpdateComponent(
            enemyId,
            new PositionComponent(pos.Position + pushVector, pos.Collidable)
        );

        enemy.Health -= Stats.Damage;
    }
}

public interface IMagazine
{
    public int MagazineCount { get; }
    public int MagazineCapacity { get; }
}

using Game.Obstacles;
using Game.Players;

namespace Game.Weapons;

public partial class Projectile : Area2D
{
    [Signal]
    public delegate void OnEntityHitEventHandler(Node2D target);

    public ProjectileTargetsWhat TargetsWhat = ProjectileTargetsWhat.Player;

    protected Weapon _origin;

    public virtual void Initialize(Weapon origin)
    {
        _origin = origin;
        OnEntityHit += _origin.OnProjectileHit;
    }

    private void OnAreaEntered(Area2D area)
    {
        switch (area)
        {
            case ObstacleCollisionArea obstacle:
                if (TargetsWhat != ProjectileTargetsWhat.Obstacle)
                    return;

                EmitSignal(SignalName.OnEntityHit, obstacle.Obstacle);
                break;
            case PlayerCollisionArea player:
                if (TargetsWhat != ProjectileTargetsWhat.Player)
                    return;

                EmitSignal(SignalName.OnEntityHit, player.Player);
                break;
            default:
                break;
        }
    }
}

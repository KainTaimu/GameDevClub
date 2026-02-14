using Game.Core;
using Game.Weapons;

namespace Game.Projectiles;

public partial class BaseProjectile : Node2D
{
    [Signal]
    public delegate void OnTargetHitEventHandler(Node target);

    [Export]
    public EntityType TargetsWhat;

    public BaseWeapon Origin { get; private set; } = null!;

    public virtual void Initialize(BaseWeapon origin, EntityType targetWhat)
    {
        Origin = origin;
        TargetsWhat = targetWhat;
    }
}

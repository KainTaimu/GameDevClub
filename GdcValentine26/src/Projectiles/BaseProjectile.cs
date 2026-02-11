using Game.Items;

namespace Game.Projectiles;

public partial class BaseProjectile : Node2D
{
    [Signal]
    public delegate void OnTargetHitEventHandler();

    public BaseItem Origin = null!;

    public virtual void Initialize() { }
}

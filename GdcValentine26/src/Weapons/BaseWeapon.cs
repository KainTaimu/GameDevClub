using Game.Items;

namespace Game.Weapons;

public partial class BaseWeapon : BaseItem
{
    [Export]
    public WeaponStats Stats { get; private set; } = null!;

    protected virtual void Attack() { }

    protected virtual void HandleHit() { }
}

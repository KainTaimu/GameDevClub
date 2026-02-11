namespace Game.Weapons;

public partial class Weapon : Node2D
{
    [Export]
    public WeaponStats Stats;

    public void OnProjectileHit(Node2D target)
    {
        if (target is not IHittable hittable)
        {
            Logger.LogError(
                $"Weapon received OnProjectileHit. But the target {target.Name} does not implement IHittable"
            );
            return;
        }
        hittable.HandleHit(this);
        AfterProjectileHit(target);
    }

    protected virtual void AfterProjectileHit(Node2D target) { }
}

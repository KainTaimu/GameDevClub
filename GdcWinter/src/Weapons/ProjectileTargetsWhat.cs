namespace Game.Weapons;

/// <summary>
/// What the projectile accepts as target.
/// </summary>
[Flags]
public enum ProjectileTargetsWhat
{
    Player = 0,
    Obstacle = 1,
}

using Godot.Collections;

namespace Game.Weapons;

[GlobalClass]
public partial class WeaponStats : Resource
{
    [Export]
    public int Damage = 10;

    [Export]
    public float CritDamageMultiplier = 1;

    [Export]
    public float CritChanceProportion = 0;

    [Export]
    public int ProjectileSpeed = 3600;

    [Export]
    public float ProjectileScaleMultiplier = 1;

    [Export]
    public float AttackSpeedSec = 2;

    [Export]
    public int PierceLimit = 1;

    [Export]
    public Dictionary<string, Variant> Additional = null!;
}

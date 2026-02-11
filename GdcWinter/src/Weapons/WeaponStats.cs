using Godot.Collections;

namespace Game.Weapons;

[GlobalClass]
public partial class WeaponStats : Resource
{
    [Export]
    public int Damage = 1;

    [Export]
    public float AttackSpeed = 1;

    [Export]
    public Dictionary<string, Variant> Additional;
}

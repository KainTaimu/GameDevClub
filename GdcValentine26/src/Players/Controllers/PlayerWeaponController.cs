using System.Collections.Generic;

namespace Game.Players.Controllers;

public partial class PlayerWeaponController : Node
{
    public readonly List<IWeapon> _weapons = [];

    public override void _Ready()
    {
        foreach (var node in GetChildren())
        {
            if (node is not IWeapon weapon)
                continue;

            _weapons.Add(weapon);
        }
    }
}

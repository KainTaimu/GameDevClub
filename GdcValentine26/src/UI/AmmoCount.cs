using System.Linq;
using Game.Weapons;

namespace Game.UI;

public partial class AmmoCount : Label
{
    private IMagazine _ak = null!;

    public override void _Ready()
    {
        _ak = (IMagazine)
            GameWorld
                .Instance!.MainPlayer!.GetNode("PlayerWeaponController")
                .GetChildren()
                .Single((x) => x is IMagazine);

        Text = string.Concat(Enumerable.Repeat("I", _ak.MagazineCapacity));
        VisibleCharacters = _ak.MagazineCount;
    }

    public override void _PhysicsProcess(double delta)
    {
        VisibleCharacters = _ak.MagazineCount;
    }
}

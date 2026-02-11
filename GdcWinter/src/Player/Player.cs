using Game.Weapons;

namespace Game.Players;

public partial class Player : Node2D, IHittable
{
    [Export]
    public PlayerHealthController StatController { get; private set; }

    public override void _EnterTree()
    {
        GameWorld.Instance.MainPlayer = this;
    }

    public void HandleHit(Weapon hitBy)
    {
        StatController.HandleHit(hitBy.Stats.Damage);
    }
}

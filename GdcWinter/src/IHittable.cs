using Game.Weapons;

namespace Game;

public interface IHittable
{
    void HandleHit(Weapon hitBy);
}

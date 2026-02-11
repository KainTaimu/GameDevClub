using Game.Weapons;

namespace Game.Obstacles;

public partial class ObstacleTurret : Obstacle, IHittable
{
    public void HandleHit(Weapon hitBy)
    {
        throw new NotImplementedException();
    }

    public override void _Process(double delta)
    {
        Scroll(delta);
    }
}

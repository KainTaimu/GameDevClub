using Game.Core;
using Game.Interfaces;

namespace Game.Players;

public partial class Player : Node2D, IHittable
{
    [Export]
    public Character Character { get; private set; } = null!;

    [Export]
    private Area2D _hitboxArea = null!;

    public EntityType EntityType { get; } = EntityType.Player;

    private float _iTime;

    public override void _Ready()
    {
        GameWorld.Instance.MainPlayer = this;

        _hitboxArea.AreaEntered += HandleHit;

        Character.PlayerStats.OnHealthChanged += (_, x) =>
        {
            if (x <= 0)
            {
                GameWorld.Instance.EmitSignal(GameWorld.SignalName.OnPlayerDeath);
                GetNode("./PlayerMovementController").QueueFree();
                GetNode("./PlayerWeaponController").QueueFree();
            }
        };
    }

    public override void _Process(double delta)
    {
        _iTime = Math.Clamp(_iTime - (float)delta, 0, float.MaxValue);
    }

    private void HandleHit(Area2D area)
    {
        if (_iTime > 0)
            return;

        Character.PlayerStats.Health--;
        Logger.LogDebug("hit", area.Name, Character.PlayerStats.Health);

        _iTime = Character.PlayerStats.InvincibilityTime;
    }
}

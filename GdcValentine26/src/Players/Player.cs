using Game.Core;
using Game.Enemies;
using Game.Interfaces;

namespace Game.Players;

public partial class Player : Node2D, IHittable
{
    [Export]
    public Character Character { get; private set; } = null!;

    [Export]
    private Area2D _hitboxArea = null!;

    [Export]
    private TextureRect _sprite = null!;

    public EntityType EntityType { get; } = EntityType.Player;

    private float _iTime;
    private Tween? _tween;
    private ShaderMaterial _spriteShaderMaterial = null!;

    public override void _Ready()
    {
        _spriteShaderMaterial = _sprite.Material as ShaderMaterial;

        GameWorld.Instance.MainPlayer = this;

        _hitboxArea.AreaEntered += HandleHit;

        Character.PlayerStats.OnHealthChanged += (_, x) =>
        {
            if (x <= 0)
            {
                GameWorld.Instance.EmitSignal(GameWorld.SignalName.OnPlayerDeath);
                GetNode("./PlayerMovementController").QueueFree();
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

        if (area.GetParent() is not EnemyECSProxy && area.GetParent() is not EnemyEcsProxyBoss)
            return;

        var enemy = area.GetParent();

        int damage;

        switch (enemy)
        {
            case EnemyECSProxy a:
                damage = a.Stats.Damage;
                break;
            case EnemyEcsProxyBoss a:
                damage = a.Stats.Damage;
                break;
            default:
                return;
        }

        Character.PlayerStats.Health -= damage;

        _iTime = Character.PlayerStats.InvincibilityTime;

        HitFeedback();
    }

    private void HitFeedback()
    {
        _tween?.Kill();
        _tween = CreateTween()
            .BindNode(this)
            .SetTrans(Tween.TransitionType.Expo)
            .SetEase(Tween.EaseType.Out);
        _spriteShaderMaterial.SetShaderParameter("flash_state", 1f);
        _spriteShaderMaterial.SetShaderParameter("color", new Color("white"));
        _tween.TweenMethod(
            Callable.From(
                (float i) =>
                {
                    _spriteShaderMaterial.SetShaderParameter("flash_state", i);
                }
            ),
            1f,
            0f,
            1f
        );
    }
}

using Game.Core;
using Game.Interfaces;

namespace Game.Enemies;

public partial class EnemyEcsProxyBoss : Node2D, IHittable
{
    public EntityType EntityType => EntityType.Enemy;

    public int Health
    {
        get => Stats.Health;
        set
        {
            if (value < Stats.Health)
                HitFeedback();

            Stats.Health = value;
            if (Stats.Health <= 0)
                Die();
        }
    }

    [Export]
    public EnemyStats Stats = null!;

    [ExportCategory("Components")]
    [Export]
    public AnimatedSprite2D Sprite { get; private set; } = null!;

    private Tween? _tween;
    private ShaderMaterial _spriteShaderMaterial = null!;

    public override void _Ready()
    {
        _spriteShaderMaterial = Sprite.Material as ShaderMaterial;
    }

    private void Die()
    {
        var tween = CreateTween().SetEase(Tween.EaseType.Out);
        tween.TweenProperty(Sprite, "scale", Vector2.Zero, 0.1f);
        tween.TweenCallback(Callable.From(QueueFree));
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

using Game.Core;
using Game.Core.ECS;
using Game.Interfaces;

namespace Game.Enemies;

public partial class EnemyECSProxy : Node, IHittable
{
    public EntityType EntityType => EntityType.Enemy;

    public int Health
    {
        get => Stats.Health;
        set
        {
            if (value < Stats.Health)
            {
                HitFeedback();
            }

            Stats.Health = value;
            if (Stats.Health <= 0)
            {
                Die();
            }
        }
    }

    [Export]
    public EnemyStats Stats { get; private set; } = null!;

    [ExportCategory("Components")]
    [Export]
    public AnimatedSprite2D Sprite { get; private set; } = null!;

    [Export]
    public Area2D HitboxArea { get; private set; } = null!;

    [Export]
    public CpuParticles2D KillParticles = null!;

    [Export]
    private TextureRect _bowRect = null!;

    public int Id { get; set; }

    private Tween? _tween;
    private ShaderMaterial _spriteShaderMaterial = null!;

    private float[] Colours = [0.418f, 0f, 0.85f];

    public override void _Ready()
    {
        _spriteShaderMaterial = Sprite.Material as ShaderMaterial;
        RandomizeHue();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (
            !GameWorld.Instance.EntityComponentStore.GetComponent<PositionComponent>(
                Id,
                out var pos
            )
        )
        {
            return;
        }

        Sprite.GlobalPosition = pos.Position;
        HitboxArea.GlobalPosition = pos.Position;
        KillParticles.GlobalPosition = pos.Position;
    }

    private void RandomizeHue()
    {
        var shader = _bowRect.Material as ShaderMaterial;
        var rand = GD.RandRange(0, Colours.GetLength(0) - 1);
        shader.SetShaderParameter("hue_shift", Colours[rand]);
    }

    private void Die()
    {
        KillParticles.Emitting = true;
        GameWorld.Instance.EntityComponentStore.UnregisterEntity(Id);

        HitboxArea.SetDeferred(Area2D.PropertyName.Monitorable, false);
        HitboxArea.SetDeferred(Area2D.PropertyName.Monitoring, false);

        var tween = CreateTween().SetEase(Tween.EaseType.Out).SetParallel(true);
        tween.TweenProperty(Sprite, "scale", Vector2.Zero, 0.1f);

        tween.TweenCallback(Callable.From(QueueFree)).SetDelay(1);
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

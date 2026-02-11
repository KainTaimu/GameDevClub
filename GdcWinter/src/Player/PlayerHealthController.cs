namespace Game.Players;

public partial class PlayerHealthController : Node
{
    [Signal]
    public delegate void OnPlayerHitEventHandler();

    [Export]
    public Stats Stats { get; private set; }

    [ExportCategory("Components")]
    [Export]
    private PlayerStateController _movementController;

    [Export]
    private AnimatedSprite2D _sprite;

    public override void _Ready()
    {
        Stats.Initialize();
    }

    public void HandleHit(int damage)
    {
        if (Stats.Health <= 0)
            return;

        if (Stats.Health - damage <= 0)
        {
            Stats.Health = 0;
            _movementController.ChangeState<StateDying>();
            GameWorld.Instance.AnnouncePlayerDead(this);
        }
        else
        {
            Stats.Health -= damage;
        }

        DamageFeedback();
        EmitSignal(SignalName.OnPlayerHit);
    }

    private void DamageFeedback()
    {
        if (_sprite.Material is not ShaderMaterial spriteShaderMaterial)
        {
            return;
        }

        var tween = CreateTween();
        tween?.Kill();
        tween = CreateTween()
            .BindNode(this)
            .SetTrans(Tween.TransitionType.Expo)
            .SetEase(Tween.EaseType.Out);

        spriteShaderMaterial.SetShaderParameter("flash_state", 1f);
        spriteShaderMaterial.SetShaderParameter("color", new Color("white"));

        tween.TweenMethod(
            Callable.From(
                (float i) =>
                {
                    spriteShaderMaterial.SetShaderParameter("flash_state", i);
                }
            ),
            1f,
            0f,
            1f
        );
    }
}

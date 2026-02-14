using Game.Interfaces;

namespace Game.Projectiles;

public partial class ProjectileAk47Bullet : BaseProjectile
{
    [Export]
    private Area2D _collisionArea = null!;

    public int ProjectileSpeed;
    public int PierceLimit;
    public bool HasHitThisFrame;

    private int _pierceCount = 0;

    public override void _Ready()
    {
        var tweenSpeed = CreateTween()
            .BindNode(this)
            .SetTrans(Tween.TransitionType.Expo)
            .SetEase(Tween.EaseType.In);
        var tweenScale = CreateTween()
            .BindNode(this)
            .SetTrans(Tween.TransitionType.Linear)
            .SetEase(Tween.EaseType.In);
        var originalSpeed = ProjectileSpeed;

        ProjectileSpeed = originalSpeed / 2;
        tweenSpeed.TweenProperty(this, nameof(ProjectileSpeed), originalSpeed, 0.13f);
        tweenScale.TweenProperty(this, "scale", new Vector2(4, 1), 0.13f);

        _collisionArea.AreaEntered += (a) =>
        {
            if (HasHitThisFrame)
                return;

            if (_pierceCount >= PierceLimit)
                return;

            if (a.GetParent() is not IHittable hit)
                return;

            if ((hit.EntityType & TargetsWhat) == 0)
                return;

            HasHitThisFrame = true;
            _pierceCount++;
            EmitSignal(BaseProjectile.SignalName.OnTargetHit, a.GetParent());

            if (_pierceCount >= PierceLimit)
            {
                Hide();
                QueueFree();
            }
        };
    }

    public override void _Process(double delta)
    {
        HasHitThisFrame = false;
        MoveTowardPoint(delta);
    }

    private void MoveTowardPoint(double delta)
    {
        var moveVector = new Vector2(1, 0).Rotated(Rotation) * ProjectileSpeed * (float)delta;

        Position += moveVector;
    }
}

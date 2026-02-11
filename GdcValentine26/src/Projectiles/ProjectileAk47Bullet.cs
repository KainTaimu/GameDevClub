namespace Game.Projectiles;

public partial class ProjectileAk47Bullet : BaseProjectile
{
    public int ProjectileSpeed;
    public int PierceLimit;
    public bool HasHitThisFrame;

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
    }

    public override void _Process(double delta)
    {
        MoveTowardPoint(delta);
    }

    private void MoveTowardPoint(double delta)
    {
        var moveVector = new Vector2(1, 0).Rotated(Rotation) * ProjectileSpeed * (float)delta;

        Position += moveVector;
    }
}

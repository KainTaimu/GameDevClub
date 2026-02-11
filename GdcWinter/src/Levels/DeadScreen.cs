namespace Game.Levels;

public partial class DeadScreen : CanvasLayer
{
    [Export]
    private ColorRect _blurRect;

    [Export]
    private Label _topLabel;

    public override void _Ready()
    {
        Visible = false;
        GameWorld.Instance.OnPlayerDeath += () =>
        {
            Visible = true;
            var tween = CreateTween().SetParallel();
            tween.TweenMethod(
                Callable.From(
                    (float f) =>
                    {
                        var shader = _blurRect.Material as ShaderMaterial;
                        shader.SetShaderParameter("amount", f);
                    }
                ),
                0f,
                1f,
                1f
            );
            tween.TweenProperty(_topLabel, "modulate", new Color("ffffff"), 1f);
        };
    }

    public void Reload()
    {
        GetTree().ReloadCurrentScene();
    }
}

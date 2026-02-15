namespace Game.UI;

public partial class WinScreen : CenterContainer
{
    [Export]
    private Button _retryButton = null!;

    [Export]
    private Button _startOverButton = null!;

    [Export]
    private ColorRect _blurRect;

    [Export]
    private Label _topLabel;

    public override void _Ready() { }

    public void ShowScreen()
    {
        Show();
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

        GetNode<Crosshair>("../Crosshair").HideCrosshair();
    }

    public void Retry()
    {
        GetTree().ReloadCurrentScene();
    }

    public void StartOver()
    {
        GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile, "uid://dk477scr0by7o");
    }
}

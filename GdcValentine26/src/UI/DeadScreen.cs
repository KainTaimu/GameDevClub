using Game.Core;
using Game.UI;

namespace Game.Levels;

public partial class DeadScreen : CanvasLayer
{
    [Export]
    private PauseController _pauseController = null!;

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

            GetNode<Crosshair>("../Crosshair").HideCrosshair();
        };
    }

    public void Reload()
    {
        GetTree().ReloadCurrentScene();
    }

    public void StartOver()
    {
        GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile, "uid://5hhxiwse42yi");
    }
}

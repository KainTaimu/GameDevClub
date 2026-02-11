namespace Game;

public partial class ScrollingBackground : Node2D
{
    [Export]
    private bool _enabled = true;

    public int ScrollSpeed { get; private set; } = 800;

    // TODO: Currently unnecessary
    [ExportCategory("Components")]
    [Export]
    private TileMapLayer _tileMapLeft;

    [Export]
    private TileMapLayer _tileMapRight;

    [ExportCategory("Dying")]
    [Export]
    private int _dyingTimeSeconds = 3;

    private Viewport _viewport;

    public static ScrollingBackground Instance;

    public override void _EnterTree()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        _viewport = GetViewport();
    }

    public override void _Process(double delta)
    {
        if (!_enabled)
            return;

        if (Position.X < _viewport.GetVisibleRect().Size.X * -1)
            Position = Vector2.Zero;

        Position -= new Vector2(ScrollSpeed, 0) * (float)delta;
    }

    public void Stop()
    {
        var tween = CreateTween().SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out);
        tween.TweenMethod(
            Callable.From(
                (int x) =>
                {
                    ScrollSpeed = x;
                }
            ),
            ScrollSpeed,
            0,
            _dyingTimeSeconds
        );
    }
}

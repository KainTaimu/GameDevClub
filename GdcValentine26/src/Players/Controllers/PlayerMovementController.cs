namespace Game.Players.Controllers;

public partial class PlayerMovementController : Node
{
    [Export]
    private Player _player = null!;

    [Export]
    private PlayerStats _characterStats = null!;

    private Viewport? _viewport;

    public override void _Ready()
    {
        var viewport = GetViewport();
        if (viewport is null)
        {
            Logger.LogError("Could not get viewport.");
            return;
        }
        _viewport = viewport;
        _viewport.SizeChanged += () => _viewport = GetViewport();
    }

    public override void _Process(double delta)
    {
        PlayerMovement(delta);
    }

    private void PlayerMovement(double delta)
    {
        var up = Input.IsActionPressed(InputMapNames.MoveUp) ? 1 : 0;
        var down = Input.IsActionPressed(InputMapNames.MoveDown) ? 1 : 0;
        var left = Input.IsActionPressed(InputMapNames.MoveLeft) ? 1 : 0;
        var right = Input.IsActionPressed(InputMapNames.MoveRight) ? 1 : 0;

        float inputX = right - left;
        float inputY = down - up;

        var moveLength = (float)Math.Sqrt(inputX * inputX + inputY * inputY);

        if (moveLength > 0)
        {
            inputX /= moveLength;
            inputY /= moveLength;
        }

        var move = new Vector2(
            inputX * (_characterStats.MoveSpeed * _characterStats.MoveSpeedMultiplier),
            inputY * (_characterStats.MoveSpeed * _characterStats.MoveSpeedMultiplier)
        );
        move *= (float)delta;
        var originalPos = _player.GetPosition();

        var newPos = originalPos + move;
        _player.SetPosition(newPos);
    }
}

namespace Game.Players;

public partial class PlayerInfo : Control
{
    [Export]
    private PlayerStateController _movementController;

    [Export]
    private PlayerHealthController _statController;

    [Export]
    private Label _health;

    [Export]
    private Label _currentState;

    public override void _Process(double delta)
    {
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        _health.Text = $"Health: {_statController.Stats.Health}";
        _currentState.Text = $"CurrentState: {_movementController.CurrentState}";
    }
}

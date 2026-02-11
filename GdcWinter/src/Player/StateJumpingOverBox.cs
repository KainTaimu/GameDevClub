using System.Linq;
using Game.Obstacles;

namespace Game.Players;

public partial class StateJumpingOverBox : State
{
    [Export]
    private Player _player;

    [Export]
    private Area2D _interactionArea;

    public override void Enter()
    {
        // TODO: Get context from previous state to avoid rechecking overlaps
        var interactable = _interactionArea.GetOverlappingAreas();
        if (interactable.Count == 0)
            return;

        var interactionArea =
            interactable.OfType<ObstacleInteractionArea>().FirstOrDefault() ?? null;
        if (interactionArea is null)
        {
            Logger.LogError("Player should still be inside interactionArea at this point");
            return;
        }

        var obstacle = interactionArea.Obstacle;
        switch (obstacle)
        {
            case ObstacleCrate crate:
                _player.Position = new Vector2(_player.Position.X, crate.Top.GlobalPosition.Y);
                break;
        }
    }

    public override void Exit()
    {
        _player.Position = new Vector2(
            _player.Position.X,
            GameWorld.Instance.CurrentLevel.GroundMarker.GlobalPosition.Y
        );
    }

    public override void _Ready()
    {
        _interactionArea.AreaExited += OnLeavingInteractionArea;
    }

    private void OnLeavingInteractionArea(Area2D area)
    {
        MovementController.ChangeState<StateGrounded>();
    }
}

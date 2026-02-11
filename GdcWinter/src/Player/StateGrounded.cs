using System.Linq;
using Game.Obstacles;

namespace Game.Players;

public partial class StateGrounded : State
{
    [Export]
    private Node2D _owner;

    [Export]
    private PlayerHealthController _statController;

    [Export]
    private Area2D _interactionArea;

    [Export]
    private Area2D _collisionArea;

    private bool _hasCollided;

    public override void Enter()
    {
        _collisionArea.AreaEntered += OnObstacleCollision;
    }

    public override void Exit()
    {
        _collisionArea.AreaEntered -= OnObstacleCollision;
    }

    public override void Process(double delta)
    {
        if (Input.IsActionJustPressed(InputMapNames.MoveUp))
            Jump();
        else if (Input.IsActionJustPressed(InputMapNames.MoveDown))
        {
            Parry();
        }
    }

    private void OnObstacleCollision(Area2D collision)
    {
        MovementController.ChangeState<StateCollidedWithBox>();
    }

    private void Jump()
    {
        var interactable = _interactionArea.GetOverlappingAreas();
        if (interactable.Count == 0)
            return;

        var interactionArea =
            interactable.OfType<ObstacleInteractionArea>().FirstOrDefault() ?? null;
        if (interactionArea is null)
            return;

        switch (interactionArea.Obstacle)
        {
            case ObstacleCrate:
                MovementController.ChangeState<StateJumpingOverBox>();
                break;
            default:
                Logger.LogDebug("Unhandled interaction:", interactionArea.Obstacle.ToString());
                break;
        }
    }

    private void Parry()
    {
        MovementController.ChangeState<StateParry>();
    }
}

namespace Game.Players;

public partial class StateCollidedWithBox : State
{
    [Export]
    private Area2D _collisionArea;

    [Export]
    private PlayerHealthController _statController;

    public override void Enter()
    {
        _statController.HandleHit(1); // TODO: Custom damage by obstacle type.
    }

    public override void Exit() { }

    private void AfterCollisionWithBox(Area2D area)
    {
        MovementController.ChangeState<StateGrounded>();
    }
}

using Game.Players;

namespace Game;

public partial class State : Node, IState
{
    [Export]
    protected PlayerStateController MovementController { get; private set; }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Process(double delta) { }

    public virtual void PhysicsProcess(double delta) { }

    public override string ToString()
    {
        return $"{Name}";
    }
}

using System.Linq;
using Godot.Collections;

namespace Game.Players;

public partial class PlayerStateController : Node
{
    [Signal]
    public delegate void OnStateChangeEventHandler(State newState);

    [ExportGroup("States")]
    [Export]
    private State _initialState;

    [Export]
    private Array<State> _states = [];

    public State CurrentState { get; private set; }

    [ExportGroup("Components")]
    [Export]
    private Node2D _owner;

    [Export]
    private AnimatedSprite2D _sprite;

    [Export]
    private PlayerHealthController _statController;

    private bool _isLocked;
    private object _lockedBy;

    public override void _Ready()
    {
        _sprite.Play();
        Start(_initialState);
    }

    public override void _Process(double delta)
    {
        Update(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        PhysicsUpdate(delta);
    }

    public void Start(IState initialState)
    {
        initialState?.Enter();
        CurrentState = (State)initialState;
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    public void Update(double delta)
    {
        CurrentState?.Process(delta);
    }

    public void PhysicsUpdate(double delta)
    {
        CurrentState?.PhysicsProcess(delta);
    }

    public void ChangeState<T>()
        where T : State
    {
        var newState = _states.OfType<T>().FirstOrDefault() ?? null;
        if (newState is null)
        {
            Logger.LogError("Changing state with state type failed. State not found");
            return;
        }

        if (_isLocked)
        {
            Logger.LogWarning(
                $"Failed to change state to {newState.Name}. Locked by {_lockedBy.GetType().Name}"
            );
            return;
        }
        Logger.LogDebug("Changing state:", newState.Name);

        var old = CurrentState;
        CurrentState = newState;
        old.Exit();
        CurrentState.Enter();
        EmitSignal(SignalName.OnStateChange, CurrentState);
    }

    public void Lock(object locker)
    {
        if (locker != _lockedBy && _lockedBy is not null)
        {
            Logger.LogWarning(
                $"Attempted lock by {locker.GetType().Name} but lock is owned by {_lockedBy.GetType().Name}"
            );
            return;
        }

        _isLocked = true;
        _lockedBy = locker;
    }

    public void Unlock(object locker)
    {
        if (locker != _lockedBy && _lockedBy is not null)
        {
            Logger.LogWarning(
                $"Attempted lock by {locker.GetType().Name} but lock is owned by {_lockedBy.GetType().Name}"
            );
            return;
        }

        _isLocked = false;
        _lockedBy = null;
    }
}

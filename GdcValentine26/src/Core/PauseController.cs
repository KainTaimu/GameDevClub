namespace Game.Core;

public partial class PauseController : Node
{
    [Signal]
    public delegate void OnPausedEventHandler();

    [Signal]
    public delegate void OnUnpausedEventHandler();

    public bool IsPaused;

    public Node? LockedBy { get; private set; }
    private SceneTree Tree => GetTree();

    public void Lock(Node locker)
    {
        if (locker != LockedBy && LockedBy is not null)
        {
            return;
        }

        LockedBy = locker;
    }

    public void Unlock(Node locker)
    {
        if (locker != LockedBy && LockedBy is not null)
        {
            return;
        }

        LockedBy = null;
    }

    public void Pause(Node locker)
    {
        if (locker != LockedBy && LockedBy is not null)
        {
            return;
        }

        EmitSignal(nameof(OnPaused));
        Tree.Paused = true;
        IsPaused = Tree.Paused;
    }

    public void Unpause(Node locker)
    {
        if (locker != LockedBy && LockedBy is not null)
        {
            return;
        }

        EmitSignal(nameof(OnUnpaused));
        Tree.Paused = false;
        IsPaused = Tree.Paused;
    }
}

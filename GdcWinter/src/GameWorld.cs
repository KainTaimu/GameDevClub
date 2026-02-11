using Game.Players;

namespace Game;

public partial class GameWorld : Node
{
    [Signal]
    public delegate void OnLevelChangeEventHandler(Level newLevel);

    [Signal]
    public delegate void OnPlayerDeathEventHandler();

    public static GameWorld Instance;

    public GameWorld()
    {
        Instance = this;
        ProcessMode = ProcessModeEnum.Always;
    }

    public Level CurrentLevel
    {
        get => _currentLevel;
        set
        {
            _currentLevel = value;
            EmitSignal(SignalName.OnLevelChange, value);
        }
    }

    public Player MainPlayer
    {
        get => _mainPlayer;
        set { _mainPlayer = value; }
    }

    private Level _currentLevel;
    private Player _mainPlayer;

    public void AnnouncePlayerDead(Node caller)
    {
        Logger.LogDebug($"{caller.GetType().Name} announced player death.");
        EmitSignal(SignalName.OnPlayerDeath);
    }
}

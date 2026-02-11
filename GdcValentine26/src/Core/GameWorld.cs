using Game.Levels;
using Game.Players;

namespace Game;

public partial class GameWorld : Node
{
    [Signal]
    public delegate void OnLevelChangeEventHandler(BaseLevel newLevel);

    [Signal]
    public delegate void OnPlayerDeathEventHandler();

    public static GameWorld Instance = null!;

    public GameWorld()
    {
        Instance = this;
        ProcessMode = ProcessModeEnum.Always;
    }

    public BaseLevel? CurrentLevel
    {
        get => _currentLevel;
        set
        {
            _currentLevel = value;
            if (value is not null)
                EmitSignal(SignalName.OnLevelChange, value);
        }
    }

    public Player? MainPlayer
    {
        get => _mainPlayer;
        set { _mainPlayer = value; }
    }

    private BaseLevel? _currentLevel;
    private Player? _mainPlayer;

    public void AnnouncePlayerDead(Node caller)
    {
        Logger.LogDebug($"{caller.GetType().Name} announced player death.");
        EmitSignal(SignalName.OnPlayerDeath);
    }
}

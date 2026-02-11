using Game.Players;

namespace Game;

public partial class ScoreController : Node
{
    [Export]
    private Player _player;

    [ExportCategory("Components")]
    [Export]
    private Label _scoreLabel;

    [Export]
    private Label _timeLabel;
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            _scoreLabel.Text = "Score: " + value.ToString();
        }
    }

    private int _score;

    private GameTimeTracker _timeElapsed = new();

    public static ScoreController Instance;

    public override void _EnterTree()
    {
        Instance = this;
        AddChild(_timeElapsed);
    }

    public override void _Process(double delta)
    {
        _timeLabel.Text = $"Time: {_timeElapsed}";
    }

    protected partial class GameTimeTracker : Node
    {
        public double StartTime;

        public GameTimeTracker()
        {
            ProcessMode = ProcessModeEnum.Pausable;
        }

        public override string ToString()
        {
            var time = new TimeSpan(0, 0, 0, (int)StartTime).ToString(@"m\:ss");
            return time;
        }

        public override void _Process(double delta)
        {
            StartTime += delta;
        }
    }
}

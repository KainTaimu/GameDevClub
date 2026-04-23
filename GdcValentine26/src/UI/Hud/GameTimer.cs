namespace Game.UI;

public partial class GameTimer : Label
{
    private DateTime _startTime;

    public override void _Ready()
    {
        _startTime = DateTime.Now;
    }

    public override void _Process(double delta)
    {
        var t = DateTime.Now - _startTime;
        Text = $"Time: {(int)t.TotalMinutes:00}:{t.Seconds:00}";
    }
}

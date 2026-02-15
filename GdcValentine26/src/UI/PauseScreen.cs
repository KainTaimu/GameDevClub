using Game.Core;
using Game.Levels;

namespace Game.UI;

public partial class PauseScreen : CanvasLayer
{
    [Export]
    private PauseController _pauseController = null!;

    [Export]
    private DeadScreen _deadScreen = null!;

    [Export]
    private WinScreen? _winScreen;

    public override void _Ready()
    {
        Hide();
    }

    public override void _Input(InputEvent @event)
    {
        if (_deadScreen.Visible)
            return;

        if (_winScreen?.Visible == true)
            return;

        if (Input.IsActionJustPressed("pause"))
            if (_pauseController.IsPaused)
            {
                Hide();
                _pauseController.Unpause(this);
            }
            else
            {
                Show();
                _pauseController.Pause(this);
            }
    }

    public void Continue()
    {
        Hide();
        _pauseController.Unpause(this);
    }

    public void Quit()
    {
        GetTree().Quit();
    }
}

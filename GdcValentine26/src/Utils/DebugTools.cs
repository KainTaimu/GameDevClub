namespace Game.Utils;

public partial class DebugTools : Node
{
#if DEBUG
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey key && key.PhysicalKeycode == Key.Quoteleft)
        {
            GetTree().Quit();
            GetViewport().SetInputAsHandled();
        }
    }
#endif
}

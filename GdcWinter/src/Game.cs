namespace Game;

public partial class Game : Node
{
    public override void _EnterTree()
    {
#if DEBUG
        GetNode("/root/DebugMenu").Set("style", 2);
#endif
    }

    public override void _Process(double delta)
    {
        if (Input.IsPhysicalKeyPressed(Key.Quoteleft))
        {
            GetTree().Quit();
            return;
        }
    }
}

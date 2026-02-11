namespace Game;

public partial class Level : Node
{
    [Export]
    public Marker2D GroundMarker { get; private set; }

    [Export]
    public ScrollingBackground ScrollingBackground { get; private set; }

    public override void _Ready()
    {
        GameWorld.Instance.CurrentLevel = this;
    }
}

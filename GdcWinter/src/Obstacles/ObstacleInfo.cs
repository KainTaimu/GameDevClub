namespace Game.Obstacles;

public partial class ObstacleInfo : Label
{
    [Export]
    private Node _owner;

    public override void _Ready()
    {
        CallDeferred(Label.MethodName.SetText, _owner.Name);
    }
}

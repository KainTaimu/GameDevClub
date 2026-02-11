namespace Game;

public class Scroller(Node2D target)
{
    private readonly Node2D _targetNode = target;

    public void Scroll(double delta)
    {
        _targetNode.Position -=
            new Vector2(ScrollingBackground.Instance.ScrollSpeed, 0) * (float)delta;
    }
}

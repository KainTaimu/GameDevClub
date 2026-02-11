namespace Game.Obstacles;

public partial class Obstacle : Node2D, IObstacle
{
	[Signal]
	public delegate void OnExitEventHandler();

	[Export]
	public ObstacleType Type { get; private set; }

	public virtual void Enter() { }

	public virtual void Exit(Area2D area)
	{
		EmitSignal(SignalName.OnExit);
	}

	public override string ToString()
	{
		return $"{Name}";
	}

	protected void Scroll(double delta)
	{
		Position -= new Vector2(ScrollingBackground.Instance.ScrollSpeed, 0) * (float)delta;
	}
}

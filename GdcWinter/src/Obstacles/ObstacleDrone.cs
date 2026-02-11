namespace Game.Obstacles;

public partial class ObstacleDrone : Obstacle
{
	private const float _timeToFinalLoiterPosition = 1f;

	private float _t;

	public override void Enter() { }

	public override void Exit(Area2D area) { }

	public override void _Process(double delta)
	{
		_t += (float)delta;

		var x = _t < _timeToFinalLoiterPosition ? 500f * (float)delta : 0f;
		var y = MathF.Cos(_t) * 1.5f;

		Position = new Vector2(Position.X - x, Position.Y + y);
	}
}

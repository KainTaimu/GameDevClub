using Godot.Collections;

namespace Game.Obstacles;

public partial class ObstacleHintsController : HBoxContainer
{
	[Export]
	private Array<ArrowDirection> _directions = [];

	[Export]
	private PackedScene _arrowScene;

	public override void _Ready()
	{
		foreach (var direction in _directions)
		{
			var arrow = _arrowScene.Instantiate<Control>();
			var sprite = arrow.GetNode<Sprite2D>("Sprite2D");
			switch (direction)
			{
				case ArrowDirection.Up:
					sprite.Frame = 0;
					break;
				case ArrowDirection.Left:
					sprite.Frame = 1;
					break;
				case ArrowDirection.Down:
					sprite.Frame = 2;
					break;
				case ArrowDirection.Right:
					sprite.Frame = 3;
					break;
			}
			AddChild(arrow);
		}
	}
}

public enum ArrowDirection
{
	Up,
	Left,
	Down,
	Right,
}

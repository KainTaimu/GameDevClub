using Game.Players;

namespace Game.Pickups;

public partial class PickupPresent : Area2D, IPickupable
{
	[Export]
	private int _score = 100;

	[ExportCategory("Components")]
	[Export]
	private AnimatedSprite2D _sprite;

	private Scroller Scroller => new(this);

	private float[] Colours =
	[
		0.418f, // Blue
		0f, // Gold
		0.85f, // Red
	];

	private bool _isPickedUp;

	public override void _Ready()
	{
		RandomizeHue();
		AreaEntered += OnAreaEntered;
	}

	public override void _Process(double delta)
	{
		Scroller.Scroll(delta);
	}

	private void OnAreaEntered(Area2D area)
	{
		if (_isPickedUp)
			return;

		switch (area)
		{
			case PlayerCollisionArea:
				PickupItem();
				break;
			default:
				break;
		}
	}

	public void PickupItem()
	{
		ScoreController.Instance.Score += _score;
		_isPickedUp = true;
		var tween = CreateTween();
		tween.TweenProperty(this, "scale", Vector2.Zero, 0.2);
		tween.TweenCallback(Callable.From(QueueFree));
	}

	private void RandomizeHue()
	{
		var shader = _sprite.Material as ShaderMaterial;
		var rand = GD.RandRange(0, Colours.GetLength(0) - 1);
		shader.SetShaderParameter("hue_shift", Colours[rand]);
	}
}

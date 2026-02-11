namespace Game.Players;

public partial class StateDying : State
{
    [Export]
    private AnimatedSprite2D _sprite;

    public override void Enter()
    {
        // BUG: WTF
        // Not enough time to figure out how to scale and position the dying sprite correctly
        _sprite.Position = new Vector2(59f, -48f);
        _sprite.Scale = new Vector2(4.9f, 4.8f);

        _sprite.Animation = "dying";

        GameWorld.Instance.CurrentLevel.ScrollingBackground.Stop();
        MovementController.Lock(this);
        GameWorld.Instance.EmitSignal(GameWorld.SignalName.OnPlayerDeath);
    }

    public override void Exit()
    {
        MovementController.Unlock(this);
    }
}

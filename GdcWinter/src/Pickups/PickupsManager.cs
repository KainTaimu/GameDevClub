namespace Game.Pickups;

public partial class PickupsManager : Node
{
    [ExportCategory("Properties")]
    [Export]
    private bool _enabled = true;

    [ExportCategory("Obstacles")]
    [Export]
    private Godot.Collections.Array<PackedScene> _pickupTypes = []; // Value is amount of scene to preload

    [ExportCategory("Spawning")]
    [Export]
    private float _spawnTimeMin = 0;

    [Export]
    private float _spawnTimeMax = 1;

    [Export]
    private Node2D _groundSpawnPoint;

    private Timer _spawnTimer = new() { Autostart = true };

    public override void _Ready()
    {
        // Disable on player death
        GameWorld.Instance.OnPlayerDeath += () =>
        {
            _enabled = false;
            _spawnTimer.Stop();
        };

        var time = GD.RandRange(_spawnTimeMin, _spawnTimeMax);
        AddChild(_spawnTimer);
        _spawnTimer.Start(time);
        _spawnTimer.Timeout += SpawnPickup;
    }

    // TODO: Timer is never disabled when _!enabled. But we check if _enabled anyways.
    public void SpawnPickup()
    {
        if (!_enabled)
            return;

        var idx = GD.RandRange(0, _pickupTypes.Count - 1);
        var scene = _pickupTypes[idx];
        var pickup = scene.Instantiate<IPickupable>() as Node2D;

        pickup.Position = _groundSpawnPoint.Position;
        AddChild(pickup);
    }
}

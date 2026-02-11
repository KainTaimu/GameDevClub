using System.Collections.Generic;

namespace Game.Obstacles;

/// <summary>
/// ObstacleManager handles the random obstacles that the player must avoid.
/// </summary>
public partial class ObstacleManager : Node
{
    [ExportCategory("Properties")]
    [Export]
    private bool _enabled = true;

    /// <summary>
    /// Scenes of the obstacles that randomly spawn. Key of dictionary is the PackedScene of the obstacle
    /// and value is the amount of that scene is preloaded into the pool
    /// </summary>
    [ExportCategory("Obstacles")]
    [Export]
    private Godot.Collections.Dictionary<PackedScene, int> _obstacleTypes = []; // Value is amount of scene to preload

    [ExportCategory("Spawning")]
    [Export]
    private float _spawnTimeMin = 0;

    [Export]
    private float _spawnTimeMax = 1;

    /// <summary>
    /// The position of this marker is where ground obstacles spawn
    /// </summary>
    [Export]
    private Node2D _groundSpawnPoint;

    /// <summary>
    /// The position of this marker is where roof obstacles spawn
    /// </summary>
    [Export]
    private Node2D _roofSpawnPoint;

    /// <summary>
    /// List of obstacles not currently active and on screen
    /// </summary>
    private readonly List<Obstacle> _offObstacles = [];

    /// <summary>
    /// List of obstacle that are currently active and on screen
    /// </summary>
    private readonly Queue<Obstacle> _onObstacles = [];

    private Timer _spawnTimer = new() { Autostart = true };

    public override void _Ready()
    {
        // Disable on player death
        GameWorld.Instance.OnPlayerDeath += () =>
        {
            _enabled = false;
            _spawnTimer.Stop();
        };

        PopulatePool();

        var time = GD.RandRange(_spawnTimeMin, _spawnTimeMax);
        AddChild(_spawnTimer);
        _spawnTimer.Start(time);
        _spawnTimer.Timeout += SpawnObstacle;
    }

    // BUG: Performance issues. Shuffle is O(n) and RemoveAt is O(n) at worse case.
    public void SpawnObstacle()
    {
        if (!_enabled)
            return;
        if (_offObstacles.Count == 0)
            return;

        var idx = GD.RandRange(0, _offObstacles.Count - 1);
        var obstacle = _offObstacles[idx];
        _offObstacles.RemoveAt(idx);
        _offObstacles.Shuffle();

        _onObstacles.Enqueue(obstacle);

        obstacle.ProcessMode = ProcessModeEnum.Inherit;
        obstacle.Enter();

        _spawnTimer.WaitTime = GD.RandRange(_spawnTimeMin, _spawnTimeMax);
    }

    /// <summary>
    /// Populates the obstacle pool and sets up the obstacle's initial position, and behaviour when exiting screen
    /// </summary>
    private void PopulatePool()
    {
        foreach (var (scene, amount) in _obstacleTypes)
        {
            for (var i = 0; i < amount; i++)
            {
                var obstacle = scene.Instantiate<Obstacle>();
                // TODO: Duplicate code with OnExit
                switch (obstacle.Type)
                {
                    case ObstacleType.Ground:
                        obstacle.Position = _groundSpawnPoint.Position;
                        break;
                    case ObstacleType.Roof:
                        obstacle.Position = _roofSpawnPoint.Position;
                        break;
                    case ObstacleType.Floating:
                        obstacle.Position = new Vector2(
                            _groundSpawnPoint.Position.X,
                            GetViewport().GetVisibleRect().Size.Y / 2
                        );
                        break;
                }

                obstacle.OnExit += () =>
                {
                    // Can't disable without deferring
                    Callable
                        .From(() =>
                        {
                            obstacle.ProcessMode = ProcessModeEnum.Disabled;
                        })
                        .CallDeferred();
                    switch (obstacle.Type)
                    {
                        case ObstacleType.Ground:
                            obstacle.Position = _groundSpawnPoint.Position;
                            break;
                        case ObstacleType.Roof:
                            obstacle.Position = _roofSpawnPoint.Position;
                            break;
                        case ObstacleType.Floating:
                            obstacle.Position = new Vector2(
                                _groundSpawnPoint.Position.X,
                                GetViewport().GetVisibleRect().Size.Y / 2
                            );
                            break;
                    }
                    _onObstacles.Dequeue();
                    _offObstacles.Add(obstacle);
                };

                // Add a unique name to each object
                obstacle.Name = obstacle.Name + " " + i;

                // Disable the object processing
                Callable
                    .From(() =>
                    {
                        obstacle.ProcessMode = ProcessModeEnum.Disabled;
                    })
                    .CallDeferred();

                _offObstacles.Add(obstacle);
                AddChild(obstacle);
            }
        }
    }
}

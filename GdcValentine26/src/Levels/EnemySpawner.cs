using Game.Core;
using Game.Core.ECS;
using Game.Enemies;

namespace Game.Levels.Controllers;

public partial class EnemySpawner : Node
{
    [Export]
    public int MaxEnemyCount = 300;

    [Export]
    public float MinSpawnTime = 0.01f;

    [Export]
    public float MaxSpawnTime = 1f;

    [Export]
    private EntityComponentStore _entities = null!;

    [Export]
    private PackedScene _enemyProxyScene = null!;

    public int TotalSpawned
    {
        get;
        private set
        {
            // Spawned should not be decremented because we rely on it to create unique ids
            field =
                Math.Clamp(value, field, MaxEnemyCount);
        }
    }

    public int Alive
    {
        get;
        private set
        {

            field = Math.Clamp(value, 0, MaxEnemyCount);
        }
    }

    private double _t;

    public override void _Ready()
    {
        _entities.BeforeEntityUnregistered += (_) => Alive--;

        GameWorld.Instance.OnPlayerDeath += () => ProcessMode = ProcessModeEnum.Disabled;
    }

    public override void _Process(double delta)
    {
        _t -= delta;
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        if (_t > 0 || Alive >= MaxEnemyCount)
            return;

        _t = GD.RandRange(MinSpawnTime, MaxSpawnTime);

        for (var i = 0; i < 1; i++)
        {
            var pos = GetPositionOutsideViewport();

            // BUG:
            // PRONE TO ID COLLISIONS. Not handling
            var id = (int)GD.Randi();

            if (!_entities.RegisterEntity(id))
                continue;

            var enemyProxyNode = _enemyProxyScene.Instantiate<EnemyECSProxy>();
            enemyProxyNode.Id = id;

            _entities.RegisterComponent(id, new NodeProxyComponent(enemyProxyNode));
            _entities.RegisterComponent(id, new EntityTypeComponent(EntityType.Enemy));
            _entities.RegisterComponent(id, new PositionComponent(pos, true));

            TotalSpawned++;
            Alive++;

            AddChild(enemyProxyNode);
        }
    }

    private Vector2 GetPositionOutsideViewport()
    {
        var viewport = GetViewport().GetCamera2D();
        var screenCenterPosition = viewport.GetScreenCenterPosition();
        var viewportRectEnd = viewport.GetViewportRect().Size;

        const float margin = 100;
        var spawnVector = new Vector2();

        var seed = GD.RandRange(0, 3);

        switch (seed)
        {
            case 0: // TOP
                spawnVector.X = (float)
                    GD.RandRange(
                        screenCenterPosition.X - viewportRectEnd.X / 2 - margin,
                        screenCenterPosition.X + viewportRectEnd.X / 2 + margin
                    );
                spawnVector.Y = screenCenterPosition.Y - viewportRectEnd.Y / 2 - margin;
                break;

            case 1: // BOTTOM
                spawnVector.X = (float)
                    GD.RandRange(
                        screenCenterPosition.X - viewportRectEnd.X / 2 - margin,
                        screenCenterPosition.X + viewportRectEnd.X / 2 + margin
                    );
                spawnVector.Y = screenCenterPosition.Y + viewportRectEnd.Y / 2 + margin;
                break;

            case 2: // LEFT
                spawnVector.X = screenCenterPosition.X - viewportRectEnd.X / 2 - margin;
                spawnVector.Y = (float)
                    GD.RandRange(
                        screenCenterPosition.Y - viewportRectEnd.Y / 2 - margin,
                        screenCenterPosition.Y + viewportRectEnd.Y / 2 + margin
                    );
                break;

            case 3: // RIGHT
                spawnVector.X = screenCenterPosition.X + viewportRectEnd.X / 2 + margin;
                spawnVector.Y = (float)
                    GD.RandRange(
                        screenCenterPosition.Y - viewportRectEnd.Y / 2 - margin,
                        screenCenterPosition.Y + viewportRectEnd.Y / 2 + margin
                    );
                break;
        }

        return spawnVector;
    }
}

using System.Collections.Generic;

namespace Game.Levels;

public partial class EnemyTracker : Node
{
    [Signal]
    public delegate void OnEnemyAddedEventHandler(Node2D enemy);

    [Signal]
    public delegate void OnEnemyRemovedEventHandler(Node2D enemy);

    public readonly List<Node2D> Enemies = [];

    public void AddEnemy(Node2D enemy)
    {
        EmitSignal(SignalName.OnEnemyAdded);
        Enemies.Add(enemy);

        if (Enemies.Count % 10 == 0)
            Logger.LogDebug(Enemies.Count);
    }

    public void RemoveEnemy(Node2D enemy)
    {
        EmitSignal(SignalName.OnEnemyRemoved);
        Enemies.Remove(enemy);
    }
}

namespace Game.Obstacles;

public interface IObstacle
{
    /// <summary>
    /// Resets this obstacle's state
    /// </summary>
    void Enter();
    void Exit(Area2D area);
}

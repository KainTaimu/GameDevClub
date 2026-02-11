public interface IState
{
    void Enter();
    void Exit();
    void Process(double delta);
    void PhysicsProcess(double delta);
}

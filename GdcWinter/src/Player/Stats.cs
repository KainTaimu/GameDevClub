[GlobalClass]
public partial class Stats : Resource
{
    public int Health
    {
        get => _health;
        set { _health = Math.Clamp(value, 0, int.MaxValue); }
    }

    [Export]
    public int MaxHealth = 3;

    private int _health;

    public void Initialize()
    {
        _health = MaxHealth;
    }
}

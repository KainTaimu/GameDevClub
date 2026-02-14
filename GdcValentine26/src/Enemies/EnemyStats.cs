namespace Game.Enemies;

public partial class EnemyStats : Node
{
    private int _health;
    private int _damage = 10;
    private int _maxHealth = 10;
    private float _moveSpeed = 120;

    public int Health
    {
        get => _health;
        set
        {
            var clamped = Math.Clamp(value, 0, _maxHealth);
            if (_health == clamped)
                return;
            _health = clamped;
        }
    }

    [Export(PropertyHint.Range, "0,0,or_greater,hide_slider")]
    public required int MaxHealth
    {
        get => _maxHealth;
        set
        {
            var clamped = Math.Clamp(value, 0, int.MaxValue);
            if (_maxHealth == clamped)
                return;
            _maxHealth = clamped;

            if (_health > _maxHealth)
                Health = _maxHealth;
        }
    }

    [Export(PropertyHint.Range, "0,0,or_greater,hide_slider")]
    public required int Damage
    {
        get => _damage;
        set
        {
            var clamped = Math.Clamp(value, 0, int.MaxValue);
            if (_damage == clamped)
                return;
            _damage = clamped;
        }
    }

    [Export(PropertyHint.Range, "0,0,or_greater,hide_slider")]
    public required float MoveSpeed
    {
        get => _moveSpeed;
        set
        {
            var clamped = Math.Max(value, 0);
            if (_moveSpeed == clamped)
                return;
            _moveSpeed = clamped;
        }
    }

    public override void _Ready()
    {
        Health = MaxHealth;
    }
}

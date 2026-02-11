namespace Game;

/// Using basic strings on Godot methods that take a StringName object results in a heap allocation everytime.
/// <summary>Contains StringNames of all input maps.</summary>
public static class InputMapNames
{
    public static StringName MoveUp { get; private set; } = "move_up";
    public static StringName MoveDown { get; private set; } = "move_down";
    public static StringName MoveLeft { get; private set; } = "move_left";
    public static StringName MoveRight { get; private set; } = "move_right";
    public static StringName PrimaryAttack { get; private set; } = "primary_attack";
    public static StringName SecondaryAttack { get; private set; } = "secondary_attack";
    public static StringName GunReload { get; private set; } = "gun_reload";
}

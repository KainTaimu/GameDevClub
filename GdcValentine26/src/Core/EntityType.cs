namespace Game.Core;

[Flags]
public enum EntityType
{
    None = 1,
    Player = 1 << 1,
    Enemy = 1 << 2,
    Breakable = 1 << 3,
}

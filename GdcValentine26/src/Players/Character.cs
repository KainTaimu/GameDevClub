namespace Game.Players;

public partial class Character : Node
{
    [Export]
    public string CharacterName = "";

    // PlayerStats should never be unset from its exported value!
    [Export]
    public PlayerStats PlayerStats { get; private set; } = null!;
}

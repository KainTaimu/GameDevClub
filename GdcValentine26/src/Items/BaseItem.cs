namespace Game.Items;

public partial class BaseItem : Node
{
    [Export]
    public ItemProperties Properties { get; private set; } = null!;
}

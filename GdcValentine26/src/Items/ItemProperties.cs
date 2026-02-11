namespace Game.Items;

[GlobalClass]
public partial class ItemProperties : Resource
{
    [Export]
    public string Name = "PLACEHOLDER";

    [Export]
    public ItemType ItemType;

    public int CurrentLevel;

    [Export(PropertyHint.MultilineText)]
    public string Description = "PLACEHOLDER";
}

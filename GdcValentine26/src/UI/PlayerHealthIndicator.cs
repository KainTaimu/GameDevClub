using System.Collections.Generic;
using System.Linq;
using Game.Players;

namespace Game.UI;

public partial class PlayerHealthIndicator : HBoxContainer
{
    [Export]
    private Texture2D _heartTexture = null!;

    [Export]
    private Player _player = null!;

    private readonly List<TextureRect> _hearts = [];

    public override void _Ready()
    {
        for (var i = 0; i < _player.Character.PlayerStats.MaxHealth; i++)
        {
            CreateHeart();
        }
        _player.Character.PlayerStats.OnHealthChanged += (_, _) => HandleOnPlayerHit();
    }

    private void HandleOnPlayerHit()
    {
        var last = _hearts.LastOrDefault();
        if (last is null)
            return;
        _hearts.Remove(last);
        last.QueueFree();
    }

    private void CreateHeart()
    {
        var rect = new TextureRect()
        {
            Texture = _heartTexture,
            ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional,
        };
        AddChild(rect);
        _hearts.Add(rect);
    }
}

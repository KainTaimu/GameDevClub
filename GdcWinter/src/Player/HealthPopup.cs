using System.Collections.Generic;
using System.Linq;

namespace Game.Players;

public partial class HealthPopup : HBoxContainer
{
    [Export]
    private Texture2D _heartTexture;

    [Export]
    private PlayerHealthController _healthController;

    private readonly List<TextureRect> _hearts = [];

    public override void _Ready()
    {
        for (var i = 0; i < _healthController.Stats.Health; i++)
        {
            CreateHeart();
        }
        _healthController.OnPlayerHit += HandleOnPlayerHit;
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

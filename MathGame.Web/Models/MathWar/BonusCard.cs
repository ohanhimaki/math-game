namespace MathGame.Web.Models.MathWar;

public enum BonusRarity { Common, Rare, Legendary }

public class BonusCard
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Emoji { get; set; } = "🃏";
    public BonusRarity Rarity { get; set; } = BonusRarity.Common;
    public bool IsUnique { get; set; }
    
    public Action<RunSettings, GameState>? Effect { get; set; }
}

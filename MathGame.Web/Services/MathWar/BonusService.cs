using MathGame.Web.Models.MathWar;

namespace MathGame.Web.Services.MathWar;

public class BonusService
{
    private readonly Random _random = new();

    public List<BonusCard> GetBonusPool(string medal, List<BonusCard> collected)
    {
        var allPossible = GetLibrary()
            .Where(b => !b.IsUnique || !collected.Any(c => c.Id == b.Id))
            .ToList();

        var pool = new List<BonusCard>();
        int cardsToPick = 3;

        for (int i = 0; i < cardsToPick; i++)
        {
            if (!allPossible.Any()) break;

            // Determine rarity based on medal and luck
            var rarity = PickRarity(medal);
            var filteredByRarity = allPossible.Where(b => b.Rarity == rarity).ToList();
            
            // Fallback if no cards of that rarity remain
            if (!filteredByRarity.Any()) filteredByRarity = allPossible;

            var pick = filteredByRarity[_random.Next(filteredByRarity.Count)];
            pool.Add(pick);
            allPossible.Remove(pick); // Avoid duplicates in the same pool
        }

        return pool;
    }

    private BonusRarity PickRarity(string medal)
    {
        int roll = _random.Next(1, 101);
        return medal switch
        {
            "Gold" => roll > 60 ? BonusRarity.Legendary : (roll > 20 ? BonusRarity.Rare : BonusRarity.Common),
            "Silver" => roll > 85 ? BonusRarity.Legendary : (roll > 50 ? BonusRarity.Rare : BonusRarity.Common),
            _ => roll > 95 ? BonusRarity.Rare : BonusRarity.Common
        };
    }

    public List<BonusCard> GetLibrary() => new List<BonusCard>
    {
        // COMMON
        new BonusCard {
            Id = "turtle", Title = "Kilpikonna", Emoji = "🐢", Rarity = BonusRarity.Common,
            Description = "Nopeus -15%",
            Effect = (s, g) => s.SpeedMultiplier *= 0.85
        },
        new BonusCard {
            Id = "factory", Title = "Porttitehdas", Emoji = "🏭", Rarity = BonusRarity.Common,
            Description = "+2 porttia joka tasoon",
            Effect = (s, g) => s.GateCountModifier += 2
        },
        new BonusCard {
            Id = "shield", Title = "Suojakilpi", Emoji = "🛡️", Rarity = BonusRarity.Common,
            Description = "Mitalirajat -5%",
            Effect = (s, g) => s.ThresholdDifficulty *= 0.95
        },

        // RARE
        new BonusCard {
            Id = "multiplier_pro", Title = "Kertoimien Mestari", Emoji = "🎯", Rarity = BonusRarity.Rare, IsUnique = true,
            Description = "Kaikki kertolaskut +20%",
            Effect = (s, g) => s.MultiplyMultiplier *= 1.2
        },
        new BonusCard {
            Id = "safety_net", Title = "Turvaverkko", Emoji = "🕸️", Rarity = BonusRarity.Rare, IsUnique = true,
            Description = "Mitalirajat -15%",
            Effect = (s, g) => s.ThresholdDifficulty *= 0.85
        },

        // LEGENDARY
        new BonusCard {
            Id = "midas", Title = "Midas", Emoji = "💎", Rarity = BonusRarity.Legendary, IsUnique = true,
            Description = "Kaikki kertolaskut +25%",
            Effect = (s, g) => s.MultiplyMultiplier *= 1.25
        },
        new BonusCard {
            Id = "no_div", Title = "Ei Jakoja", Emoji = "🚫", Rarity = BonusRarity.Legendary, IsUnique = true,
            Description = "Poistaa ÷2 portit",
            Effect = (s, g) => s.NoDivisions = true
        }
    };
}

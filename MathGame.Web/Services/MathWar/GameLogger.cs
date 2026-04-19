using MathGame.Web.Models.MathWar;

namespace MathGame.Web.Services.MathWar;

public class GameLogger
{
    private readonly List<string> _entries = new();
    private DateTime _sessionStart;

    public bool HasLog => _entries.Count > 0;

    public void StartSession()
    {
        _entries.Clear();
        _sessionStart = DateTime.UtcNow;
        _entries.Add($"=== MathWar Session {_sessionStart:yyyy-MM-dd HH:mm:ss} UTC ===");
    }

    public void LogLevelStart(int level, int startValue, int gateCount, IEnumerable<BonusCard> bonuses, RunSettings settings, string tierSuffix)
    {
        var bonusStr = bonuses.Any() ? string.Join(", ", bonuses.Select(b => b.Title)) : "–";
        Log($"[TASO {level} ALKU] Arvo={startValue}{tierSuffix}, Portit={gateCount}, Bonukset=[{bonusStr}], " +
            $"Nopeus={settings.CurrentScrollSpeed:F2}, Kertoin={settings.MultiplyMultiplier:F2}");
    }

    public void LogGateHit(int level, int before, string operation, int after, string tierSuffix)
    {
        Log($"[TASO {level}] Portti: {before}{tierSuffix} {operation} = {after}{tierSuffix}");
    }

    public void LogLevelEnd(int level, int finalValue, string rating, int minPossible, int maxPossible, int bronze, int silver, int gold)
    {
        Log($"[TASO {level} LOPPU] Arvo={finalValue}, Mitali={rating}, " +
            $"Min={minPossible}, Max={maxPossible}, Rajat=[🥉{bronze} 🥈{silver} 🥇{gold}]");
    }

    public void LogBonusSelected(int level, string bonusTitle, string bonusDescription)
    {
        Log($"[TASO {level} BONUS] Valittu: {bonusTitle} – {bonusDescription}");
    }

    public void LogBonusSkipped(int level)
    {
        Log($"[TASO {level} BONUS] Ohitettu (+50 sotilasta)");
    }

    public void LogGameOver(int level, int finalValue, string rating)
    {
        Log($"[PELI OHI] Taso={level}, Arvo={finalValue}, Mitali={rating}");
        _entries.Add("=== Peli päättyi ===");
    }

    public string GetFullLog() => string.Join("\n", _entries);

    private void Log(string message) =>
        _entries.Add($"[+{(DateTime.UtcNow - _sessionStart).TotalSeconds:F1}s] {message}");
}

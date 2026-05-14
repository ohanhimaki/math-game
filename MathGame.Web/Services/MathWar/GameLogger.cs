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

    public void LogLevelStart(int level, int startValue, int gateCount, IEnumerable<BonusCard> bonuses, RunSettings settings)
    {
        var bonusStr = bonuses.Any() ? string.Join(", ", bonuses.Select(b => b.Title)) : "–";
        Log($"[TASO {level} ALKU] Arvo={Fmt(startValue)}, Portit={gateCount}, Bonukset=[{bonusStr}], " +
            $"Nopeus={settings.CurrentScrollSpeed:F2}, Kertoin={settings.MultiplyMultiplier:F2}");
    }

    public void LogGateHit(int level, int before, string operation, int after)
    {
        Log($"[TASO {level}] Portti: {Fmt(before)} {operation} = {Fmt(after)}");
    }

    public void LogLevelEnd(int level, int finalValue, string rating, int minPossible, int maxPossible, int bronze, int silver, int gold)
    {
        Log($"[TASO {level} LOPPU] Arvo={Fmt(finalValue)}, Mitali={rating}, " +
            $"Min={Fmt(minPossible)}, Max={Fmt(maxPossible)}, Rajat=[🥉{Fmt(bronze)} 🥈{Fmt(silver)} 🥇{Fmt(gold)}]");
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
        Log($"[PELI OHI] Taso={level}, Arvo={Fmt(finalValue)}, Mitali={rating}");
        _entries.Add("=== Peli päättyi ===");
    }

    public string GetFullLog() => string.Join("\n", _entries);

    private void Log(string message) =>
        _entries.Add($"[+{(DateTime.UtcNow - _sessionStart).TotalSeconds:F1}s] {message}");

    private static string Fmt(int v)
    {
        int abs = Math.Abs(v);
        string sign = v < 0 ? "−" : "";
        if (abs >= 1_000_000_000) return $"{sign}{abs / 1_000_000_000.0:F1}B";
        if (abs >= 1_000_000)     return $"{sign}{abs / 1_000_000.0:F1}M";
        if (abs >= 10_000)        return $"{sign}{abs / 1_000}k";
        return v.ToString();
    }
}

using MathGame.Web.Models.MathWar;

namespace MathGame.Web.Services.MathWar;

public static class LevelLibrary
{
    private static readonly LevelConfig[] _defined =
    [
        // 1: intro – only +/-,  2 gates, 2 segments
        new() { StartValue = 100,  GateCount = 2, MaxSegments = 2, AllowMultiply = false, AllowDivide = false },
        // 2: more lanes – only +/-, 3 gates, 3 segments
        new() { StartValue = 100,  GateCount = 3, MaxSegments = 3, AllowMultiply = false, AllowDivide = false },
        // 3: × and ÷ introduced, 2 gates (easy exposure)
        new() { StartValue = 100,  GateCount = 2, MaxSegments = 3, AllowMultiply = true,  AllowDivide = true  },
        // 4: full ops, more gates
        new() { StartValue = 100,  GateCount = 5, MaxSegments = 4, AllowMultiply = true,  AllowDivide = true  },
        // 5: start from negative – escape theme
        new() { StartValue = -500, GateCount = 5, MaxSegments = 4, AllowMultiply = true,  AllowDivide = true,
                Theme = "⚠️ Miinukselta!" },
    ];

    public static LevelConfig Get(int level)
    {
        if (level >= 1 && level <= _defined.Length)
            return _defined[level - 1];

        // Procedural levels after the curated ones
        int extra = level - _defined.Length;
        return new LevelConfig
        {
            StartValue   = 100,
            GateCount    = Math.Min(5 + extra, 12),
            MaxSegments  = Math.Min(3 + extra / 2, 5),
            AllowMultiply = true,
            AllowDivide  = true
        };
    }
}

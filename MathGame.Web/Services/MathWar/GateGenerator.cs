using MathGame.Web.Models.MathWar;

namespace MathGame.Web.Services.MathWar;

public class GateGenerator
{
    private readonly Random _random = new();

    public List<Gate> Generate(LevelConfig config, int gameLevel, RunSettings settings)
    {
        int count = GateCount(config, settings);
        int scaleBase = Math.Max(Math.Abs(config.StartValue), 10);
        var gates = new List<Gate>();
        for (int i = 0; i < count; i++)
        {
            gates.Add(new Gate
            {
                Segments = GenerateSegments(config, gameLevel, settings, scaleBase),
                YPosition = -20.0 - i * 30.0
            });
        }
        return gates;
    }

    public int GateCount(LevelConfig config, RunSettings settings) =>
        Math.Min(config.GateCount + settings.GateCountModifier, 15);

    private List<GateSegment> GenerateSegments(LevelConfig config, int gameLevel, RunSettings settings, int scaleBase)
    {
        int segCount = _random.Next(2, config.MaxSegments + 1);

        var splitPoints = new SortedSet<int>();
        while (splitPoints.Count < segCount - 1)
            splitPoints.Add(_random.Next(1, 6));

        var boundaries = new List<int> { 0 };
        boundaries.AddRange(splitPoints);
        boundaries.Add(6);

        var segments = new List<GateSegment>();
        for (int i = 0; i < boundaries.Count - 1; i++)
        {
            segments.Add(new GateSegment
            {
                StartLane = boundaries[i],
                EndLane   = boundaries[i + 1] - 1,
                Operation = RandomOperation(config, gameLevel, settings, scaleBase)
            });
        }
        return segments;
    }

    private Operation RandomOperation(LevelConfig config, int gameLevel, RunSettings settings, int scaleBase)
    {
        int multiplyWeight = config.AllowMultiply ? Math.Min(1 + gameLevel / 2, 3) : 0;
        int divideWeight   = (config.AllowDivide && !settings.NoDivisions) ? 1 : 0;
        int total = 2 + multiplyWeight + divideWeight;

        int roll = _random.Next(0, total);
        if (roll == 0) return Add(scaleBase);
        if (roll == 1) return Subtract(scaleBase);
        if (roll < 2 + multiplyWeight) return Multiply(gameLevel, settings);
        return new Operation { Formula = "x/2", Label = "÷2" };
    }

    private Operation Add(int scaleBase)
    {
        // 5–20% of level start value (abs), min 5
        double pct = 0.05 + _random.NextDouble() * 0.15;
        int n = Math.Max((int)(scaleBase * pct), 5);
        return new Operation { Formula = $"x+{n}", Label = $"+{n}" };
    }

    private Operation Subtract(int scaleBase)
    {
        // 3–12% of level start value (abs), min 3
        double pct = 0.03 + _random.NextDouble() * 0.09;
        int n = Math.Max((int)(scaleBase * pct), 3);
        return new Operation { Formula = $"x-{n}", Label = $"-{n}" };
    }

    private Operation Multiply(int gameLevel, RunSettings settings)
    {
        // Max ×2 base — ×3 and ×4 removed for balance
        var options = new List<(string f, string l, double baseVal)>
        {
            ("x*1.5", "×1.5", 1.5),
            ("x*2",   "×2",   2.0)
        };

        var opt = options[_random.Next(options.Count)];

        if (settings.MultiplyMultiplier != 1.0)
        {
            var newVal = opt.baseVal * settings.MultiplyMultiplier;
            return new Operation
            {
                Formula = $"x*{newVal.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)}",
                Label   = $"×{newVal.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)}"
            };
        }

        return new Operation { Formula = opt.f, Label = opt.l };
    }
}

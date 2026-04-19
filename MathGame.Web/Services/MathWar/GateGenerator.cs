using MathGame.Web.Models.MathWar;

namespace MathGame.Web.Services.MathWar;

public class GateGenerator
{
    private readonly Random _random = new();

    public List<Gate> Generate(int count, int level, RunSettings settings, int startValue, string tierSuffix)
    {
        var gates = new List<Gate>();
        for (int i = 0; i < count; i++)
        {
            gates.Add(new Gate
            {
                Segments = GenerateSegments(level, settings, startValue, tierSuffix),
                YPosition = -20.0 - i * 30.0
            });
        }
        return gates;
    }

    public int GateCount(int level, RunSettings settings) => 
        Math.Min(5 + (level - 1) + settings.GateCountModifier, 15);

    private List<GateSegment> GenerateSegments(int level, RunSettings settings, int startValue, string tierSuffix)
    {
        int maxSegments = Math.Min(2 + (level / 2), 5);
        int segCount = _random.Next(2, maxSegments + 1);

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
                EndLane = boundaries[i + 1] - 1,
                Operation = RandomOperation(level, settings, startValue, tierSuffix)
            });
        }
        return segments;
    }

    private Operation RandomOperation(int level, RunSettings settings, int startValue, string tierSuffix)
    {
        int multiplyWeight = Math.Min(1 + level / 2, 3);
        int divideWeight = settings.NoDivisions ? 0 : 1;
        int total = 2 + multiplyWeight + divideWeight;

        int roll = _random.Next(0, total);
        if (roll == 0) return Add(startValue, tierSuffix);
        if (roll == 1) return Subtract(startValue, tierSuffix);
        if (roll < 2 + multiplyWeight) return Multiply(level, settings);
        return new Operation { Formula = "x/2", Label = "÷2" };
    }

    private Operation Add(int startValue, string tierSuffix)
    {
        // 5–20% of start value, min 5
        double pct = 0.05 + _random.NextDouble() * 0.15;
        int n = Math.Max((int)(startValue * pct), 5);
        return new Operation { Formula = $"x+{n}", Label = $"+{n}{tierSuffix}" };
    }

    private Operation Subtract(int startValue, string tierSuffix)
    {
        // 3–12% of start value, min 3
        double pct = 0.03 + _random.NextDouble() * 0.09;
        int n = Math.Max((int)(startValue * pct), 3);
        return new Operation { Formula = $"x-{n}", Label = $"-{n}{tierSuffix}" };
    }

    private Operation Multiply(int level, RunSettings settings)
    {
        // Max ×2 base — higher multipliers (×3, ×4) removed for balance
        var options = new List<(string f, string l, double baseVal)>
        {
            ("x*1.5", "×1.5", 1.5),
            ("x*2",   "×2",   2.0)
        };

        var opt = options[_random.Next(options.Count)];

        if (settings.MultiplyMultiplier != 1.0)
        {
            var newVal = opt.baseVal * settings.MultiplyMultiplier;
            return new Operation {
                Formula = $"x*{newVal.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)}",
                Label   = $"×{newVal.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)}"
            };
        }

        return new Operation { Formula = opt.f, Label = opt.l };
    }
}

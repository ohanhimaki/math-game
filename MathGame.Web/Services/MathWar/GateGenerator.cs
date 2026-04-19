using MathGame.Web.Models.MathWar;

namespace MathGame.Web.Services.MathWar;

public class GateGenerator
{
    private readonly Random _random = new();

    public List<Gate> Generate(int count, int level, RunSettings settings)
    {
        var gates = new List<Gate>();
        for (int i = 0; i < count; i++)
        {
            gates.Add(new Gate
            {
                Segments = GenerateSegments(level, settings),
                YPosition = -20.0 - i * 30.0
            });
        }
        return gates;
    }

    public int GateCount(int level, RunSettings settings) => 
        Math.Min(5 + (level - 1) + settings.GateCountModifier, 15);

    private List<GateSegment> GenerateSegments(int level, RunSettings settings)
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
                Operation = RandomOperation(level, settings)
            });
        }
        return segments;
    }

    private Operation RandomOperation(int level, RunSettings settings)
    {
        int multiplyWeight = Math.Min(1 + level / 2, 3);
        int divideWeight = settings.NoDivisions ? 0 : 1;
        int total = 2 + multiplyWeight + divideWeight;

        int roll = _random.Next(0, total);
        if (roll == 0) return Add(level);
        if (roll == 1) return Subtract(level);
        if (roll < 2 + multiplyWeight) return Multiply(level, settings);
        return new Operation { Formula = "x/2", Label = "÷2" };
    }

    private Operation Add(int level)
    {
        int max = Math.Min(10 + level * 15, 100);
        var n = _random.Next(10, max + 1);
        return new Operation { Formula = $"x+{n}", Label = $"+{n}" };
    }

    private Operation Subtract(int level)
    {
        int max = Math.Min(10 + level * 10, 60);
        var n = _random.Next(10, max + 1);
        return new Operation { Formula = $"x-{n}", Label = $"-{n}" };
    }

    private Operation Multiply(int level, RunSettings settings)
    {
        var options = new List<(string f, string l, double baseVal)> { 
            ("x*1.5", "×1.5", 1.5), 
            ("x*2", "×2", 2.0) 
        };
        if (level >= 3) options.Add(("x*3", "×3", 3.0));
        if (level >= 5) options.Add(("x*4", "×4", 4.0));
        
        var opt = options[_random.Next(options.Count)];
        
        // Apply multiplier bonus if active
        if (settings.MultiplyMultiplier != 1.0)
        {
            var newVal = opt.baseVal * settings.MultiplyMultiplier;
            return new Operation { 
                Formula = $"x*{newVal.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)}", 
                Label = $"×{newVal.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)}" 
            };
        }

        return new Operation { Formula = opt.f, Label = opt.l };
    }
}

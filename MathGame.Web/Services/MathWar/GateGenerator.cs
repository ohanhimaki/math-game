using MathGame.Web.Models.MathWar;

namespace MathGame.Web.Services.MathWar;

public class GateGenerator
{
    private readonly Random _random = new();

    public List<Gate> Generate(int count, int level = 1)
    {
        var gates = new List<Gate>();
        for (int i = 0; i < count; i++)
        {
            gates.Add(new Gate
            {
                Segments = GenerateSegments(level),
                YPosition = -20.0 - i * 30.0
            });
        }
        return gates;
    }

    public int GateCount(int level = 1) => Math.Min(5 + (level - 1), 10);

    private List<GateSegment> GenerateSegments(int level)
    {
        // Higher levels allow more segments
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
                Operation = RandomOperation(level)
            });
        }
        return segments;
    }

    private Operation RandomOperation(int level)
    {
        // Higher levels: heavier multiply weight, larger numbers
        int multiplyWeight = Math.Min(1 + level / 2, 3);
        int total = 2 + multiplyWeight + 1; // add, subtract, multiply×weight, divide

        int roll = _random.Next(0, total);
        if (roll == 0) return Add(level);
        if (roll == 1) return Subtract(level);
        if (roll < 2 + multiplyWeight) return Multiply(level);
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

    private Operation Multiply(int level)
    {
        // Level 1-2: ×1.5/×2; level 3+: also ×3; level 5+: also ×4
        var options = new List<(string f, string l)> { ("x*1.5", "×1.5"), ("x*2", "×2") };
        if (level >= 3) options.Add(("x*3", "×3"));
        if (level >= 5) options.Add(("x*4", "×4"));
        var (f, l) = options[_random.Next(options.Count)];
        return new Operation { Formula = f, Label = l };
    }
}

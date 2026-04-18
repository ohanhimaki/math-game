using MathGame.Web.Models.MathWar;

namespace MathGame.Web.Services.MathWar;

public class GateGenerator
{
    private readonly Random _random = new();

    public List<Gate> Generate(int count)
    {
        var gates = new List<Gate>();
        for (int i = 0; i < count; i++)
        {
            gates.Add(new Gate
            {
                Segments = GenerateSegments(),
                YPosition = -20.0 - i * 30.0
            });
        }
        return gates;
    }

    public int GateCount() => _random.Next(5, 8);

    private List<GateSegment> GenerateSegments()
    {
        // 2–4 segments, randomly partition 6 lanes
        int segCount = _random.Next(2, 5);

        var splitPoints = new SortedSet<int>();
        while (splitPoints.Count < segCount - 1)
            splitPoints.Add(_random.Next(1, 6)); // valid split points: 1–5

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
                Operation = RandomOperation()
            });
        }
        return segments;
    }

    private Operation RandomOperation()
    {
        return _random.Next(0, 5) switch
        {
            0 => Add(),
            1 => Subtract(),
            2 => new Operation { Formula = "x*1.5", Label = "×1.5" },
            3 => new Operation { Formula = "x*2",   Label = "×2" },
            _ => new Operation { Formula = "x/2",   Label = "÷2" },
        };
    }

    private Operation Add()
    {
        var n = _random.Next(10, 51);
        return new Operation { Formula = $"x+{n}", Label = $"+{n}" };
    }

    private Operation Subtract()
    {
        var n = _random.Next(10, 31);
        return new Operation { Formula = $"x-{n}", Label = $"-{n}" };
    }
}

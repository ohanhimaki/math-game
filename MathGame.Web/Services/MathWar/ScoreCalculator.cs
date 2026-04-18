using MathGame.Web.Models.MathWar;

namespace MathGame.Web.Services.MathWar;

public class ScoreCalculator
{
    public (int Min, int Max) SimulateMinMax(int startValue, List<Gate> gates)
    {
        return Recurse(startValue, 0, gates);
    }

    public int SimulateMax(int startValue, List<Gate> gates)
    {
        return SimulateMinMax(startValue, gates).Max;
    }

    private (int Min, int Max) Recurse(int value, int index, List<Gate> gates)
    {
        if (index >= gates.Count)
            return (value, value);

        int min = int.MaxValue, max = int.MinValue;
        foreach (var seg in gates[index].Segments)
        {
            var next = seg.Operation.Apply(value);
            var (segMin, segMax) = Recurse(next, index + 1, gates);
            min = Math.Min(min, segMin);
            max = Math.Max(max, segMax);
        }
        return (min, max);
    }

    public string GetRating(int finalValue, int min, int max)
    {
        if (max == min) return "Gold";
        var (bronze, silver, gold) = GetThresholds(min, max);
        if (finalValue >= gold)   return "Gold";
        if (finalValue >= silver) return "Silver";
        if (finalValue >= bronze) return "Bronze";
        return "–";
    }

    public (int Bronze, int Silver, int Gold) GetThresholds(int min, int max)
    {
        if (max == min) return (min, min, min);
        return (
            (int)(min + 0.5  * (max - min)),
            (int)(min + 0.75 * (max - min)),
            (int)(min + 0.9  * (max - min))
        );
    }
}

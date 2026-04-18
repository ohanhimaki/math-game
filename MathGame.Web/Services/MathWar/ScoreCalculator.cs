using MathGame.Web.Models.MathWar;

namespace MathGame.Web.Services.MathWar;

public class ScoreCalculator
{
    public (int Min, int Max) SimulateMinMax(int startValue, List<Gate> gates)
    {
        return Recurse(startValue, 0, gates);
    }

    private (int Min, int Max) Recurse(int value, int index, List<Gate> gates)
    {
        if (index >= gates.Count)
            return (value, value);

        var gate = gates[index];
        var a = gate.OptionA.Apply(value);
        var b = gate.OptionB.Apply(value);

        var (minA, maxA) = Recurse(a, index + 1, gates);
        var (minB, maxB) = Recurse(b, index + 1, gates);

        return (Math.Min(minA, minB), Math.Max(maxA, maxB));
    }

    public string GetRating(int finalValue, int min, int max)
    {
        if (max == min) return "Gold";
        double bronze = min + 0.4 * (max - min);
        double silver = min + 0.7 * (max - min);
        double gold   = min + 0.9 * (max - min);
        if (finalValue >= gold)   return "Gold";
        if (finalValue >= silver) return "Silver";
        if (finalValue >= bronze) return "Bronze";
        return "–";
    }
}

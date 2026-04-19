using MathGame.Web.Models.MathWar;

namespace MathGame.Web.Services.MathWar;

public class ScoreCalculator
{
    // All operations are monotone-increasing in x, so min/max can be computed
    // greedily in O(N*S) instead of the exponential O(S^N) recursive approach.
    public (int Min, int Max) SimulateMinMax(int startValue, List<Gate> gates)
    {
        int minVal = startValue;
        int maxVal = startValue;

        foreach (var gate in gates)
        {
            maxVal = gate.Segments.Max(s => s.Operation.Apply(maxVal));
            minVal = gate.Segments.Min(s => s.Operation.Apply(minVal));
        }

        return (minVal, maxVal);
    }

    public int SimulateMax(int startValue, List<Gate> gates)
    {
        return SimulateMinMax(startValue, gates).Max;
    }

    public string GetRating(int finalValue, int min, int max, RunSettings settings)
    {
        if (max == min) return "Gold";
        var (bronze, silver, gold) = GetThresholds(min, max, settings);
        if (finalValue >= gold)   return "Gold";
        if (finalValue >= silver) return "Silver";
        if (finalValue >= bronze) return "Bronze";
        return "–";
    }

    public (int Bronze, int Silver, int Gold) GetThresholds(int min, int max, RunSettings settings)
    {
        if (max == min) return (min, min, min);
        
        double diff = max - min;
        double mod = settings.ThresholdDifficulty;

        return (
            (int)(min + (0.5  * mod) * diff),
            (int)(min + (0.75 * mod) * diff),
            (int)(min + (0.9  * mod) * diff)
        );
    }
}

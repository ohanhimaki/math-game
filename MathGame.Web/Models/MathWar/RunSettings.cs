namespace MathGame.Web.Models.MathWar;

public class RunSettings
{
    public double BaseScrollSpeed { get; set; } = 0.4;
    public double SpeedMultiplier { get; set; } = 1.0;
    public int GateCountModifier { get; set; } = 0;
    public double ThresholdDifficulty { get; set; } = 1.0;
    
    // Operation modifiers
    public double MultiplyMultiplier { get; set; } = 1.0;
    public bool NoDivisions { get; set; } = false;

    public double CurrentScrollSpeed => BaseScrollSpeed * SpeedMultiplier;
}

namespace MathGame.Web.Models.MathWar;

public class LevelConfig
{
    public int StartValue { get; set; } = 100;
    public int GateCount { get; set; } = 5;
    public int MaxSegments { get; set; } = 3;
    public bool AllowMultiply { get; set; } = true;
    public bool AllowDivide { get; set; } = true;
    public string? Theme { get; set; }
}

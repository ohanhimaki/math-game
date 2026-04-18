namespace MathGame.Web.Models.MathWar;

public class GateSegment
{
    public int StartLane { get; set; }
    public int EndLane { get; set; }
    public Operation Operation { get; set; } = new();
}

public class Gate
{
    public List<GateSegment> Segments { get; set; } = new();
    /// <summary>Y position as percentage (0=top, 100=bottom)</summary>
    public double YPosition { get; set; }
    public bool Applied { get; set; }

    // Trophy/threshold gate fields (IsThreshold=true → no operation applied)
    public bool IsThreshold { get; set; }
    public string? ThresholdEmoji { get; set; }
    public int ThresholdValue { get; set; }
    public string? ThresholdColor { get; set; }
}

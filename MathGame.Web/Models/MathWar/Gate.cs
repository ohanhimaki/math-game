namespace MathGame.Web.Models.MathWar;

public class Gate
{
    public Operation OptionA { get; set; } = new();
    public Operation OptionB { get; set; } = new();
    /// <summary>Y position as percentage (0=top, 100=bottom)</summary>
    public double YPosition { get; set; }
    public bool Applied { get; set; }
}

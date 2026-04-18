namespace MathGame.Web.Models.MathWar;

public class GameState
{
    public int CurrentValue { get; set; } = 100;
    public List<Gate> Gates { get; set; } = new();
    public bool IsGameOver { get; set; }
    /// <summary>Which lane (0–5) the player is in. Lanes 0–2 = OptionA, 3–5 = OptionB.</summary>
    public int PlayerLane { get; set; } = 2;
    public string? Rating { get; set; }
    public int GatesCompleted { get; set; }
}

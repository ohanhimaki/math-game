public class Round
{
    public Round(Game? activeGame)
    {
        
    }

    public List<int> Numbers { get; set; } = new List<int>();
    public int TotalSum => Numbers.Sum();
    public int? Answer { get; set; }
    public bool? IsCorrect { get; set; }
    public TimeSpan? TimeTaken { get; set; }
    public DateTime LastNumberShownTime { get; set; }

    public void GiveAnswer(int userSum)
    {
            Answer = userSum;
            IsCorrect = userSum == TotalSum;
            TimeTaken = DateTime.Now - LastNumberShownTime;
    }
}
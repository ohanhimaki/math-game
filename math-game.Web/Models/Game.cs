public class Game
{
    public Game(GameSettings gameSettings)
    {
        Settings = gameSettings;
        DelayInMilliseconds = gameSettings.DelayInMilliseconds;
        DigitsToShow = gameSettings.DigitsToShow;
        NumbersToSum = gameSettings.NumbersToSum;
    }
    
    public int DelayInMilliseconds { get; set; } 
    public int DigitsToShow { get; set; } 
    public int NumbersToSum { get; set; } 

    public GameSettings Settings { get; set; } = new GameSettings();

    public List<Round> Rounds { get; set; } = new List<Round>();
    public string RuleDescriptionShort => $"Laske yhteen {NumbersToSum} lukua joissa on {DigitsToShow} numeroa jokaisessa sinulla on {DelayInMilliseconds/1000.00} sekuntia aikaa numeroiden välillä";

    public void AddRound(Round activeRound)
    {
        
        Rounds.Add(activeRound);
        if(activeRound.IsCorrect ?? false)
        {
            IncreaseDifficulty();
        }
        else
        {
            DecreaseDifficulty();
        }
    }


    private void IncreaseDifficulty()
    {
        if(Settings.DelayLevelScaleOn)
        {
            DelayInMilliseconds = DelayInMilliseconds * 9 / 10;
        }
        if(Settings.DigitsLevelScaleOn)
        {
            DigitsToShow = DigitsToShow + 1;
        }
        if(Settings.NumbersLevelScaleOn)
        {
            NumbersToSum = NumbersToSum + 1;
        }
    }
    private void DecreaseDifficulty()
    {
        if(Settings.DelayLevelScaleOn)
        {
            DelayInMilliseconds = DelayInMilliseconds * 10 / 9;
        }
        if(Settings.DigitsLevelScaleOn)
        {
            DigitsToShow = DigitsToShow - 1;
        }
        if(Settings.NumbersLevelScaleOn)
        {
            NumbersToSum = NumbersToSum - 1;
        }
        
    }
}
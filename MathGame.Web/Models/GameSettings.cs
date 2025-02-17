


public class GameSettings
{
    public int DelayInMilliseconds { get; set; } = 1200;
    public int DigitsToShow { get; set; } = 1;
    public int NumbersToSum { get; set; } = 10;
    public bool DelayLevelScaleOn { get; set; } = true;
    public bool DigitsLevelScaleOn { get; set; } = false;
    public bool NumbersLevelScaleOn { get; set; } = false;
    
    public bool UseEnglishNumbers { get; set; } = false;
    
    
    // advanced 
    public int? MaxAnswerTimeMilliseconds { get; set; } = null;
    public bool ShowNumbers { get; set; } = false;
}

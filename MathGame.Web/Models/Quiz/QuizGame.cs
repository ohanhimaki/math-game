using MathGame.Web.Models.Quiz;

public class QuizGame
{
    private readonly Quiz _quiz;

    public QuizGame(Quiz quiz, int players = 1)
    {
        _quiz = quiz;
        AvailableQuestions = _quiz.items.ToList();
        Players = players;

        ShowValue = quiz.showAnswer;
        
        for (int i = 1; i <= Players; i++)
        {
            PlayerCards[i] = new List<QuizItem>();
        }
    }

    public bool ShowValue = false;
    private List<QuizItem> AvailableQuestions;

    public int Players { get; set; } = 1;
    
    public Dictionary<int,List<QuizItem>> PlayerCards { get; set; } = new(); 
    
    public QuizItem? GetRandomQuestion()
    {
        if (AvailableQuestions.Count == 0)
            return null;

        var random = new Random();
        int index = random.Next(AvailableQuestions.Count);
        var question = AvailableQuestions[index];
        AvailableQuestions.RemoveAt(index);
        return question;
    }
    
}
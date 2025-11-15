using MathGame.Web.Models.Quiz;

public class QuizGame<T> where T : IComparable<T>
{
    private readonly Quiz<T> _quiz;
    
    public string? QuizTitle => _quiz.title;
    public string? QuizDescription => _quiz.description;
    

    public QuizGame(Quiz<T> quiz, string playerNames = "", int players = 1)
    {
        _quiz = quiz;
        AvailableQuestions = _quiz.items?.ToList() ?? new List<QuizItem<T>>();

        ShowValue = quiz.showAnswer;
        
        var playerNamesArray = playerNames.Split(',');
        if (playerNamesArray.Length > 0 && playerNamesArray[0].Trim() != "")
        {
            players = 0;
            foreach (var name in playerNamesArray)
            {
                Players.Add(new Player<T>
                {
                    Name = name.Trim()
                });
            }
        }
        else if (players < 1)
        {
            players = 1;
        }
        for (int i = 1; i <= players; i++)
        {
            if (playerNamesArray.Length >= i && playerNamesArray[i - 1].Trim() != "")
            {
                Players.Add(new Player<T>
                {
                    Name = playerNamesArray[i - 1].Trim()
                });
                continue;
            }
            Players.Add(new Player<T>
            {
                Name = $"Player {i}"
            });
        }
    }

    public bool ShowValue = false;
    private List<QuizItem<T>> AvailableQuestions;
    public int TotalQuestionsLeft => AvailableQuestions.Count;
    public int TotalQuestions => _quiz.items?.Length ?? 0;
    

    
    // public Dictionary<int,List<QuizItem<T>>> PlayerCards { get; set; } = new(); 
    public List<Player<T>> Players { get; set; } = new();
    
    public QuizItem<T>? GetRandomQuestion()
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

public class Player<T> where T : IComparable<T>
{
    public string? Name { get; set; }
    public List<QuizItem<T>> Cards { get; set; } = new();
    public List<QuizItem<T>> FailedCards { get; set; } = new();

    public bool CheckOrderGuess(QuizItem<T> currentItem, QuizItem<T>? prev, QuizItem<T>? next)
    {
        if (currentItem.value == null) return false;
        if (prev != null && prev.value != null && currentItem.value.CompareTo(prev.value) < 0)
            return false;

        if (next != null && next.value != null && currentItem.value.CompareTo(next.value) > 0)
            return false;

        return true;
    }
    public void AddCard(QuizItem<T> item)
    {
        if (item != null)
        {
            Cards.Add(item);
        }
    }
    public void SortCards()
    {
        Cards.Sort((x, y) => x.value?.CompareTo(y.value) ?? 0);
    }

    public void AddFailedCard(QuizItem<T> currentItem) 
    {
        if (currentItem != null)
        {
            FailedCards.Add(currentItem);
        }
    }
}

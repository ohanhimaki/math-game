using MathGame.Web.Models.Quiz;

public class QuizGame<T> where T : IComparable<T>
{
    private readonly Quiz<T> _quiz;
    
    public string? QuizTitle => _quiz.title;
    public string? QuizDescription => _quiz.description;
    

    public QuizGame(Quiz<T> quiz, string playerNames = "", int players = 1, int initialCards = 1, int initialRyostoCards = 2)
    {
        _quiz = quiz;
        AvailableQuestions = _quiz.items?.ToList() ?? new List<QuizItem<T>>();
        InitialCardsPerPlayer = initialCards;
        InitialRyostoCardsPerPlayer = initialRyostoCards;

        ShowValue = quiz.showAnswer;
        
        var playerNamesArray = playerNames.Split(',');
        if (playerNamesArray.Length > 0 && playerNamesArray[0].Trim() != "")
        {
            players = 0;
            foreach (var name in playerNamesArray)
            {
                Players.Add(new Player<T>
                {
                    Name = name.Trim(),
                    RyostoCards = initialRyostoCards
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
                    Name = playerNamesArray[i - 1].Trim(),
                    RyostoCards = initialRyostoCards
                });
                continue;
            }
            Players.Add(new Player<T>
            {
                Name = $"Player {i}",
                RyostoCards = initialRyostoCards
            });
        }
    }

    public bool ShowValue = false;
    public int InitialCardsPerPlayer = 1;
    public int InitialRyostoCardsPerPlayer = 2;
    public bool UseRyostoCards = true;
    public bool UseDecadeGuessing = true;
    private List<QuizItem<T>> AvailableQuestions;
    public int TotalQuestionsLeft => AvailableQuestions.Count;
    public int TotalQuestions => _quiz.items?.Length ?? 0;

    public int CurrentPlayerIndex { get; private set; } = 0;
    public Player<T>? CurrentPlayer => Players.Count > 0 ? Players[CurrentPlayerIndex] : null;


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

    public void ReturnQuestion(QuizItem<T> question)
    {
        if (question != null && !AvailableQuestions.Contains(question))
        {
            AvailableQuestions.Add(question);
        }
    }

    public void NextPlayer()
    {
        CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
    }

    public List<int> GetAvailableDecades()
    {
        var allItems = new List<QuizItem<T>>();
        allItems.AddRange(AvailableQuestions);
        
        foreach (var player in Players)
        {
            allItems.AddRange(player.Cards);
            allItems.AddRange(player.FailedCards);
        }

        var decades = new HashSet<int>();
        foreach (var item in allItems)
        {
            if (item.value is int year)
            {
                int decade = (year / 10) * 10;
                decades.Add(decade);
            }
        }

        return decades.OrderBy(d => d).ToList();
    }

}

public class Player<T> where T : IComparable<T>
{
    public string? Name { get; set; }
    public List<QuizItem<T>> Cards { get; set; } = new();
    public List<QuizItem<T>> FailedCards { get; set; } = new();
    public int RyostoCards { get; set; } = 0;

    public bool CheckOrderGuess(QuizItem<T> currentItem, QuizItem<T>? prev, QuizItem<T>? next)
    {
        if (currentItem.value == null) return false;
        if (prev != null && prev.value != null && currentItem.value.CompareTo(prev.value) < 0)
            return false;

        if (next != null && next.value != null && currentItem.value.CompareTo(next.value) > 0)
            return false;

        return true;
    }

    public bool CheckDecadeGuess(QuizItem<T> currentItem, int guessedDecade)
    {
        if (currentItem.value is int year)
        {
            int actualDecade = (year / 10) * 10;
            return actualDecade == guessedDecade;
        }
        return false;
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

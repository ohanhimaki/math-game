namespace MathGame.Web.Models.Quiz;

public class Quiz<T> where T : IComparable<T>
{
    public string id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public bool showAnswer { get; set; }
    public QuizItem<T>[] items { get; set; }
}


public class QuizItem<T> where T : IComparable<T>
{
    public string id { get; set; } = Guid.NewGuid().ToString();
    public string text { get; set; }
    public T value { get; set; }
}


public class QuizFileInfo
{
    public string filename { get; set; }
    public string title { get; set; }
    public string valueType { get; set; }
    public string description { get; set; }
}


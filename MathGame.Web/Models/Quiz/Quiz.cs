namespace MathGame.Web.Models.Quiz;

public class Quiz
{
    public string id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string valueType { get; set; }
    public bool showAnswer { get; set; }
    public QuizItem[] items { get; set; }
}

public class QuizItem
{
    public string id { get; set; }
    public string text { get; set; }
    public string value { get; set; }
}



public class QuizOld
{
    public string Id { get; set; } = string.Empty;               // Visa-id, esim. "quiz-planeetat"
    public string Title { get; set; } = string.Empty;            // Näytettävä otsikko
    public string Description { get; set; } = string.Empty;      // Lyhyt kuvaus
    public string ValueType { get; set; } = string.Empty;        // "number" tai "date"
    public List<QuizItemOld> Items { get; set; } = new();           // Visaan kuuluvat itemit
}
// QuizItem.cs
public class QuizItemOld
{
    public string Id { get; set; } = string.Empty;               // Itemin tunniste
    public string Text { get; set; } = string.Empty;             // Näkyvä teksti (esim. nimi)
    public string Value { get; set; } = string.Empty;            // Järjestämisarvo (string, käsitellään erikseen)
    public bool ShowValue { get; set; }                          // Näytetäänkö arvo käyttäjälle
}

public class QuizFileInfo
{
    public string filename { get; set; }
    public string title { get; set; }
    public string description { get; set; }
}


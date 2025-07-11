using System.Text.Json;
using MathGame.Web.Models.Quiz;

namespace MathGame.Web;
// QuizService.cs
public class QuizService
{
    private readonly HttpClient _http;
    public List<QuizFileInfo> Quizzes { get; private set; } = new();

    public QuizService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<QuizFileInfo>> LoadQuizzesAsync()
    {
        if (Quizzes.Count > 0)
            return Quizzes;
        var json = await _http.GetStringAsync("quizzes/index.json");
        Quizzes = JsonSerializer.Deserialize<List<QuizFileInfo>>(json) ?? new();
        return Quizzes;
    }

    public async Task<Quiz<T>?> LoadQuizByFilenameAsync<T>(string filename) where T : IComparable<T>
    {
        var json = await _http.GetStringAsync($"quizzes/{filename}.json");
        return JsonSerializer.Deserialize<Quiz<T>>(json);
    }
}

using System.Linq;

namespace MathGame.Web.Models.Quiz
{
    public class QuizFactory
    {
        private readonly CsvQuizParser _csvParser;

        public QuizFactory(CsvQuizParser csvParser)
        {
            _csvParser = csvParser;
        }

        public QuizGame<int>? CreateGameFromCsv(string csvContent, string playerNames)
        {
            var quiz = _csvParser.Parse(csvContent);
            if (quiz == null)
            {
                return null;
            }

            return new QuizGame<int>(quiz, playerNames);
        }
    }
}
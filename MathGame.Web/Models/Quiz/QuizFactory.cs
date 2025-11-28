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

        public QuizGame<int>? CreateGameFromCsv(string csvContent, string playerNames, int initialCards = 1, int initialRyostoCards = 2)
        {
            var quiz = _csvParser.Parse(csvContent);
            if (quiz == null)
            {
                return null;
            }

            return new QuizGame<int>(quiz, playerNames, initialCards: initialCards, initialRyostoCards: initialRyostoCards);
        }
    }
}
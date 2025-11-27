using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace MathGame.Web.Models.Quiz
{
    public class CsvQuizParser
    {
        public Quiz<int>? Parse(string csvContent)
        {
            try
            {
                using var reader = new StringReader(csvContent);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

                var records = csv.GetRecords<SpotifyTrack>().ToList();

                var quiz = new Quiz<int>
                {
                    title = "Spotify Playlist Quiz",
                    description = "A quiz generated from a CSV file.",
                    showAnswer = true,
                    items = records
                        .Where(r => r.Album_Date != null && int.TryParse(r.Album_Date.AsSpan(0, 4), out _))
                        .Select(r => new QuizItem<int>
                        {
                            text = $"{r.Artist} - {r.Song}",
                            value = int.Parse(r.Album_Date.AsSpan(0, 4)),
                            Uri = !string.IsNullOrEmpty(r.Spotify_Track_Id) ? $"spotify:track:{r.Spotify_Track_Id}" : null
                        }).ToArray()
                };

                return quiz;
            }
            catch (System.Exception ex)
            {
                // TODO: Log the exception
                System.Console.WriteLine($"Error parsing CSV: {ex.Message}");
                return null;
            }
        }
    }
}

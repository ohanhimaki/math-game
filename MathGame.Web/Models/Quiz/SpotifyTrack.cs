using CsvHelper.Configuration.Attributes;

namespace MathGame.Web.Models.Quiz
{
    public class SpotifyTrack
    {
        [Name("Song")]
        public string? Song { get; set; }

        [Name("Artist")]
        public string? Artist { get; set; }

        [Name("Album Date")]
        public string? Album_Date { get; set; }

        [Name("Spotify Track Id")]
        public string? Spotify_Track_Id { get; set; }
    }
}

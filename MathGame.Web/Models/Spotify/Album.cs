using System.Text.Json.Serialization;

namespace MathGame.Web.Models.Spotify
{
    public class Album
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; } = "";
    }
}

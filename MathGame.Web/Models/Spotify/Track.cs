using System.Text.Json.Serialization;

namespace MathGame.Web.Models.Spotify
{
    public class Track
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("uri")]
        public string Uri { get; set; } = "";

        [JsonPropertyName("album")]
        public Album? Album { get; set; }

        [JsonPropertyName("artists")]
        public List<Artist> Artists { get; set; } = new List<Artist>();
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MathGame.Web.Models.Spotify
{
    public class Playlist
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("description")]
        public string Description { get; set; } = "";

        [JsonPropertyName("tracks")]
        public PlaylistTrackCollection? Tracks { get; set; }
    }

    public class PlaylistTrackCollection
    {
        [JsonPropertyName("items")]
        public List<PlaylistItem> Items { get; set; } = new List<PlaylistItem>();
    }

    public class PlaylistItem
    {
        [JsonPropertyName("track")]
        public Track? Track { get; set; }
    }
}

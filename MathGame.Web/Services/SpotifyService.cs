using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using MathGame.Web.Models.Spotify;

namespace MathGame.Web.Services
{
    public class SpotifyService
    {
        private readonly HttpClient _httpClient;
        private const string SpotifyApiBaseUrl = "https://api.spotify.com/v1/";
        private string _accessToken = "";

        public SpotifyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void SetAccessToken(string token)
        {
            _accessToken = token;
        }

        public async Task<Playlist?> GetPlaylistAsync(string playlistId)
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                throw new InvalidOperationException("Spotify Access Token is not set.");
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"{SpotifyApiBaseUrl}playlists/{playlistId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Playlist>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return null;
        }
    }
}
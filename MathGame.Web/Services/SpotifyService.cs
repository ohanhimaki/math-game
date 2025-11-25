using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MathGame.Web.Models.Spotify;

namespace MathGame.Web.Services
{
    public class SpotifyService
    {
        private readonly HttpClient _httpClient;
        private const string SpotifyApiBaseUrl = "https://api.spotify.com/v1/";
        private const string SpotifyTokenUrl = "https://accounts.spotify.com/api/token";
        private string _accessToken = "";

        public SpotifyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void SetAccessToken(string token)
        {
            _accessToken = token;
        }

        public async Task<TokenResponse?> AuthenticateAsync(string clientId, string clientSecret)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, SpotifyTokenUrl);

            // Encode credentials to Base64
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            // Set form data for client credentials grant
            var formData = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" }
            };
            request.Content = new FormUrlEncodedContent(formData);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Set the access token automatically
                if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    _accessToken = tokenResponse.AccessToken;
                }

                return tokenResponse;
            }

            return null;
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

        public async Task<DevicesResponse?> GetAvailableDevicesAsync()
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                throw new InvalidOperationException("Spotify Access Token is not set.");
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"{SpotifyApiBaseUrl}me/player/devices");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<DevicesResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return null;
        }

        public async Task<bool> PlayTrackAsync(string trackUri, string? deviceId = null)
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                throw new InvalidOperationException("Spotify Access Token is not set.");
            }

            var url = $"{SpotifyApiBaseUrl}me/player/play";
            if (!string.IsNullOrEmpty(deviceId))
            {
                url += $"?device_id={deviceId}";
            }

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var playbackRequest = new
            {
                uris = new[] { trackUri }
            };

            var jsonContent = JsonSerializer.Serialize(playbackRequest);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PausePlaybackAsync(string? deviceId = null)
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                throw new InvalidOperationException("Spotify Access Token is not set.");
            }

            var url = $"{SpotifyApiBaseUrl}me/player/pause";
            if (!string.IsNullOrEmpty(deviceId))
            {
                url += $"?device_id={deviceId}";
            }

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }

    public class DevicesResponse
    {
        public Device[]? Devices { get; set; }
    }

    public class Device
    {
        public string? Id { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Private_Session { get; set; }
        public bool Is_Restricted { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int? Volume_Percent { get; set; }
    }
}

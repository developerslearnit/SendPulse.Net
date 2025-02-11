using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json;
using SendPulseNetSDK.src.SendPulse.Models;
using Microsoft.Extensions.Options;

namespace SendPulseNetSDK.src.SendPulse.Services;

public class AuthService
{

    private readonly HttpClient _httpClient;
    private readonly SendPulseOptions _options;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "SendPulseNet_AccessToken";
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public AuthService(HttpClient httpClient,IOptions<SendPulseOptions> options, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _cache = cache;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        if (_cache.TryGetValue(CacheKey, out string? cachedToken))
        {
            return cachedToken;
        }

        var requestBody = new
        {
            grant_type = "client_credentials",
            client_id = _options.ClientId,
            client_secret = _options.ClientSecret
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_options.BaseUrl}/oauth/access_token", content);

        if (!response.IsSuccessStatusCode)
            return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        var authResponse = JsonSerializer.Deserialize<ApiAuthResponse>(responseBody, JsonOptions);

        if (authResponse?.AccessToken is null) return null;

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(authResponse.ExpiresIn - 60)
        };
        _cache.Set(CacheKey, authResponse.AccessToken, cacheOptions);

        return authResponse.AccessToken;

    }
}
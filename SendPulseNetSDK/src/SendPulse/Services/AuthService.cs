using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SendPulseNetSDK.SendPulse.Models;

namespace SendPulseNetSDK.SendPulse.Services;

public class AuthService(HttpClient httpClient, IOptions<SendPulseOptions> options, IMemoryCache cache)
{
    private readonly SendPulseOptions _options = options.Value;
    private const string CacheKey = "SendPulseNet_AccessToken";
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    /// <summary>
    /// Retrieves an access token asynchronously.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, returning the access token as a string, 
    /// or <c>null</c> if the retrieval fails.
    /// </returns>
    /// <exception cref="HttpRequestException">
    /// Thrown when authentication fails.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown when there is an error in the API request.
    /// </exception>
    /// <exception cref="AuthenticationException">
    /// Thrown when an unexpected error occurs.
    /// </exception>
    public async Task<string?> GetAccessTokenAsync()
    {
        if (cache.TryGetValue(CacheKey, out string? cachedToken))
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
        var response = await httpClient.PostAsync($"{_options.BaseUrl}/oauth/access_token", content);

        if (!response.IsSuccessStatusCode)
            return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        var authResponse = JsonSerializer.Deserialize<ApiAuthResponse>(responseBody, JsonOptions);

        if (authResponse?.AccessToken is null) return null;

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(authResponse.ExpiresIn - 60)
        };
        cache.Set(CacheKey, authResponse.AccessToken, cacheOptions);

        return authResponse.AccessToken;

    }
}
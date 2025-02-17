using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SendPulseNetSDK.SendPulse.Models;

namespace SendPulseNetSDK.SendPulse.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly SendPulseOptions _options;

    public AuthService(IOptions<SendPulseOptions> options, IMemoryCache cache, HttpClient httpClient)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient)); 
    }

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

        if (_cache.TryGetValue(CacheKey, out string? cachedToken))
        {
            return cachedToken!;
        }

        var requestBody = new
        {
            grant_type = "client_credentials",
            client_id = _options.ClientId,
            client_secret = _options.ClientSecret
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync($"{_options.BaseUrl}/oauth/access_token", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Authentication failed. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<ApiAuthResponse>(responseBody, JsonOptions);

            if (authResponse?.AccessToken is null)
            {
                throw new AuthenticationException("Authentication response does not contain an access token.");
            }

            var expirationSeconds = Math.Max(authResponse.ExpiresIn - 60, 30);

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expirationSeconds)
            };
            _cache.Set(CacheKey, authResponse.AccessToken, cacheOptions);

            return authResponse.AccessToken;
        }
        catch (HttpRequestException e)
        {
            throw new AuthenticationException("Failed to retrieve access token due to an HTTP error.", e);
        }
        catch (Exception e)
        {
            throw new AuthenticationException("An unexpected error occurred while retrieving access token.", e);
        }

    }
}
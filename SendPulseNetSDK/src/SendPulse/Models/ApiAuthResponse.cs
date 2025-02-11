using System.Text.Json.Serialization;

namespace SendPulseNetSDK.src.SendPulse.Models;

public class ApiAuthResponse
{
    [JsonPropertyName("token_type")] 
    public string TokenType { get; set; } = null!;

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; } 

    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = null!;
}
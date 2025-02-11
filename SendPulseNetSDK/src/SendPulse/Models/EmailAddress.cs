using System.Text.Json.Serialization;

namespace SendPulseNetSDK.src.SendPulse.Models;

public class EmailAddress
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }
}
using System.Text.Json.Serialization;

namespace SendPulseNetSDK.SendPulse.Models;

public class ApiEmailRoot
{
    [JsonPropertyName("email")]
    public SendPulseEmail Email { get; set; }
}
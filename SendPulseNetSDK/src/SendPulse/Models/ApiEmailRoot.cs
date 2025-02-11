using System.Text.Json.Serialization;

namespace SendPulseNetSDK.src.SendPulse.Models;

public class ApiEmailRoot
{
    [JsonPropertyName("email")]
    public SendPulseEmail Email { get; set; }
}
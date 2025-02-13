using System.Text.Json.Serialization;

namespace SendPulseNetSDK.SendPulse.Models;

public class SendPulseEmail
{
    [JsonPropertyName("html")]
    public string Html { get; set; }

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Text { get; set; }

    [JsonPropertyName("subject")]
    public string Subject { get; set; }

    [JsonPropertyName("from")]
    public EmailAddress From { get; set; }

    [JsonPropertyName("to")]
    public List<EmailAddress> To { get; set; }

    [JsonPropertyName("cc")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<EmailAddress>? Cc { get; set; }

    [JsonPropertyName("bcc")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<EmailAddress> Bcc { get; set; }

    [JsonPropertyName("attachments_binary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string> AttachmentsBinary { get; set; }
}
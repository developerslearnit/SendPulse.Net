using System.Text.Json.Serialization;

namespace SendPulseNetSDK.SendPulse.Models;

public class EmailListModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("sender")]
    public string Sender { get; set; }

    [JsonPropertyName("total_size")]
    public int TotalSize { get; set; }

    [JsonPropertyName("smtp_answer_code")]
    public int SmtpAnswerCode { get; set; }

    [JsonPropertyName("smtp_answer_code_explain")]
    public string SmtpAnswerCodeExplain { get; set; }

    [JsonPropertyName("smtp_answer_subcode")]
    public string SmtpAnswerSubcode { get; set; }

    [JsonPropertyName("used_ip")]
    public string UsedIp { get; set; }

    [JsonPropertyName("recipient")]
    public string Recipient { get; set; }

    [JsonPropertyName("subject")]
    public string Subject { get; set; }

    [JsonPropertyName("send_date")]
    public DateTime SendDate { get; set; }

    [JsonPropertyName("sender_ip")]
    public string SenderIp { get; set; }

    [JsonPropertyName("smtp_answer_data")]
    public string SmtpAnswerData { get; set; }
}
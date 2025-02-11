using SendPulseNetSDK.src.SendPulse.Models;

namespace SendPulseNetSDK.src.SendPulse;

public interface ISendPulseClient
{
    Task<string?> GetAccessTokenAsync();
    Task<EmailResponse?> SendApiEmailAsync(EmailAddress fromEmail, List<EmailAddress> toEmails, string subject, string htmlBody,
        List<EmailAddress>? ccEmails = null, List<EmailAddress>? bccEmails = null, Dictionary<string, byte[]>? attachments = null);
}
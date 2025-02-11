using SendPulseNetSDK.src.SendPulse.Models;

namespace SendPulseNetSDK.src.SendPulse.Services;

public class SendPulseClient(AuthService authService, EmailApiService emailApiService) : ISendPulseClient
{
    public async Task<string?> GetAccessTokenAsync()
    {
        return await authService.GetAccessTokenAsync();
    }

    public async Task<EmailResponse?> SendApiEmailAsync(EmailAddress fromEmail, List<EmailAddress> toEmails, string subject, string htmlBody, List<EmailAddress>? ccEmails = null,
        List<EmailAddress>? bccEmails = null, Dictionary<string, byte[]>? attachments = null)
    {
        return await emailApiService.SendApiEmailAsync(fromEmail, toEmails, subject, htmlBody, ccEmails, bccEmails,
            attachments);
    }
}
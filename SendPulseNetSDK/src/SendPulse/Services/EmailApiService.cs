using SendPulseNetSDK.src.SendPulse.Models;
using System.Text;
using SendPulseNetSDK.src.SendPulse.Helpers;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace SendPulseNetSDK.src.SendPulse.Services;

public class EmailApiService
{

    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;
    private readonly SendPulseOptions _options;

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public EmailApiService(HttpClient httpClient, AuthService authService, IOptions<SendPulseOptions> options)
    {
        _httpClient = httpClient;
        _authService = authService;
        _options = options.Value;
    }

    public async Task<EmailResponse?> SendApiEmailAsync(EmailAddress fromEmail, List<EmailAddress> toEmails,
        string subject, string htmlBody,
        List<EmailAddress>? ccEmails = null, List<EmailAddress>? bccEmails = null,
        Dictionary<string, byte[]>? attachments = null)
    {
        string? token = await _authService.GetAccessTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            return new EmailResponse(){Id = "",Result = false};

        var emailMessage = new SendPulseEmail
        {
            Html = Convert.ToBase64String(Encoding.UTF8.GetBytes(htmlBody)),
            Subject = subject,
            From = new EmailAddress { Name = fromEmail.Name, Email = fromEmail.Email },
            To = toEmails
        };

        if (ccEmails != null && ccEmails.Any())
        {
            emailMessage.Cc = ccEmails;
        }

        if (bccEmails != null && bccEmails.Any())
        {
            emailMessage.Bcc = bccEmails;
        }

        if (attachments != null)
        {
            emailMessage.AttachmentsBinary = StaticHelpers.ConvertAttachmentsToBase64(attachments);
        }

        var jsonRequest = JsonSerializer.Serialize(new ApiEmailRoot { Email = emailMessage }, JsonOptions);
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}/smtp/emails")
        {
            Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json")
        };
        requestMessage.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

        var response = await _httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
            return new EmailResponse() { Id = "", Result = false };

        var responseBody = await response.Content.ReadAsStringAsync();
        var emailResponse = JsonSerializer.Deserialize<EmailResponse>(responseBody, JsonOptions);

        return emailResponse;
    }
}

//mark2k@outlook.com
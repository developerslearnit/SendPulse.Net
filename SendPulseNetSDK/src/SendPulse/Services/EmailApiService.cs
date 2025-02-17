using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SendPulseNetSDK.SendPulse.Exceptions;
using SendPulseNetSDK.SendPulse.Helpers;
using SendPulseNetSDK.SendPulse.Models;

namespace SendPulseNetSDK.SendPulse.Services;

public class EmailApiService
{

    private readonly HttpClient _httpClient;
    private readonly SendPulseOptions _options;
    private readonly AuthService _authService;

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public EmailApiService(HttpClient httpClient, IOptions<SendPulseOptions> options, AuthService authService)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _authService = authService;
    }

    /// <summary>
    /// Sends an email using the SendPulse SMTP Service API.
    /// </summary>
    /// <param name="fromEmail">The sender's email address.</param>
    /// <param name="toEmails">A list of recipient email addresses.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="htmlBody">The HTML content of the email.</param>
    /// <param name="ccEmails">An optional list of recipients who will receive a copy of the email. Recipients will see who received an email copy</param>
    /// <param name="bccEmails">An optional list of recipients who will receive a blind carbon copy of the email. Recipients will not see who received an email copy</param>
    /// <param name="attachments">
    /// An optional dictionary of attachments where the key is the filename and the value is the byte array of the file content.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation, returning an <see cref="EmailResponse"/> 
    /// containing the result of the email send operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when one or more required parameters are null
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// Thrown when there is an error in the API request.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown when the email cannot be sent due to an API failure or other processing errors.
    /// </exception>
    /// <exception cref="SendPulseEmailException">
    /// Thrown when an unexpected error occurs.
    /// </exception>
    /// <example>
    ///  <code>
    /// var emailService = new EmailService();
    /// var from = new EmailAddress("sender@example.com", "Sender Name");
    /// var toList = new List&lt;EmailAddress&gt;
    /// {
    ///     new EmailAddress("recipient@example.com", "Recipient Name")
    /// };
    /// string subject = "Welcome to Our Service";
    /// string body = "&lt;h1&gt;Thank you for signing up!&lt;/h1&gt;";
    ///
    /// // Sending email without optional parameters
    /// var response = await emailService.SendApiEmailAsync(from, toList, subject, body);
    ///
    /// // Sending email with CC and BCC
    /// var ccList = new List&lt;EmailAddress&gt; { new EmailAddress("cc@example.com", "CC Person") };
    /// var bccList = new List&lt;EmailAddress&gt; { new EmailAddress("bcc@example.com", "BCC Person") };
    ///
    /// var responseWithCcBcc = await emailService.SendApiEmailAsync(from, toList, subject, body, ccList, bccList);
    ///
    /// // Sending email with attachments
    /// var attachments = new Dictionary&lt;string, byte[]&gt;
    /// {
    ///     { "invoice.pdf", File.ReadAllBytes("path/to/invoice.pdf") }
    /// };
    ///
    /// var responseWithAttachments = await emailService.SendApiEmailAsync(from, toList, subject, body, null, null, attachments);
    /// </code>
    /// </example>
    public async Task<EmailResponse?> SendApiEmailAsync(EmailAddress fromEmail, List<EmailAddress> toEmails,
        string subject, string htmlBody,
        List<EmailAddress>? ccEmails = null, List<EmailAddress>? bccEmails = null,
        Dictionary<string, byte[]>? attachments = null)
    {
        string? token = await _authService.GetAccessTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            throw new Exception("Authentication token is missing");

        if (!toEmails.Any())
            throw new ArgumentNullException(nameof(toEmails));

        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentNullException(nameof(subject));

        if (string.IsNullOrWhiteSpace(htmlBody))
            throw new ArgumentNullException(nameof(htmlBody));

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
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new SendPulseEmailException($"SendPulse API Error: {response.StatusCode} - {errorResponse}");
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        var emailResponse = JsonSerializer.Deserialize<EmailResponse>(responseBody, JsonOptions);

        return emailResponse;
    }

    /// <summary>
    /// This method fetches all sent emails
    /// </summary>
    /// <returns>
    ///  A task that represents the List of all sent emails, returning an <see cref="EmailListModel"/> 
    /// </returns>

    public async Task<List<EmailListModel>> GetEmailListAsync()
    {
        string? token = await _authService.GetAccessTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            throw new Exception("Authentication token is missing");

        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");

        var response = await _httpClient.GetAsync($"{_options.BaseUrl}/smtp/emails");


        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new SendPulseEmailException($"SendPulse API Error: {response.StatusCode} - {errorResponse}");
        }

        var responseBody = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions();
        options.Converters.Add(new CustomDateTimeConverter());

        var emailResponse = JsonSerializer.Deserialize<List<EmailListModel>>(responseBody, options);

        return emailResponse;

    }

    public async Task<EmailDetails> GetEmailDetailsAsync(string id)
    {
        string? token = await _authService.GetAccessTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            throw new Exception("Authentication token is missing");

        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");

        var response = await _httpClient.GetAsync($"{_options.BaseUrl}/smtp/emails/{id}");


        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new SendPulseEmailException($"SendPulse API Error: {response.StatusCode} - {errorResponse}");
        }

        var responseBody = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions();
        options.Converters.Add(new CustomDateTimeConverter());

        var emailResponse = JsonSerializer.Deserialize<EmailDetails>(responseBody, options);

        return emailResponse;
    }

}


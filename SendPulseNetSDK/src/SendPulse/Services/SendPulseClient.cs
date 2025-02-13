using System.Security.Authentication;
using SendPulseNetSDK.SendPulse.Exceptions;
using SendPulseNetSDK.SendPulse.Models;

namespace SendPulseNetSDK.SendPulse.Services;

public class SendPulseClient(AuthService authService, EmailApiService emailApiService) : ISendPulseClient
{
    /// <summary>
    /// Retrieves an access token asynchronously.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, returning the access token as a string, 
    /// or <c>null</c> if the retrieval fails.
    /// </returns>
    /// <exception cref="HttpRequestException">
    /// Thrown when authentication fails.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown when there is an error in the API request.
    /// </exception>
    /// <exception cref="AuthenticationException">
    /// Thrown when an unexpected error occurs.
    /// </exception>
    public async Task<string?> GetAccessTokenAsync()
    {
        return await authService.GetAccessTokenAsync();
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
    public async Task<EmailResponse?> SendApiEmailAsync(EmailAddress fromEmail, List<EmailAddress> toEmails, string subject, string htmlBody, List<EmailAddress>? ccEmails = null,
        List<EmailAddress>? bccEmails = null, Dictionary<string, byte[]>? attachments = null)
    {
        return await emailApiService.SendApiEmailAsync(fromEmail, toEmails, subject, htmlBody, ccEmails, bccEmails,
            attachments);
    }
}
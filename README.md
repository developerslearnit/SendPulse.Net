# SendPulse.Net

![SendPulse Logo](https://login.sendpulse.com/img/sendpulse-logo-menu.svg)

SendPulse.Net is a .NET SDK that simplifies integration with the SendPulse API, allowing developers to manage email campaigns, SMTP, push notifications, chatbots, and more in their .NET applications.

## Features
- Authenticate and interact with the SendPulse API
- Manage email campaigns, mailing lists, and templates
- Send transactional emails and SMS
- Work with push notifications
- Automate chatbot interactions
- Retrieve analytics and reports

## Installation

Install the package via NuGet:
```sh
Install-Package SendPulse.Net
```

Or using .NET CLI:
```sh
dotnet add package SendPulse.Net
```

## Getting Started

### 1. Configure API Credentials
To use SendPulse.Net, you need your SendPulse API credentials.
Obtain them from [SendPulse API settings](https://login.sendpulse.com/settings/#api).

### 2. Initialize the Client
```csharp
using SendPulse.Net;

var sendPulseClient = new SendPulseClient(
    "your-client-id",
    "your-client-secret"
);
```

### 3. Example: Send an Email
```csharp
var email = new EmailMessage
{
    From = new EmailAddress("your-email@example.com", "Your Name"),
    To = new List<EmailAddress>
    {
        new EmailAddress("recipient@example.com", "Recipient Name")
    },
    Subject = "Hello from SendPulse.Net",
    HtmlContent = "<h1>Welcome!</h1><p>This is a test email.</p>"
};

var response = await sendPulseClient.Email.SendAsync(email);
Console.WriteLine(response.IsSuccess ? "Email sent!" : "Failed to send email.");
```

## Available APIs
- **Email API** - Manage campaigns, templates, and transactional emails.
- **SMS API** - Send and track SMS messages.
- **Push API** - Manage and send push notifications.
- **Chatbot API** - Automate chatbot workflows.
- **Contacts API** - Manage mailing lists and subscribers.

## Documentation
Detailed documentation can be found [here](https://sendpulse.com/integrations/api).

## Contributing
Contributions are welcome! Feel free to submit pull requests or report issues.

## License
This project is licensed under the MIT License.

## Support
For issues or feature requests, open an issue in the repository or contact [SendPulse Support](https://sendpulse.com/support).

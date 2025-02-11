# SendPulse.Net

![SendPulse Logo](https://login.sendpulse.com/img/sendpulse-logo-menu.svg)

SendPulse.Net is a .NET SDK that simplifies integration with the SendPulse API, allowing developers to manage email campaigns, SMTP, push notifications, chatbots, and more in their .NET applications.

## Features
- Authenticate and interact with the SendPulse API
- Manage email campaigns, mailing lists, and templates
- Send transactional emails and SMS

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

### 2. Configure Service
```csharp
using SendPulseNetSDK.src.SendPulse;
using SendPulseNetSDK.src.SendPulse.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSendPulseNet(config =>
{
    config.BaseUrl = "https://api.sendpulse.com";
    config.ClientId = "991e5dcb9163ca4d5d**********";
    config.ClientSecret = "client_secret_here";
});
```

### 3. Example: Send an Email
```csharp
//Inject Interface
// ISendPulseClient sendPulseClient;

var fromEmail = new EmailAddress() { Email = "me@email.com", Name = "My Email Name" };

    var toEmail = new List<EmailAddress>()
    {
        new EmailAddress()
        {
            Email = "me@gmail.com",
            Name ="John Doe"
        }
    };
 var response = await sendPulseClient.SendApiEmailAsync(fromEmail, toEmail, "Test Email", "<p>Testing Sendpulse Nuget package</p>");

Console.WriteLine(response.Result ? "Email sent!" : "Failed to send email.");
```

## Available APIs in this version
- **Email API** - Send transactional emails.

## Documentation
Detailed documentation can be found [here](https://sendpulse.com/integrations/api).

## Contributing
Contributions are welcome! Feel free to submit pull requests or report issues.

## License
This project is licensed under the MIT License.

## Support
For issues or feature requests, open an issue in the repository or contact [SendPulse Support](https://sendpulse.com/support).

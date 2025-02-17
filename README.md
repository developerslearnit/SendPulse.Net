# SendPulse.Net

![SendPulse Logo](https://login.sendpulse.com/img/sendpulse-logo-menu.svg)

## Overview

**SendPulse.Net is a .NET SDK that simplifies integration with the SendPulse API, allowing developers to manage email campaigns, SMTP, push notifications, chatbots, and more in their .NET applications.**


# Table of Contents

* [Installation](#installation)
* [Usage](#usage)

# Installation

## Prerequisites

- .NET Core 6.0+
- A SendPulse account, [sign up for free](https://sendpulse.com) to send up to 15,000 emails. Check out [their pricing](https://sendpulse.com/pricing).





## Install Package


Install the package via NuGet:
```sh
Install-Package SendPulse.Net
```

Or using .NET CLI:
```sh
dotnet add package SendPulse.Net
```


# Quick Start

### 1. Configure Service

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

### 2. Send basic email

```csharp
var fromEmail = new EmailAddress() { Email = "test@test.com", Name = "From Name" };

var toEmail = new List<EmailAddress>()
{
    new EmailAddress()
    {
        Email = "john@gmail.com",
        Name ="John Doe"
    }
};
await sendpulse.SendApiEmailAsync(fromEmail, toEmail, "Test Email", "<p>Testing SendPulse Nuget package</p>");
```


### 2. Sending email with CC and BCC

```csharp
var fromEmail = new EmailAddress() { Email = "test@test.com", Name = "From Name" };
var ccList = new List<EmailAddress>{ new EmailAddress("cc@example.com", "CC Person") };
var bccList = new List<EmailAddress> { new EmailAddress("bcc@example.com", "BCC Person") };

var toEmail = new List<EmailAddress>()
{
    new EmailAddress()
    {
        Email = "john@gmail.com",
        Name ="John Doe"
    }
};
await sendpulse.SendApiEmailAsync(fromEmail, toEmail, "Test Email", "<p>Testing SendPulse Nuget package</p>",ccList,bccList);
```


### 3. Sending email with attachments

```csharp
var fromEmail = new EmailAddress() { Email = "test@test.com", Name = "From Name" }; };

var toEmail = new List<EmailAddress>()
{
    new EmailAddress()
    {
        Email = "john@gmail.com",
        Name ="John Doe"
    }
};


var attachments = new Dictionary<string, byte[]>;
{
	{ "invoice.pdf", File.ReadAllBytes("path/to/invoice.pdf") }
};

await sendpulse.SendApiEmailAsync(fromEmail, toEmail, "Test Email", "<p>Testing SendPulse Nuget package</p>",null,null,attachments);
```

## Available APIs in this version
- **Email API** - Send transactional emails.
- **Email API** - Get List of Sent emails
- **Email API** - Get single sent email by Id

## Documentation
Detailed documentation can be found [here](https://sendpulse.com/integrations/api).

## Contributing
Contributions are welcome! Feel free to submit pull requests or report issues.

## License
This project is licensed under the MIT License.

## Support
For issues or feature requests, open an issue in the repository or contact [SendPulse Support](https://sendpulse.com/support).

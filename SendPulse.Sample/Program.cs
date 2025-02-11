using SendPulseNetSDK.src.SendPulse;
using SendPulseNetSDK.src.SendPulse.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSendPulseNet(config =>
{
    config.BaseUrl = "https://api.sendpulse.com";
    config.ClientId = "771e5dcb9163ca4d****3c4f971222cd3";
    config.ClientSecret = "4a53b798c35*******a7d7015f808804";
});


var app = builder.Build();

app.MapGet("/",async (ISendPulseClient sendpulse) =>
{
    var fromEmail = new EmailAddress() { Email = "postmaster@nassipartnerships.com", Name = "NASSI Partnership" };

    var toEmail = new List<EmailAddress>()
    {
        new EmailAddress()
        {
            Email = "mark2kk@gmail.com",
            Name ="Adesina Mark"
        }
    };
    await sendpulse.SendApiEmailAsync(fromEmail, toEmail, "Test Email", "<p>Testing Sendpulse Nuget package</p>");
});

app.Run();

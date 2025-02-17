using Microsoft.AspNetCore.Http.HttpResults;
using SendPulseNetSDK.SendPulse;
using SendPulseNetSDK.SendPulse.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSendPulseNet(config =>
{
    config.BaseUrl = "https://api.sendpulse.com";
    config.ClientId = "c689a88004";
    config.ClientSecret = "91923b328c";
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
    await sendpulse.SendApiEmailAsync(fromEmail, toEmail, "Test Email", "<p>Testing SendPulse Nuget package</p>");
});

app.MapGet("/emails", async (ISendPulseClient sendpulse) =>
{
    var emails = await sendpulse.GetEmailListAsync();

    return emails;
});

app.MapGet("/emails/{id}", async (ISendPulseClient sendpulse,string id) =>
{
    var email = await sendpulse.GetEmailDetailsAsync(id);

    return email;
});

app.Run();

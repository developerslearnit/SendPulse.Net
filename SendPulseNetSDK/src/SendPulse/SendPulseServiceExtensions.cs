using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendPulseNetSDK.src.SendPulse.Services;

namespace SendPulseNetSDK.src.SendPulse;

public static class SendPulseServiceExtensions
{
    public static IServiceCollection AddSendPulseNet(this IServiceCollection services, Action<SendPulseOptions> configureOptions)
    {
        
    
        services.Configure(configureOptions);

        services.AddMemoryCache();
        services.AddHttpClient<AuthService>();
        services.AddHttpClient<EmailApiService>();
        services.AddScoped<ISendPulseClient, SendPulseClient>();

        return services;
    }
}
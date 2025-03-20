using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using POC.FleetManager.Common.Events;

namespace POC.FleetManager.Loader;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {

        var config = new ConfigurationBuilder()
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .Build();

        var azureAppConfigSettings = config.GetSection("AzureAppConfiguration").Get<AzureAppConfigSettings>()!;
        var entraIdSettings = config.GetSection("EntraId").Get<EntraIdSettings>()!;
        var webServiceSettings = config.GetSection("WebService").Get<WebServiceSettings>()!;

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

            });

        // Add Azure App Configuration as a configuration source
        builder.Configuration.AddAzureAppConfiguration(options =>
        {
            options.Connect(azureAppConfigSettings!.ConnectionString)
                   .ConfigureRefresh(refresh =>
                   {
                       refresh.RegisterAll();
                       refresh.SetRefreshInterval(TimeSpan.FromMinutes(5));
                   });
        });

        builder.Services.AddSingleton(azureAppConfigSettings);
        builder.Services.AddSingleton(entraIdSettings);
        builder.Services.AddSingleton(webServiceSettings);
        builder.Services.AddAzureAppConfiguration();
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<AuthService>();
        //builder.Services.AddSingleton<ExtensionService>();
        //builder.Services.AddSingleton<ConfigService>();
        builder.Services.AddSingleton<EventAggregator>();
        builder.Services.AddLogging();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

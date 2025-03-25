using System.Composition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using POC.FleetManager.Common;
using DbUp;
using POC.FleetManager.Common.Events;

namespace POC.FleetManager.Extensions.DriverRegistration;

[Export(typeof(IExtension))]
public class DriverRegistrationExtension:ExtensionBase
{
    public override string Name => "Driver Registration";
    public override string Version => "1.0";

    public override async void Initialize(IConfiguration configuration, ILogger<IExtension> logger, IServiceProvider serviceProvider, IEventAggregator eventAggregator)
    {
        base.Initialize(configuration, logger, serviceProvider, eventAggregator);

        logger.LogInformation("Initializing Driver Registration Extension");

        var connectionString = configuration.GetConnectionString("DriverDb");
        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(System.Reflection.Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();
        if (!result.Successful)
        {
            logger.LogError(result.Error, "Failed to initialize Driver Registration database schema");
            throw result.Error;
        }
        logger.LogInformation("Driver Registration database schema initialized");

        await Task.Delay(5000);

        await PublishEvent(new DriverRegistrationEvent(("DriverID", "CDE456")));
    }
}

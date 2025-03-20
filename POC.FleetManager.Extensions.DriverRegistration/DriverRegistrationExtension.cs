using System.Composition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using POC.FleetManager.Common;
using DbUp;
using POC.FleetManager.Common.Events;

namespace POC.FleetManager.Extensions.DriverRegistration;

[Export(typeof(IExtension))]
public class DriverRegistrationExtension(IConfiguration configuration, ILogger<IExtension> logger, EventAggregator eventAggregator) : ExtensionBase(configuration, logger, eventAggregator)
{
    public override string Name => "Driver Registration";
    public override string Version => "1.0";

    public override async Task Initialize()
    {
        Logger.LogInformation("Initializing Driver Registration Extension");

        var connectionString = Configuration.GetConnectionString("DriverDb");
        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(System.Reflection.Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();
        if (!result.Successful)
        {
            Logger.LogError(result.Error, "Failed to initialize Driver Registration database schema");
            throw result.Error;
        }
        Logger.LogInformation("Driver Registration database schema initialized");

        await Task.Delay(5000);

        var DriverData = new Dictionary<string, object> { { "DriverID", "CDE456" } };

        await PublishEvent(new DriverRegistrationEvent(DriverData));
    }
}

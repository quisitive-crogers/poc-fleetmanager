using System.Composition;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using POC.FleetManager.Common;
using DbUp;

namespace POC.FleetManager.Extensions.VehicleRegistration
{
    [Export(typeof(IExtension))]
    public class VehicleRegistrationExtension(IConfiguration configuration, ILogger<IExtension> logger, EventAggregator eventAggregator) : ExtensionBase(configuration, logger, eventAggregator)
    {
        public override string Name => "Vehicle Registration";
        public override string Version => "1.0";

        public override async Task Initialize()
        {
            Logger.LogInformation("Initializing Vehicle Registration Extension");

            // Initialize database schema with DbUp
            var connectionString = Configuration.GetConnectionString("VehicleDb");
            var upgrader = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(System.Reflection.Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();
            if (!result.Successful)
            {
                Logger.LogError(result.Error, "Failed to initialize Vehicle Registration database schema");
                throw result.Error;
            }
            Logger.LogInformation("Vehicle Registration database schema initialized");
            await Task.CompletedTask;
        }

        public override async Task Run()
        {
            Logger.LogInformation("Vehicle Registration Extension running");
            // Placeholder for continuous operation (e.g., polling, UI updates)
            await Task.CompletedTask;
        }
    }
}

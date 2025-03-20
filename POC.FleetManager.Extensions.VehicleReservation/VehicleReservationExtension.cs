using System.Composition;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using POC.FleetManager.Common;
using DbUp;

namespace POC.FleetManager.Extensions.VehicleReservation
{
    [Export(typeof(IExtension))]
    public class VehicleReservationExtension(IConfiguration configuration, ILogger<IExtension> logger, EventAggregator eventAggregator) : ExtensionBase(configuration, logger, eventAggregator)
    {
        public override string Name => "Vehicle Reservation";
        public override string Version => "1.0";

        public override async Task Initialize()
        {
            Logger.LogInformation("Initializing Vehicle Reservation Extension");

            var connectionString = Configuration.GetConnectionString("ReservationDb");
            var upgrader = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(System.Reflection.Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();
            if (!result.Successful)
            {
                Logger.LogError(result.Error, "Failed to initialize Vehicle Reservation database schema");
                throw result.Error;
            }
            Logger.LogInformation("Vehicle Reservation database schema initialized");
            await Task.CompletedTask;
        }

        public override async Task Run()
        {
            Logger.LogInformation("Vehicle Reservation Extension running");
            // Simulate a reservation event
            await Task.Delay(5000); // Simulate work
            PublishEvent(new ReservationEvent(new Dictionary<string, object> { { "VIN", "V123" }, { "Driver", "D456" } }));
        }

        public async void HandleNewVehicleRegistration(EventData eventData)
        {
            Logger.LogInformation("New Vehicle information received. VIN:{VIN}", eventData.Payload["VIN"]);
            await Task.CompletedTask;
        }
    }

    public record ReservationEvent(Dictionary<string, object> Payload) : EventData(nameof(ReservationEvent), Payload);
}

using System.Composition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using POC.FleetManager.Common;
using DbUp;
using POC.FleetManager.Common.Events;

namespace POC.FleetManager.Extensions.VehicleReservation;

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

        SubscribeToEvent(nameof(VehicleRegistrationEvent), HandleNewVehicleRegistration);
        SubscribeToEvent(nameof(DriverRegistrationEvent), HandleNewDriverRegistration);
        SubscribeToEvent(nameof(ReservationRequestEvent), HandleRegistrationRequest);

        Logger.LogInformation("Vehicle Reservation database schema initialized");

        Logger.LogInformation("Vehicle Reservation Extension running");
        // Simulate a reservation event
        await Task.Delay(5000); // Simulate work
    }

    public async Task HandleNewVehicleRegistration(EventData eventData)
    {
        Logger.LogInformation("New Vehicle information received. VIN:{VIN}", eventData.Payload["VIN"]);
        await Task.CompletedTask;
    }

    public async Task HandleNewDriverRegistration(EventData eventData)
    {
        Logger.LogInformation("New Driver information received. Drier ID:{DriverID}", eventData.Payload["ID"]);
        await Task.CompletedTask;
    }

    public async Task HandleRegistrationRequest(EventData eventData)
    {
        Logger.LogInformation("Reservation Requested for VIN:{VehicleID} Driver:{DriverID} on {ReservationDate}", eventData.Payload["VIN"], eventData.Payload["DriverID"], eventData.Payload["ReservationDate"]);

        var reservation = new Dictionary<string, object>
        {
            { "VIN", eventData.Payload["VIN"] },
            { "DriverID", eventData.Payload["DriverID"] },
            { "ReservationDate", eventData.Payload["ReservationDate"] }
        };

        await PublishEvent(new ReservationEvent(reservation));
        await Task.CompletedTask;
    }
}

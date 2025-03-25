using System.Composition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using POC.FleetManager.Common;
using DbUp;
using POC.FleetManager.Common.Events;

namespace POC.FleetManager.Extensions.VehicleReservation;

[Export(typeof(IExtension))]
public class VehicleReservationExtension : ExtensionBase
{
    public override string Name => "Vehicle Reservation";
    public override string Version => "1.0";

    public override async void Initialize(IConfiguration configuration, ILogger<IExtension> logger, IServiceProvider serviceProvider, IEventAggregator eventAggregator)
    {
        base.Initialize(configuration, logger, serviceProvider, eventAggregator);

        logger.LogInformation("Initializing Vehicle Reservation Extension");

        var connectionString = configuration.GetConnectionString("ReservationDb");
        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(System.Reflection.Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();
        if (!result.Successful)
        {
            logger.LogError(result.Error, "Failed to initialize Vehicle Reservation database schema");
            throw result.Error;
        }

        SubscribeToEvent(nameof(VehicleRegistrationEvent), HandleNewVehicleRegistration);
        SubscribeToEvent(nameof(DriverRegistrationEvent), HandleNewDriverRegistration);
        SubscribeToEvent(nameof(ReservationRequestEvent), HandleRegistrationRequest);

        logger.LogInformation("Vehicle Reservation database schema initialized");

        logger.LogInformation("Vehicle Reservation Extension running");
        // Simulate a reservation event
        await Task.Delay(5000); // Simulate work
    }

    public async Task HandleNewVehicleRegistration(EventData eventData)
    {
        EnsureInitialized();
        logger!.LogInformation("New Vehicle information received. VIN:{VIN}", eventData.Payload.FindTupleByKey("VIN")!.Value.ToString());
        await Task.CompletedTask;
    }

    public async Task HandleNewDriverRegistration(EventData eventData)
    {
        EnsureInitialized();
        logger!.LogInformation("New Driver information received. Drier ID:{DriverID}", eventData.Payload.FindTupleByKey("ID")!.Value.ToString());
        await Task.CompletedTask;
    }

    public async Task HandleRegistrationRequest(EventData eventData)
    {
        EnsureInitialized();
        logger!.LogInformation("Reservation Requested for VIN:{VehicleID} Driver:{DriverID} on {ReservationDate}", 
            eventData.Payload.FindTupleByKey("VIN")!.Value.ToString(), 
            eventData.Payload.FindTupleByKey("DriverID")!.Value.ToString(), 
            eventData.Payload.FindTupleByKey("ReservationDate")!.Value.ToString());

        await PublishEvent(new ReservationEvent(eventData.Payload));
        await Task.CompletedTask;
    }
}

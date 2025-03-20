namespace POC.FleetManager.Common.Events
{
    public record VehicleRegistrationEvent(Dictionary<string, object> Payload) : EventData(nameof(VehicleRegistrationEvent), Payload);
}

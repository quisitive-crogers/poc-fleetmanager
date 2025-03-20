namespace POC.FleetManager.Common.Events
{
    public record DriverRegistrationEvent(Dictionary<string, object> Payload) : EventData(nameof(DriverRegistrationEvent), Payload);
}

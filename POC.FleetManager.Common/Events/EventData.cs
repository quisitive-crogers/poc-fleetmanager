namespace POC.FleetManager.Common.Events;

public record EventData(string EventType, Dictionary<string, object> Payload, Guid EventId = default)
{
}

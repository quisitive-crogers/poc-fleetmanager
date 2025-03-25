namespace POC.FleetManager.Common.Events;

public record EventData(string EventType, Guid EventId = default, params (string Key, object Value)[] Payload)
{
}

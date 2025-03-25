namespace POC.FleetManager.Common.Events;

public record DriverRegistrationEvent(params (string, object)[] Payload) : EventData(nameof(DriverRegistrationEvent), default, Payload);

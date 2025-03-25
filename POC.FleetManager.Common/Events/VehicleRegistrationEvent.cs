namespace POC.FleetManager.Common.Events;

public record VehicleRegistrationEvent(params (string, object)[] Payload) : EventData(nameof(VehicleRegistrationEvent), default, Payload);

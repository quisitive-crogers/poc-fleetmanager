namespace POC.FleetManager.Common.Events;
public record ReservationEvent(params (string, object)[] Payload) : EventData(nameof(ReservationEvent), default, Payload);

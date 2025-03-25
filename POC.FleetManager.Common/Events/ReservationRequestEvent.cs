namespace POC.FleetManager.Common.Events;

public record ReservationRequestEvent(params (string, object)[] Payload) : EventData(nameof(ReservationRequestEvent), default, Payload);

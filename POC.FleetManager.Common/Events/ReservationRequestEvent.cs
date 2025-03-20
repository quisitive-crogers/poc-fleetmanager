namespace POC.FleetManager.Common.Events;

public record ReservationRequestEvent(Dictionary<string, object> Payload) : EventData(nameof(ReservationRequestEvent), Payload);

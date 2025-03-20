namespace POC.FleetManager.Common.Events;
public record ReservationEvent(Dictionary<string, object> Payload) : EventData(nameof(ReservationEvent), Payload);

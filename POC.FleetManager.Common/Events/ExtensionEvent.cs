namespace POC.FleetManager.Common.Events;

    public record ExtensionEvent(params (string, object)[] Payload):EventData(nameof(ExtensionEvent), default, Payload)
    {
    }

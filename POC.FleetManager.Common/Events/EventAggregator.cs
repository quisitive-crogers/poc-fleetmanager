namespace POC.FleetManager.Common.Events;

public class EventAggregator : IEventAggregator
{
    private readonly Dictionary<string, List<Func<EventData, Task>>> subscribers = [];

    public void Subscribe(string type, Func<EventData, Task> handler)
    {
        var hasHandlers = subscribers.TryGetValue(type, out List<Func<EventData, Task>>? handlers);

        if (!hasHandlers)
        {
            handlers = [];
            subscribers.Add(type, handlers);
        }

        handlers!.Add(handler);
    }

    public async Task Publish(EventData eventData)
    {
        var handlers = subscribers.GetValueOrDefault(eventData.EventType);
        if (handlers?.Count > 0)
        {
            await Task.WhenAll(handlers.Select(handler => handler(eventData)));
        }
    }
}

namespace POC.FleetManager.Common
{
    public class EventAggregator
    {
        private readonly List<Action<object>> _handlers = new();

        public void Subscribe(Action<object> handler) => _handlers.Add(handler);
        public void Publish(object eventData) => _handlers.ForEach(h => h(eventData));
    }

    public record FleetEvent(string Message);
}

namespace POC.FleetManager.Common
{
    public class EventAggregator
    {
        private readonly Dictionary<string, List<Action<EventData>>> subscribers = [];

        public void Subscribe(string type, Action<EventData> handler)
        {
            var hasHandlers = subscribers.TryGetValue(type, out List<Action<EventData>>? handlers);

            if (!hasHandlers)
            {
                subscribers.Add(type, []);
                handlers = [];
            }

            handlers!.Add(handler);
        }

        public void Publish(EventData eventData)
        {
            if(subscribers.TryGetValue(eventData.EventType, out List<Action<EventData>>? handlers))
                            handlers.ForEach(h => h(eventData));
        }
    }
}

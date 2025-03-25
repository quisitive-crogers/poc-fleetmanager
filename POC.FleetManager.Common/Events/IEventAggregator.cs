
namespace POC.FleetManager.Common.Events;

public interface IEventAggregator
{
    Task Publish(EventData eventData);
    void Subscribe(string type, Func<EventData, Task> handler);
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using POC.FleetManager.Common.Events;

namespace POC.FleetManager.Common;

public abstract class ExtensionBase(IConfiguration configuration, ILogger<IExtension> logger, EventAggregator eventAggregator) : IExtension
{
    public virtual string Name { get; set; } = "Extension Base";
    public virtual string Version { get; set; } = "0.0.0";
    public IConfiguration Configuration { get; } = configuration;
    public ILogger<IExtension> Logger { get; } = logger;
    public EventAggregator EventAggregator { get; } = eventAggregator;

    protected async Task PublishEvent(EventData eventData)
    {
        await EventAggregator.Publish(eventData);
    }

    protected void SubscribeToEvent(string type, Func<EventData, Task> target)
    {
        EventAggregator.Subscribe(type, target);
    }

    public abstract Task Initialize();
}

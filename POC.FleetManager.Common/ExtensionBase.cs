using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using POC.FleetManager.Common.Events;

namespace POC.FleetManager.Common;

public abstract class ExtensionBase : IExtension
{
    protected IConfiguration? configuration;
    protected ILogger<IExtension>? logger;
    protected IEventAggregator? eventAggregator;
    protected IServiceProvider? serviceProvider;

    public virtual string Name { get; set; } = "Extension Base";
    public virtual string Version { get; set; } = "0.0.0";

    public virtual void Initialize(IConfiguration configuration, ILogger<IExtension> logger, IServiceProvider serviceProvider, IEventAggregator eventAggregator)
    {
        this.configuration = configuration;
        this.logger = logger;
        this.eventAggregator = eventAggregator;
        this.serviceProvider = serviceProvider;
    }

    protected virtual void EnsureInitialized()
    {
        if (eventAggregator == null || configuration == null || serviceProvider == null || logger == null) throw new UninitializedException();
    }

    protected async Task PublishEvent(EventData eventData)
    {
        await eventAggregator!.Publish(eventData);
    }

    protected void SubscribeToEvent(string type, Func<EventData, Task> target)
    {
        if (eventAggregator == null) throw new UninitializedException();
        eventAggregator!.Subscribe(type, target);
    }

}

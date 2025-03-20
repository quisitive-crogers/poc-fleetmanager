using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.FleetManager.Common
{
    public abstract class ExtensionBase(IConfiguration configuration, ILogger<IExtension> logger, EventAggregator eventAggregator) : IExtension
    {
        public virtual string Name { get; set; } = "Extension Base";
        public virtual string Version { get; set; } = "0.0.0";
        public IConfiguration Configuration { get; } = configuration;
        public ILogger<IExtension> Logger { get; } = logger;
        public EventAggregator EventAggregator { get; } = eventAggregator;

        protected void PublishEvent(EventData eventData)
        {
            EventAggregator.Publish(eventData);
        }

        protected void SubscribeToEvent(string type, Action<EventData> target)
        {
            EventAggregator.Subscribe(type, target);
        }

        public abstract Task Initialize();
        public abstract Task Run();
    }
}

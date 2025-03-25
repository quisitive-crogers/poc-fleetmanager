using Microsoft.Extensions.Configuration;
using POC.FleetManager.Common.Events;
using Microsoft.Extensions.Logging;

namespace POC.FleetManager.Common;

public interface IExtension
{
    string Name { get; }
    string Version { get; }
    void Initialize(IConfiguration configuration, ILogger<IExtension> logger, IServiceProvider serviceProvider, IEventAggregator eventAggregator);
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace POC.FleetManager.Common
{
    public interface IExtension
    {
        string Name { get; }
        string Version { get; }
        Task Initialize(IConfiguration configuration, ILogger<IExtension> logger);

        Task Run();
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace POC.FleetManager.Common
{
    public interface IExtension
    {
        string Name { get; }
        string Version { get; }
        Task Initialize();
        Task Run();
    }
}

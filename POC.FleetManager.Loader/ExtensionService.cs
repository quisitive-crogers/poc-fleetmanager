using Microsoft.Extensions.Logging;
using System.Composition.Hosting;

namespace POC.FleetManager.Loader;

public class ExtensionService(WebServiceCatalog catalog, ILogger<ExtensionService> logger)
{
    public async Task<CompositionHost> LoadExtensionsAsync(string username)
    {
        await catalog.InitializeAsync();
        var config = new ContainerConfiguration();
        config.WithProvider(catalog);
        return config.CreateContainer();
    }
}
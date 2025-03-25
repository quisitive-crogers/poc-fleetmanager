using System.Composition.Hosting;
using System.Composition.Hosting.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Reflection;
using POC.FleetManager.Common.Events;

namespace POC.FleetManager.Loader;

public class WebServiceCatalog(HttpClient httpClient, WebServiceSettings webServiceSettings,
                               ExtensionHostService extensionHostService, ILogger<WebServiceCatalog> logger,
                               AuthService authService, EventAggregator eventAggregator) : ExportDescriptorProvider
{
    private readonly List<CompositionHost> _assemblyContainers = [];

    public async Task<CompositionHost> LoadExtensionsAsync()
    {
        try
        {
            var eventData = new ExtensionEvent(("Message", $"Loading extension manifest for {authService.GetUsername()}"));
            await eventAggregator.Publish(eventData);

            var url = $"{webServiceSettings.BaseUrl}/extensions?framework={this.GetTargetFramework().Replace(".NET ", "net")}-{this.GetTargetPlatform().ToLower()}";
            logger.LogInformation("Polling extensions from {Url}", url);

            // Get the Entra ID token
            var token = await authService.GetAccessTokenAsync();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetStringAsync(url);
            var manifest = JsonConvert.DeserializeObject<ExtensionManifest>(response);

            foreach (var ext in manifest!.Extensions)
            {

                eventData = new ExtensionEvent(("Message", $"Loading extension {ext.Id} for {authService.GetUsername()}"));
                await eventAggregator.Publish(eventData);

                // Always download the latest binary (no version field)
                var extensionUrl = webServiceSettings.BaseUrl + ext.Url;
                logger.LogInformation("Downloading extension {Id} from {Url}", ext.Id, extensionUrl);
                var dllBytes = await httpClient.GetByteArrayAsync(extensionUrl);

                // Load the assembly and create a CompositionHost
                var assembly = Assembly.Load(dllBytes);
                var config = new ContainerConfiguration().WithAssembly(assembly);
                var container = config.CreateContainer();
                _assemblyContainers.Add(container);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to initialize WebServiceCatalog");
        }

        var finalConfig = new ContainerConfiguration();
        finalConfig.WithProvider(this);
        var host = finalConfig.CreateContainer();
        extensionHostService.SetHost(host);
        
        var eventData1 = new ExtensionEvent(("Message", $"Extensions loaded!"));
        await eventAggregator.Publish(eventData1);

        return host;
    }

    public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor)
    {
        if (_assemblyContainers == null || _assemblyContainers.None())
        {
            return [];
        }

        var descriptors = new List<ExportDescriptorPromise>();
        foreach (var container in _assemblyContainers)
        {
            var exports = container.GetExports(contract.ContractType);
            foreach (var export in exports)
            {
                descriptors.Add(new ExportDescriptorPromise(
                    contract,
                    export.ToString(),
                    true,
                    () => [],
                    lifetimeContext => ExportDescriptor.Create((c, o) => export, new Dictionary<string, object> { { "Shared", lifetimeContext } })));
            }
        }

        return descriptors;
    }
}

public class ExtensionManifest
{
    public required List<ExtensionInfo> Extensions { get; set; }
}

public class ExtensionInfo
{
    public required string Id { get; set; }
    public required string Url { get; set; }
}
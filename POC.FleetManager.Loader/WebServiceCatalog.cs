using System.Composition.Hosting;
using System.Composition.Hosting.Core;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using POC.FleetManager.Common;

namespace POC.FleetManager.Loader
{
    public class WebServiceCatalog : ExportDescriptorProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _extensionsPath;
        private readonly WebServiceSettings _webServiceSettings;
        private readonly ILogger<WebServiceCatalog> _logger;
        private readonly AuthService _authService;
        private readonly string _username;
        private readonly string _framework;
        private readonly List<CompositionHost> _assemblyContainers;

        public WebServiceCatalog(HttpClient httpClient, WebServiceSettings webServiceSettings, ILogger<WebServiceCatalog> logger, AuthService authService, string username, string framework)
        {
            _httpClient = httpClient;
            _webServiceSettings = webServiceSettings;
            _logger = logger;
            _authService = authService;
            _username = username;
            _framework = framework;
            _extensionsPath = Path.Combine(FileSystem.AppDataDirectory, "Extensions");
            Directory.CreateDirectory(_extensionsPath);
            _assemblyContainers = [];
        }

        public async Task InitializeAsync()
        {
            try
            {
                var url = $"{_webServiceSettings.BaseUrl}/extensions?framework={_framework}";
                _logger.LogInformation("Polling extensions from {Url}", url);

                // Get the Entra ID token
                var token = await _authService.GetAccessTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetStringAsync(url);
                var manifest = JsonConvert.DeserializeObject<ExtensionManifest>(response);

                foreach (var ext in manifest!.Extensions)
                {
                    var dllPath = Path.Combine(_extensionsPath, $"{ext.Id}.dll");
                    if (!File.Exists(dllPath))
                    {
                        _logger.LogInformation("Downloading extension {Id} from {Url}", ext.Id, ext.Url);
                        var dllBytes = await _httpClient.GetByteArrayAsync(ext.Url);
                        File.WriteAllBytes(dllPath, dllBytes);
                    }

                    // Load the assembly and create a CompositionHost
                    var assembly = Assembly.LoadFrom(dllPath);
                    var config = new ContainerConfiguration().WithAssembly(assembly);
                    var container = config.CreateContainer();
                    _assemblyContainers.Add(container);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize WebServiceCatalog for user {Username}", _username);
            }
        }

        public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor)
        {
            if (_assemblyContainers == null || _assemblyContainers.Count == 0)
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
}
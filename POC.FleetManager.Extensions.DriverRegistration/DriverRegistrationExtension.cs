using System.Composition;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using POC.FleetManager.Common;
using DbUp;

namespace POC.FleetManager.Extensions.DriverRegistration
{
    [Export(typeof(IExtension))]
    public class DriverRegistrationExtension : IExtension
    {
        private readonly ILogger<IExtension> _logger;
        private readonly IConfiguration _configuration;

        public DriverRegistrationExtension()
        {
        }

        public string Name => "Driver Registration";

        public async Task Initialize(IConfiguration configuration, ILogger<IExtension> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _logger.LogInformation("Initializing Driver Registration Extension");

            var connectionString = _configuration.GetConnectionString("DriverDb");
            var upgrader = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(System.Reflection.Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();
            if (!result.Successful)
            {
                _logger.LogError(result.Error, "Failed to initialize Driver Registration database schema");
                throw result.Error;
            }
            _logger.LogInformation("Driver Registration database schema initialized");
        }

        public async Task Run()
        {
            _logger.LogInformation("Driver Registration Extension running");
            await Task.CompletedTask;
        }
    }
}

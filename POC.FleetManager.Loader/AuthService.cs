using Microsoft.Identity.Client;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace POC.FleetManager.Loader
{
    public class AuthService
    {
        private readonly IPublicClientApplication _app;
        private readonly ILogger<AuthService> _logger;

        public AuthService(EntraIdSettings settings, ILogger<AuthService> logger)
        {
            _logger = logger;
            _app = PublicClientApplicationBuilder.Create(settings.ClientId)
                .WithAuthority(settings.Authority)
                .WithDefaultRedirectUri()
                .Build();
        }

        public async Task<bool> AuthenticateAsync()
        {
            try
            {
                var scopes = new[] { "User.Read" };
                var accounts = await _app.GetAccountsAsync();
                var account = accounts.FirstOrDefault();

                if (account != null)
                {
                    var result = await _app.AcquireTokenSilent(scopes, account).ExecuteAsync();
                    _logger.LogInformation("Authenticated silently.");
                    return true;
                }

                var interactiveResult = await _app.AcquireTokenInteractive(scopes).ExecuteAsync();
                _logger.LogInformation("Authenticated interactively.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Authentication failed.");
                return false;
            }
        }
    }
}

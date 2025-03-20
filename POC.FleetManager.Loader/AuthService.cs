using Microsoft.Identity.Client;
using Microsoft.Extensions.Logging;

namespace POC.FleetManager.Loader;

public class AuthService(EntraIdSettings settings, ILogger<AuthService> logger)
{
    private readonly IPublicClientApplication _app = PublicClientApplicationBuilder.Create(settings.ClientId)
            .WithAuthority(settings.Authority)
            .WithDefaultRedirectUri()
            .Build();
    private string? _accessToken;

    public async Task<bool> AuthenticateAsync()
    {
        try
        {
            var scopes = settings.Scopes;
            var accounts = await _app.GetAccountsAsync();
            var account = accounts.FirstOrDefault();

            if (account != null)
            {
                var result = await _app.AcquireTokenSilent(scopes, account).ExecuteAsync();
                _accessToken = result.AccessToken;
                logger.LogInformation("Authenticated silently.");
                return true;
            }

            var interactiveResult = await _app.AcquireTokenInteractive(scopes).ExecuteAsync();
            _accessToken = interactiveResult.AccessToken;
            logger.LogInformation("Authenticated interactively.");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Authentication failed.");
            return false;
        }
    }

    public async Task<string> GetAccessTokenAsync()
    {
        if (string.IsNullOrEmpty(_accessToken))
        {
            await AuthenticateAsync();
        }
        return _accessToken!;
    }
}

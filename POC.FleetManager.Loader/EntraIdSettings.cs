namespace POC.FleetManager.Loader;

public class EntraIdSettings
{
    public required string ClientId { get; set; }
    public required string Authority { get; set; }
    public required string[] Scopes{ get; set; }
}

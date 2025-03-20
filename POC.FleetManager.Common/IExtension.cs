namespace POC.FleetManager.Common;

public interface IExtension
{
    string Name { get; }
    string Version { get; }
    Task Initialize();
}

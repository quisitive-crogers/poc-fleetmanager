using System.Composition.Hosting;
using POC.FleetManager.Common;

namespace POC.FleetManager.Loader;
public class ExtensionHostService
{
    private CompositionHost? _host;

    public void SetHost(CompositionHost host)
    {
        _host = host;
    }

    public IEnumerable<IExtension> GetExtensions()
    {
        if (_host == null)
        {
            throw new InvalidOperationException("CompositionHost has not been set. Call SetHost first.");
        }
        return _host.GetExports<IExtension>();
    }
}
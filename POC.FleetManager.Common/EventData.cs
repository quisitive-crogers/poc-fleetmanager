using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.FleetManager.Common
{
    public record EventData(string EventType, Dictionary<string, object> Payload, Guid EventId = default)
    {
    }
}

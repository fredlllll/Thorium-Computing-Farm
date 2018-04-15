using System;

namespace Thorium_Services_Shared
{
    /// <summary>
    /// on lookup it determines where to look for a service definition
    /// on register it determines where the service is registered
    /// </summary>
    [Flags]
    public enum ServiceTenancy
    {
        Local = 0x1,
        Remote = 0x2,
        Both = Local | Remote
    }
}

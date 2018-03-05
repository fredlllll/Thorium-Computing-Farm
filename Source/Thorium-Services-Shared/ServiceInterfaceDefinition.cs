using Newtonsoft.Json.Linq;
using Thorium_Utils;

namespace Thorium_Services_Shared
{
    public class ServiceInterfaceDefinition
    {
        public string Id { get; }
        public string Name { get; }
        public JObject InterfaceInfo { get; }

        public ServiceInterfaceDefinition(string name, JObject interfaceInfo)
        {
            Id = Utils.GetRandomGUID();
            Name = name;
            InterfaceInfo = interfaceInfo;
        }

        public ServiceInterfaceDefinition(string id, string name, JObject interfaceInfo)
        {
            Id = id;
            Name = name;
            InterfaceInfo = interfaceInfo;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Services
{
    public class ServiceInterfaceDefinition
    {
        public string Id { get; }
        public string Name { get; }
        public JObject InterfaceInfo { get; }

        public ServiceInterfaceDefinition(string name, JObject interfaceInfo)
        {
            Id = Utils.GetRandomID();
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

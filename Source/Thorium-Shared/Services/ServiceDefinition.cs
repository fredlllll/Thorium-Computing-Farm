using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Services
{
    public class ServiceDefinition
    {
        public string Id { get; }
        public string Name { get; }
        public ServiceInterfaceDefinition[] InterfaceDefinitions { get; }

        public ServiceDefinition(string name, ServiceInterfaceDefinition[] interfaceDefinitions)
        {
            Id = Utils.GetRandomID();
            Name = name;
            InterfaceDefinitions = interfaceDefinitions;
        }

        public ServiceDefinition(string id, string name, ServiceInterfaceDefinition[] interfaceDefinitions)
        {
            Id = id;
            Name = name;
            InterfaceDefinitions = interfaceDefinitions;
        }
    }
}

using Thorium_Utils;

namespace Thorium_Services_Shared
{
    public class ServiceDefinition
    {
        public string Id { get; }
        public string Name { get; }
        public ServiceInterfaceDefinition[] InterfaceDefinitions { get; }

        public ServiceDefinition(string name, ServiceInterfaceDefinition[] interfaceDefinitions)
        {
            Id = Utils.GetRandomGUID();
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
